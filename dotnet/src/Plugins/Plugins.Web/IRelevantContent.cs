// Copyright (c) Microsoft. All rights reserved.

namespace Microsoft.SemanticKernel.Plugins.Web;
public interface IRelevantContent
{
    public string Text { get; }
    public double Relevance { get; set; }
}
