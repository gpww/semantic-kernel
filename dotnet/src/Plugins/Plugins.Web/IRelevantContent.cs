// Copyright (c) Microsoft. All rights reserved.

namespace Microsoft.SemanticKernel.Plugins.Web;
public interface IRelevantContent
{
    public string Text { get; }
    public float Relevance { get; set; }
}
