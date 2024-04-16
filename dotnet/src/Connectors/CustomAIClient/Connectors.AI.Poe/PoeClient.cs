// Copyright (c) Microsoft. All rights reserved.
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
    private IEnumerable<string> InvokeSSE(string message, double temperature, CancellationToken cancellationToken = default, string scriptPath = "PythonScript\\poe_client.py")
    {
        if (string.IsNullOrEmpty(message))
        {
            return null;
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

        return SystemHelper.GetLinesFromScriptOutput(this._pythonExecutablePath,
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, scriptPath),
            jsonString, cancellationToken);
    }

    public override Task<string> GetTextContentsAsync(string text, OpenAIPromptExecutionSettings requestSettings, Kernel kernel, CancellationToken cancellationToken)
    {
        var stream = this.InvokeSSE(text, requestSettings.Temperature, cancellationToken);
        var buffer = new StringBuilder();
        foreach (var chunk in stream)
        {
            buffer.Append(chunk);
        }
        return Task.FromResult<string>(buffer.ToString());
    }

    public override IAsyncEnumerable<string> GetStreamingTextContentsAsync(string text, OpenAIPromptExecutionSettings requestSettings, Kernel kernel, CancellationToken cancellationToken)
    {
        var stream = this.InvokeSSE(text, requestSettings.Temperature, cancellationToken);
        return stream.ToAsyncEnumerable();
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
