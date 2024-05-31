// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.SemanticKernel;

/// <summary>
/// Represents image content.
/// </summary>
public sealed class ImageContent : KernelContent
{
    /// <summary>
    /// The URI of image.
    /// </summary>
    public Uri? Uri { get; set; }
    private readonly string _absoluteUri;

    /// <summary>
    /// The image data.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ReadOnlyMemory<byte>? Data { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageContent"/> class.
    /// </summary>
    [JsonConstructor]
    public ImageContent()
    {
    }

    public ImageContent(string absoluteUri, ReadOnlyMemory<byte> data) : this(data, innerContent: absoluteUri)
    {
        this._absoluteUri = absoluteUri;
    }
    public int Width { get; set; }
    public int Height { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageContent"/> class.
    /// </summary>
    /// <param name="uri">The URI of image.</param>
    /// <param name="modelId">The model ID used to generate the content</param>
    /// <param name="innerContent">Inner content</param>
    /// <param name="metadata">Additional metadata</param>
    public ImageContent(
        Uri uri,
        string? modelId = null,
        object? innerContent = null,
        IReadOnlyDictionary<string, object?>? metadata = null)
        : base(innerContent, modelId, metadata)
    {
        this.Uri = uri;
        this._absoluteUri = uri.AbsoluteUri;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageContent"/> class.
    /// </summary>
    /// <param name="data">The Data used as DataUri for the image.</param>
    /// <param name="modelId">The model ID used to generate the content</param>
    /// <param name="innerContent">Inner content</param>
    /// <param name="metadata">Additional metadata</param>
    public ImageContent(
        ReadOnlyMemory<byte> data,
        string? modelId = null,
        object? innerContent = null,
        IReadOnlyDictionary<string, object?>? metadata = null)
        : base(innerContent, modelId, metadata)
    {
        if (data!.IsEmpty)
        {
            throw new ArgumentException("Data cannot be empty", nameof(data));
        }

        this.Data = data;
    }

    /// <summary>
    /// Returns the string representation of the image.
    /// In-memory images will be represented as DataUri
    /// Remote Uri images will be represented as is
    /// </summary>
    /// <remarks>
    /// When Data is provided it takes precedence over URI
    /// </remarks>
    public override string ToString()
    {
        return this._absoluteUri ?? this.Uri?.ToString() ?? this.BuildDataUri() ?? string.Empty;
    }
    //正在 Microsoft.SemanticKernel.Connectors.OpenAI.ClientCore 第 978 行调用

    private string? BuildDataUri()
    {
        if (this.Data is null || string.IsNullOrEmpty(this.MimeType))
        {
            return null;
        }

        return $"data:{this.MimeType};base64,{Convert.ToBase64String(this.Data.Value.ToArray())}";
    }
}
