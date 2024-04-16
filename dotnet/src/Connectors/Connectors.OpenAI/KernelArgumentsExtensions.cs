// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;

namespace Microsoft.SemanticKernel.Connectors.OpenAI;

public static class KernelArgumentsExtensions
{
    /// <summary>
    /// 这里的ModelId是ModelService设置时候的ModelAlias
    /// </summary>
    /// <param name="modelId">这里的ModelId是ModelService设置时候的ModelAlias</param>
    /// <returns></returns>
    public static PromptExecutionSettings Set(this KernelArguments kernelArguments, string modelId, PromptExecutionSettings promptExecutionSettings)
    {
        var openAIPromptExecutionSettings = OpenAIPromptExecutionSettings.FromExecutionSettings(promptExecutionSettings);
        openAIPromptExecutionSettings.ModelId = modelId; //设置ModelAlias
        kernelArguments.SetDefaultPromptExecutionSettings(openAIPromptExecutionSettings);
        return openAIPromptExecutionSettings;
    }
}
