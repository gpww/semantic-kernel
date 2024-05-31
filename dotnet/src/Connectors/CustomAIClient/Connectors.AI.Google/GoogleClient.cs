// Copyright (c) Microsoft. All rights reserved.

using System.Runtime.CompilerServices;
using System.Text;
using GenerativeAI.Helpers;
using GenerativeAI.Models;
using GenerativeAI.Types;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
namespace Connectors.AI.Google;

public class GoogleClient : CustomAIClient
{
    private readonly GenerativeModel _generativeModel;
    public GoogleClient(string modelId, string apiKey, HttpClient? httpClient) : base(modelId, apiKey)
    {
        this._generativeModel = new GenerativeModel(apiKey, client: httpClient);
    }
    public override Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(string modelId, IList<string> data, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings executionSettings, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        var buffer = new StringBuilder();
        await foreach (var chunk in this.GetStreamingChatMessageContentsAsync(chatHistory, executionSettings, kernel, cancellationToken).ConfigureAwait(false))
        {
            buffer.Append(chunk.Content);
        }
        return [new ChatMessageContent(AuthorRole.Assistant, buffer.ToString())];
    }

    private Part ToImagePart(ImageContent imageContent)
    {
        return new Part()
        {
            InlineData = new GenerativeContentBlob()
            {
                MimeType = "image/png",
                Data = Convert.ToBase64String(imageContent.Data.Value.ToArray())
            }
        };
    }
    private Content[] ToContents(ChatHistory chatHistory)
    {
        var contents = new List<Content>();
        foreach (var m in chatHistory)
        {
            var content = RequestExtensions.FormatGenerateContentInput(m.Items.Select(i => i switch
            {
                TextContent textContent => new Part() { Text = m.Content },
                ImageContent imageContent => this.ToImagePart(imageContent),
                _ => throw new NotSupportedException($"Unsupported chat message content type '{m.GetType()}'.")
            }), this.ToRoles(m.Role));

            contents.Add(content);
        }
        return contents.ToArray();
    }
    private GenerationConfig ToGenerationConfig(PromptExecutionSettings promptExecutionSettings)
    {
        var settings = promptExecutionSettings as OpenAIPromptExecutionSettings ?? new OpenAIPromptExecutionSettings();
        return new GenerationConfig
        {
            CandidateCount = settings.ResultsPerPrompt,
            Temperature = settings.Temperature,
            MaxOutputTokens = settings.MaxTokens,
            TopP = settings.TopP
        };
    }
    private string ToRoles(AuthorRole authorRole)
    {
        if (authorRole == AuthorRole.User || authorRole == AuthorRole.System)
        {
            return Roles.User;
        }
        else if (authorRole == AuthorRole.Assistant)
        {
            return Roles.Model;
        }
        else if (authorRole == AuthorRole.Tool)
        {
            return Roles.Function;
        }
        return Roles.User;
    }

    public override async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings executionSettings, Kernel? kernel = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GenerateContentRequest
        {
            Contents = this.ToContents(chatHistory),
            GenerationConfig = this.ToGenerationConfig(executionSettings)
        };

        await foreach (var chunk in this._generativeModel.GetStreamContentAsync(_modelId, request, cancellationToken).ConfigureAwait(false))
        {
            if (string.IsNullOrEmpty(chunk))
            {
                continue;
            }
            yield return new StreamingChatMessageContent(AuthorRole.Assistant, chunk);
        }
    }

    public override IAsyncEnumerable<string> GetStreamingTextContentsAsync(string text, OpenAIPromptExecutionSettings requestSettings, Kernel? kernel, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task<string> GetTextContentsAsync(string text, OpenAIPromptExecutionSettings requestSettings, Kernel kernel, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
