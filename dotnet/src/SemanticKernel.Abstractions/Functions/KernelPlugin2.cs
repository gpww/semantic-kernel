// Copyright (c) Microsoft. All rights reserved.

using System.Threading.Tasks;

namespace Microsoft.SemanticKernel;

public abstract partial class KernelPlugin
{
    /// <summary>
    /// 是否发送到服务器端作为 FunctionCall候选
    /// plugin中这个属性默认为 false，主要提供给Kernel用，不发送OpenAI服务端
    /// Function 中的 FunctionCallAvailable 默认为 false
    /// </summary>
    public bool FunctionCallAvailable { get; set; } = false;

    public abstract bool SetModelServiceName(string functionName, string modelServiceName);
    public abstract string? GetModelServiceName(string functionName);

    public override string ToString()
    {
        return string.Format("{0}: {1}", this.Name, this.Description);
    }
    public virtual async Task UpdateDescriptionAsync()
    {
    }
}
