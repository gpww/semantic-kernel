// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Connectors.AI;

public static class ImageContentExtensions
{
    public static async Task ComputeWidthHeightAsync(this ImageContent imageContent)
    {
        // Check if Data is null
        if (imageContent.Data != null)// Calculate width and height from data
        {
            using var ms = new MemoryStream(imageContent.Data.Value.Span.ToArray());
            using var image = Image.Load<Rgba32>(ms);
            imageContent.Width = image.Width;
            imageContent.Height = image.Height;
        }
        if (imageContent.Uri != null)
        {
            using var httpClient = new HttpClient();
            var data = await httpClient.GetByteArrayAsync(imageContent.Uri).ConfigureAwait(false);
            using var stream = new MemoryStream(data);
            using var image = Image.Load<Rgba32>(stream);
            imageContent.Width = image.Width;
            imageContent.Height = image.Height;
        }
    }
}
