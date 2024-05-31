// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Connectors.AI.ChatGLM;

public class GlmChatCompletion : CustomChatCompletion
{
    public GlmChatCompletion(
        string modelId,
         string serviceName,
        string apiKey,
        string pythonExecutablePath) : base(serviceName, modelId)
    {
        this._myClient = new GlmClient(modelId, apiKey, pythonExecutablePath);
    }

    public override ChatHistory CreateNewChat(string? instructions = null)
    {
        var chatHistory = new CustomChatHistory();

        if (!string.IsNullOrWhiteSpace(instructions))
        {
            chatHistory.Add(new ChatMessageContent(AuthorRole.User, instructions));
            chatHistory.Add(new ChatMessageContent(AuthorRole.Assistant, "好的，知道了！"));
        }

        return chatHistory;
    }

    //public Task<IReadOnlyList<IChatResult>> GetChatCompletionsAsync(ChatHistory chat, AIRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
    //{
    //    var openAIRequestSettings = GetRequestSettings(requestSettings);

    //    var reply = this._myClient.Invoke(chat.ToString(), openAIRequestSettings.Temperature, openAIRequestSettings.TopP);
    //    return Task.FromResult<IReadOnlyList<IChatResult>>(new List<IChatResult>
    //    {
    //        new GlmChatResult(AuthorRole.Assistant, reply)
    //    });
    //}

    //private static OpenAIRequestSettings GetRequestSettings(AIRequestSettings? requestSettings)
    //{
    //    if (requestSettings == null) return new OpenAIRequestSettings();
    //    else if(requestSettings is OpenAIRequestSettings) return requestSettings as OpenAIRequestSettings;
    //    else return new OpenAIRequestSettings()
    //    {
    //        Temperature = requestSettings.GetTemperature(),
    //        TopP = requestSettings.GetTopP()
    //    };
    //}

    //public Task<IReadOnlyList<ITextResult>> GetCompletionsAsync(string text, AIRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
    //{
    //    var openAIRequestSettings = GetRequestSettings(requestSettings);

    //    var reply = this._glmClient.Invoke(text, openAIRequestSettings.Temperature, openAIRequestSettings.TopP);
    //    return Task.FromResult<IReadOnlyList<ITextResult>>(new List<ITextResult>
    //    {
    //        new GlmChatResult(AuthorRole.Assistant, reply)
    //    });
    //}
    //public IAsyncEnumerable<IChatStreamingResult> GetStreamingChatCompletionsAsync(ChatHistory chat, AIRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
    //{
    //    var openAIRequestSettings = GetRequestSettings(requestSettings);

    //    var stream = this._glmClient.InvokeSSE(chat.ToString(), openAIRequestSettings.Temperature, openAIRequestSettings.TopP, cancellationToken);
    //    return (new List<IChatStreamingResult>
    //    {
    //        new GlmChatResult(AuthorRole.Assistant, stream)
    //    }).ToAsyncEnumerable();
    //}
    //public IAsyncEnumerable<ITextStreamingResult> GetStreamingCompletionsAsync(string text, AIRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
    //{
    //    var openAIRequestSettings = GetRequestSettings(requestSettings);

    //    var stream = this._glmClient.InvokeSSE(text, openAIRequestSettings.Temperature, openAIRequestSettings.TopP, cancellationToken);
    //    return (new List<ITextStreamingResult>
    //    {
    //        new GlmChatResult(AuthorRole.Assistant, stream)
    //    }).ToAsyncEnumerable();
}
