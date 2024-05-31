// Copyright (c) Microsoft. All rights reserved.

using System;

namespace Microsoft.SemanticKernel;
public partial class Kernel
{
    /// <summary>
    /// Provides an event that's raised prior to a function's invocation.
    /// </summary>
    public event EventHandler<FunctionInvokingEventArgs>? FunctionInvoking;

    /// <summary>
    /// Provides an event that's raised after a function's invocation.
    /// </summary>
    public event EventHandler<FunctionInvokedEventArgs>? FunctionInvoked;

    /// <summary>
    /// Provides an event that's raised prior to a prompt being rendered.
    /// </summary>
    public event EventHandler<PromptRenderingEventArgs>? PromptRendering;

    /// <summary>
    /// Provides an event that's raised after a prompt is rendered.
    /// </summary>
    public event EventHandler<PromptRenderedEventArgs>? PromptRendered;

    internal FunctionInvokingEventArgs? OnFunctionInvoking(KernelFunction function, KernelArguments arguments)
    {
        FunctionInvokingEventArgs? eventArgs = null;
        if (this.FunctionInvoking is { } functionInvoking)
        {
            eventArgs = new(function, arguments);
            functionInvoking.Invoke(this, eventArgs);
        }

        return eventArgs;
    }

    internal FunctionInvokedEventArgs? OnFunctionInvoked(KernelFunction function, KernelArguments arguments, FunctionResult result)
    {
        FunctionInvokedEventArgs? eventArgs = null;
        if (this.FunctionInvoked is { } functionInvoked)
        {
            eventArgs = new(function, arguments, result);
            functionInvoked.Invoke(this, eventArgs);
        }

        return eventArgs;
    }

    internal PromptRenderingEventArgs? OnPromptRendering(KernelFunction function, KernelArguments arguments)
    {
        PromptRenderingEventArgs? eventArgs = null;
        if (this.PromptRendering is { } promptRendering)
        {
            eventArgs = new(function, arguments);
            promptRendering.Invoke(this, eventArgs);
        }

        return eventArgs;
    }

    internal PromptRenderedEventArgs? OnPromptRendered(KernelFunction function, KernelArguments arguments, string renderedPrompt)
    {
        PromptRenderedEventArgs? eventArgs = null;
        if (this.PromptRendered is { } promptRendered)
        {
            eventArgs = new(function, arguments, renderedPrompt);
            promptRendered.Invoke(this, eventArgs);
        }

        return eventArgs;
    }
}
