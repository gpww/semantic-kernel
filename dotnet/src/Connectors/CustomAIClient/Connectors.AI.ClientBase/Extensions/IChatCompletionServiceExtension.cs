// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel.ChatCompletion;

namespace Connectors.AI;
public static class IChatCompletionServiceExtension
{
    public static ChatHistory CreateNewChat(this IChatCompletionService chatCompletionService, string? instructions = null)
    {
        ChatHistory _chatHistory = null;
        if (chatCompletionService is CustomChatCompletion cmpl)
        {
            _chatHistory = cmpl.CreateNewChat(instructions);
        }
        else
        {
            _chatHistory = new CustomChatHistory(instructions);
        }

        return _chatHistory;
    }
}
