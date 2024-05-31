// Copyright (c) Microsoft. All rights reserved.

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
    private string Invoke(string message, double temperature, double topP, CancellationToken cancellationToken = default, string scriptPath = "PythonScript\\glm_client.py")
    {
        if (string.IsNullOrEmpty(message))
        {
            return null;
        }

        string jsonString = this.GetJsonArgs("invoke", message, temperature, topP);

        return SystemHelper.GetScriptOutput(this._pythonExecutablePath,
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, scriptPath),
            jsonString, cancellationToken);
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

    private IEnumerable<string> InvokeSSE(string message, double temperature, double topP, CancellationToken cancellationToken = default, string scriptPath = "PythonScript\\glm_client.py")
    {
        if (string.IsNullOrEmpty(message))
        {
            return null;
        }

        string jsonString = this.GetJsonArgs("sse_invoke", message, temperature, topP);

        return SystemHelper.GetLinesFromScriptOutput(this._pythonExecutablePath,
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, scriptPath),
            jsonString, cancellationToken);
    }

    public override Task<string> GetTextContentsAsync(string text, OpenAIPromptExecutionSettings executionSettings, Kernel kernel, CancellationToken cancellationToken)
    {
        //var reply = this.Invoke(text, requestSettings.Temperature, requestSettings.TopP);
        //return Task.FromResult<string>(reply);

        var stream = this.InvokeSSE(text, executionSettings.Temperature, executionSettings.TopP, cancellationToken);
        var buffer = new StringBuilder();
        foreach (var chunk in stream)
        {
            buffer.Append(chunk);
        }
        return Task.FromResult<string>(buffer.ToString());
    }

    public override IAsyncEnumerable<string> GetStreamingTextContentsAsync(string text, OpenAIPromptExecutionSettings requestSettings, Kernel kernel, CancellationToken cancellationToken)
    {
        var stream = this.InvokeSSE(text, requestSettings.Temperature, requestSettings.TopP, cancellationToken);
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
