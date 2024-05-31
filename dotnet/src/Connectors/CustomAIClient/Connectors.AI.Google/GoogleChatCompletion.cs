// Copyright (c) Microsoft. All rights reserved.

namespace Connectors.AI.Google;

public class GoogleChatCompletion : CustomChatCompletion
{
    public GoogleChatCompletion(string modelId,
         string serviceName,
        string apiKey, HttpClient? httpClient) : base(serviceName, modelId)
    {
        this._myClient = new GoogleClient(modelId, apiKey, httpClient);
    }
}
