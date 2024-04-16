// Copyright (c) Microsoft. All rights reserved.

namespace Microsoft.SemanticKernel;

public abstract partial class KernelPlugin
{
    public abstract bool SetModelServiceName(string functionName, string modelServiceName);
    public abstract string? GetModelServiceName(string functionName);
}
