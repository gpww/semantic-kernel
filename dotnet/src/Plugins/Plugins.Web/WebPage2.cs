// Copyright (c) Microsoft. All rights reserved.

using System.Text.Json.Serialization;

namespace Microsoft.SemanticKernel.Plugins.Web;
public sealed partial class WebPage : IRelevantContent
{
    public string Text => this.Snippet;

    [JsonPropertyName("InnerText")]
    public string InnerText { get; set; } = string.Empty;

    [JsonPropertyName("Html")]
    public string Html { get; set; } = string.Empty;

    public float Relevance { get; set; } = 0;

    public override string ToString()
    {
        return string.Join("\n", this.Name, this.Url, this.Snippet, this.InnerText);
    }
}
