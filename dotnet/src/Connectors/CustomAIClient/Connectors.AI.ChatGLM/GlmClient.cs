// Copyright (c) Microsoft. All rights reserved.

using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Silver;

namespace Connectors.AI.ChatGLM;

public class GlmClient : CustomAIClient
{
    public GlmClient(string modelId, string apiKey, string pythonExecutablePath) : base(modelId, apiKey, pythonExecutablePath)
    {
    }
    //private readonly string _modelId = "chatglm_turbo";
    //private readonly string _apiKey = "f5ee8f8f18897ce66deb14a468ef3c30.K5qtVj1bvtWNJjpi";
    //private readonly string _pythonExecutablePath = "D:\\ProgramData\\anaconda3\\envs\\GLM\\python.exe";
    private async Task<string> InvokeAsync(string message, double temperature, double topP, string scriptPath = "PythonScript\\glm_client.py", CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(message))
        {
            return null;
        }

        string jsonString = this.GetJsonArgs("invoke", message, temperature, topP);

        return await SystemHelper.GetScriptOutput(this._pythonExecutablePath,
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, scriptPath),
            jsonString, cancellationToken).ConfigureAwait(false);
    }

    private string GetJsonArgs(string funcType, string message, double temperature, double topP)
    {
        var json = new
        {
            api_key = this._apiKey,
            model = this._modelId,
            prompt = StringHelper.IsJsonValidWithContent(message) ? JsonSerializer.Deserialize<object>(message) : message,
            func_type = funcType,
            temperature = (float)temperature,
            top_p = (float)topP
        };

        return JsonSerializer.Serialize(json);
    }

    private async IAsyncEnumerable<string> InvokeSSEAsync(string message, double temperature, double topP,
        [EnumeratorCancellation] CancellationToken cancellationToken = default, string scriptPath = "PythonScript\\glm_client.py")
    {
        if (string.IsNullOrEmpty(message))
        {
            yield break;
        }

        string jsonString = this.GetJsonArgs("sse_invoke", message, temperature, topP);

        await foreach (var line in SystemHelper.GetLinesFromScriptOutput(this._pythonExecutablePath,
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, scriptPath),
            jsonString, cancellationToken).ConfigureAwait(false))
        {
            yield return line;
        }
    }

    public override async Task<string> GetTextContentsAsync(string text, OpenAIPromptExecutionSettings executionSettings, Kernel kernel, CancellationToken cancellationToken)
    {
        //var reply = this.Invoke(text, requestSettings.Temperature, requestSettings.TopP);
        //return Task.FromResult<string>(reply);

        var stream = this.InvokeSSEAsync(text, executionSettings.Temperature, executionSettings.TopP, cancellationToken);
        var buffer = new StringBuilder();
        await foreach (var chunk in stream.ConfigureAwait(false))
        {
            buffer.Append(chunk);
        }
        return buffer.ToString();
    }

    public override IAsyncEnumerable<string> GetStreamingTextContentsAsync(string text, OpenAIPromptExecutionSettings requestSettings, Kernel kernel, CancellationToken cancellationToken)
    {
        return this.InvokeSSEAsync(text, requestSettings.Temperature, requestSettings.TopP, cancellationToken);
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
