// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Linq;

namespace Microsoft.SemanticKernel;

public partial class KernelFunctionFromPrompt
{
    private readonly PromptTemplateConfig _promptConfig = null;
    public string TemplateText
    {
        get => this._promptConfig.Template;
        set => this._promptConfig.Template = value;
    }
    public PromptExecutionSettings DefaultPromptExecutionSettings => this._promptConfig.DefaultExecutionSettings;

    public int MaxTokens
    {
        get
        {
            if (this.DefaultPromptExecutionSettings.ExtensionData != null &&
                this.DefaultPromptExecutionSettings.ExtensionData.TryGetValue("max_tokens", out var maxTokens))
            {
                return int.Parse(maxTokens.ToString());
            }
            throw new InvalidOperationException("max_tokens not found in DefaultPromptExecutionSettings");
        }
    }

    /// <summary>
    /// 设置唯一优选的AI服务（删除除默认外的所有其它服务）
    /// </summary>
    /// <param name="serviceName"></param>
    public void SetPreferredModelServiceName(string serviceName)
    {
        foreach (var key in this._promptConfig.ExecutionSettings.Keys)
        {
            if (key != PromptExecutionSettings.DefaultServiceId)
            {
                this._promptConfig.ExecutionSettings.Remove(key);//remove all except default
            }
        }

        this._promptConfig.AddExecutionSettings(new PromptExecutionSettings(), serviceName);
    }
    //找到除了DefaultServiceId的第一个服务（之前设置的）
    public string? GetPreferredModelServiceName()
    {
        if (this._promptConfig.ExecutionSettings.Count < 2)
        {
            return null;
        }
        return this._promptConfig.ExecutionSettings.Keys.Where(k => k != PromptExecutionSettings.DefaultServiceId).First();
    }
}
