// Copyright (c) Microsoft. All rights reserved.

using Azure.AI.OpenAI;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Microsoft.SemanticKernel.Connectors.OpenAI;

public static class ChatHistoryExtension
{
    public static string ToJsonString(this ChatHistory chatHistory)
    {
        var option = new ChatCompletionsOptions();

        foreach (var m in chatHistory)
        {
            option.Messages.Add(ClientCore.GetRequestMessage(new ChatRole(m.Role.Label), m.Content, m.AuthorName, null));
        }

        return option.GetMessagesJson();
    }
}
