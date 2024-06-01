// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;

namespace Microsoft.SemanticKernel;
public partial class KernelArguments
{
    public const string ChatHistoryJson = "chatHistoryJson";//发送给服务器的聊天记录Json表示
    public const string ToolsJson = "toolsJson";//发送给服务器的聊天记录Json表示

    public T Get<T>(string key)
    {
        var v = this[key];
        return v == null ? default : (T)Convert.ChangeType(v, typeof(T));
    }
    public bool IsEmpty(string key) => !this._arguments.ContainsKey(key) || this[key] == null || string.IsNullOrWhiteSpace(this[key].ToString());

    public void SetDefaultPromptExecutionSettings(PromptExecutionSettings promptExecutionSettings)
    {
        this.ExecutionSettings =
           new Dictionary<string, PromptExecutionSettings>()
           {
                {
                    PromptExecutionSettings.DefaultServiceId, promptExecutionSettings
                }
           };
    }

    public void AddExtensionData(PromptExecutionSettings promptExecutionSettings)
    {
        foreach (var item in promptExecutionSettings.ExtensionData)
        {
            this.Add(item.Key, item.Value);
        }
    }
}
