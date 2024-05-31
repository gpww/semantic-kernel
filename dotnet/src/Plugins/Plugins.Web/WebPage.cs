// Copyright (c) Microsoft. All rights reserved.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Microsoft.SemanticKernel.Plugins.Web;

/// <summary>
/// A sealed class containing the deserialized response from the respective Web Search API.
/// </summary>
/// <returns>A WebPage object containing the Web Search API response data.</returns>
[SuppressMessage("Performance", "CA1056:Change the type of parameter 'uri'...",
Justification = "A constant Uri cannot be defined, as required by this class")]
public sealed class WebPage
{
    /// <summary>
    /// The name of the result.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// The URL of the result.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
    /// <summary>
    /// The result snippet.
    /// </summary>
    [JsonPropertyName("snippet")]
    public string Snippet { get; set; } = string.Empty;
}

/// <summary>
/// A sealed class containing the deserialized response from the respective Web Search API.
/// </summary>
/// <returns>A WebPages? object containing the WebPages array from a Search API response data or null.</returns>
public sealed class WebSearchResponse
{
    /// <summary>
    /// A nullable WebPages object containing the Web Search API response data.
    /// </summary>
    [JsonPropertyName("webPages")]
    public WebPages? WebPages { get; set; }
}

/// <summary>
/// A sealed class containing the deserialized response from the Web respective Search API.
/// </summary>
/// <returns>A WebPages array object containing the Web Search API response data.</returns>
[SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Required by the Web Search API")]
public sealed class WebPages
{
    /// <summary>
    /// a nullable WebPage array object containing the Web Search API response data.
    /// </summary>
    [JsonPropertyName("value")]
    public WebPage[]? Value { get; set; }
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
