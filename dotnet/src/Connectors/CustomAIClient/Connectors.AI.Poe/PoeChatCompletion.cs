// Copyright (c) Microsoft. All rights reserved.

namespace Connectors.AI.Poe;

public class PoeChatCompletion : CustomChatCompletion
{
    public PoeChatCompletion(
        string modelId,
        string serviceName,
        string apiKey, string pythonExecutablePath,
        string proxy) : base(serviceName, modelId)
    {
        this._myClient = new PoeClient(modelId, apiKey, pythonExecutablePath, proxy);
    }
}
