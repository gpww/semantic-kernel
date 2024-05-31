// Copyright (c) Microsoft. All rights reserved.
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Silver;

namespace Connectors.AI.Poe;

public class PoeClient : CustomAIClient
{
    public PoeClient(string modelId, string apiKey, string pythonExecutablePath, string proxy) : base(modelId, apiKey, pythonExecutablePath)
    {
        this._proxy = proxy;
    }
    private readonly string _proxy;
    private async IAsyncEnumerable<string> InvokeSSEAsync(string message, double temperature, string scriptPath = "PythonScript\\poe_client.py", [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(message))
        {
            yield break;
        }

        var json = new
        {
            api_key = this._apiKey,
            model = this._modelId,
            proxy = this._proxy,
            prompt = StringHelper.IsJsonValidWithContent(message) ? JsonSerializer.Deserialize<object>(message) : message,
            temperature = (float)temperature
        };

        var jsonString = JsonSerializer.Serialize(json);

        await foreach (var line in SystemHelper.GetLinesFromScriptOutput(this._pythonExecutablePath,
             Path.Combine(AppDomain.CurrentDomain.BaseDirectory, scriptPath),
             jsonString, cancellationToken).ConfigureAwait(false))
        {
            yield return line;
        }
    }

    public override async Task<string> GetTextContentsAsync(string text, OpenAIPromptExecutionSettings requestSettings, Kernel kernel, CancellationToken cancellationToken)
    {
        var stream = this.InvokeSSEAsync(text, requestSettings.Temperature, cancellationToken: cancellationToken);
        var buffer = new StringBuilder();
        await foreach (var chunk in stream.ConfigureAwait(false))
        {
            buffer.Append(chunk);
        }
        return buffer.ToString();
    }

    public override IAsyncEnumerable<string> GetStreamingTextContentsAsync(string text, OpenAIPromptExecutionSettings requestSettings, Kernel kernel, CancellationToken cancellationToken)
    {
        return this.InvokeSSEAsync(text, requestSettings.Temperature, cancellationToken: cancellationToken);
    }

    public override Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(string modelId, IList<string> data, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
