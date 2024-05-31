// Copyright (c) Microsoft. All rights reserved.

using System;

namespace Microsoft.SemanticKernel.Connectors.OpenAI;
public static class KernelFunctionFromPromptExtensions
{
    /// <summary>
    /// 默认采用func.MaxTokens，如果参数输入maxTokens小于func.MaxTokens，则取小者
    /// </summary>
    public static OpenAIPromptExecutionSettings GetOpenAIPromptExecutionSettings(this KernelFunctionFromPrompt func, int maxTokens = int.MaxValue)
    {
        return OpenAIPromptExecutionSettings.FromExecutionSettings(func.DefaultPromptExecutionSettings, Math.Min(func.MaxTokens, maxTokens));
    }
}
