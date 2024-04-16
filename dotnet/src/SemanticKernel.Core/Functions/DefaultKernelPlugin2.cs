// Copyright (c) Microsoft. All rights reserved.

namespace Microsoft.SemanticKernel;

/// <summary>
/// Provides an <see cref="KernelPlugin"/> implementation around a collection of functions.
/// </summary>
public partial class DefaultKernelPlugin
{
    public void AddFunction(KernelFunction function, bool functionCallAvailable = false, string? funcName = null)
    {
        Verify.NotNull(function, nameof(function));
        this._functions.Add(funcName ?? function.Name, function);
        function.Metadata.FunctionCallAvailable = functionCallAvailable;
    }

    public override bool SetModelServiceName(string functionName, string modelServiceName)
    {
        if (this.Contains(functionName))
        {
            var function = this[functionName];
            if (function is KernelFunctionFromPrompt semanticFunc)
            {
                semanticFunc.SetPreferredModelServiceName(modelServiceName);
                return true;
            }
        }
        return false;
    }
    public override string? GetModelServiceName(string functionName)
    {
        if (this.Contains(functionName))
        {
            var function = this[functionName];
            if (function is KernelFunctionFromPrompt semanticFunc)
            {
                return semanticFunc.GetPreferredModelServiceName();
            }
        }
        return null;
    }
}
