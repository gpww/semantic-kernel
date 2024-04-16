// Copyright (c) Microsoft. All rights reserved.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Microsoft.SemanticKernel.Plugins.Web;
[SuppressMessage("Performance", "CA1812:Internal class that is apparently never instantiated",
    Justification = "Class is instantiated through deserialization.")]
public sealed class WebPage : IRelevantContent
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("snippet")]
    public string Snippet { get; set; } = string.Empty;

    public string Text => this.Snippet;

    [JsonPropertyName("InnerText")]
    public string InnerText { get; set; } = string.Empty;

    [JsonPropertyName("Html")]
    public string Html { get; set; } = string.Empty;

    public double Relevance { get; set; } = 0;

    public override string ToString()
    {
        return string.Join("\n", this.Name, this.Url, this.Snippet, this.InnerText);
    }
}

public interface IRelevantContent
{
    public string Text { get; }
    public double Relevance { get; set; }
}
