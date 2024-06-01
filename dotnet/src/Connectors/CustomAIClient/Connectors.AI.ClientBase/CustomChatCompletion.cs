// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.TextGeneration;

namespace Connectors.AI;

public abstract class CustomChatCompletion : ITextGenerationService, IChatCompletionService
{
    protected CustomChatCompletion(string serviceName, string modelId)
    {
        this.ServiceName = serviceName;
    }
    protected CustomAIClient _myClient;
    public string ServiceName { get; set; }

    public IReadOnlyDictionary<string, object?> Attributes => this._myClient.Attributes;

    public virtual ChatHistory CreateNewChat(string? instructions = null)
    {
        return new CustomChatHistory(instructions);
    }

    private static OpenAIPromptExecutionSettings GetRequestSettings(PromptExecutionSettings? executionSettings)
    {
        if (executionSettings is OpenAIPromptExecutionSettings)
        {
            return executionSettings as OpenAIPromptExecutionSettings;
        }

        return new OpenAIPromptExecutionSettings();
    }

    public async Task<IReadOnlyList<TextContent>> GetTextContentsAsync(string prompt,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        var openAIRequestSettings = GetRequestSettings(executionSettings);

        var reply = await this._myClient.GetTextContentsAsync(prompt, openAIRequestSettings, kernel, cancellationToken).ConfigureAwait(true);

        return [new TextContent(reply)];
    }
    public async IAsyncEnumerable<StreamingTextContent> GetStreamingTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        var openAIRequestSettings = GetRequestSettings(executionSettings);

        var stream = this._myClient.GetStreamingTextContentsAsync(prompt, openAIRequestSettings, kernel, cancellationToken);

        await foreach (var chunk in stream.ConfigureAwait(false))
        {
            yield return new StreamingTextContent(chunk);
        }
    }

    public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        var openAIRequestSettings = GetRequestSettings(executionSettings);
        return await this._myClient.GetChatMessageContentsAsync(chatHistory, openAIRequestSettings, kernel, cancellationToken).ConfigureAwait(true);
    }

    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        var openAIRequestSettings = GetRequestSettings(executionSettings);

        var stream = this._myClient.GetStreamingChatMessageContentsAsync(chatHistory, openAIRequestSettings, kernel, cancellationToken);

        await foreach (var chunk in stream.ConfigureAwait(false))
        {
            yield return chunk;
        }
    }
}
