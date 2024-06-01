// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel;
using TiktokenSharp;

namespace Connectors.AI;

public static class ChatMessageContentExtensions
{
    /// <summary>
    /// 计算消息的 token 数量
    /// </summary>
    public static int ComputeTokenCount(this ChatMessageContent message)
    {
        int totalTokens = 0;

        foreach (var item in message.Items)
        {
            totalTokens += item switch
            {
                TextContent textContent => TikToken.GetCount(textContent.Text),
                ImageContent imageContent => CalculateImageTokens(imageContent),
                _ => throw new NotSupportedException($"Unsupported chat message content type '{item.GetType()}'.")
            };
        }

        return totalTokens;
    }
    private static int CalculateImageTokens(ImageContent imageContent)
    {
        imageContent.ComputeWidthHeightAsync().Wait();
        return CalculateImageTokens(imageContent.Width, imageContent.Height);
    }
    private static int CalculateImageTokens(int imageWidth, int imageHeight)
    {
        const int BaseTokens = 85;
        const int TokensPerBlock = 170;
        const int BlockSize = 512;

        // Calculate the number of blocks by dividing the image size by the block size, rounding up
        int blocksWidth = (imageWidth + BlockSize - 1) / BlockSize;
        int blocksHeight = (imageHeight + BlockSize - 1) / BlockSize;

        // Total blocks is width blocks times height blocks
        int totalBlocks = blocksWidth * blocksHeight;

        // Total tokens is base tokens plus (tokens per block times total blocks)
        int totalTokens = BaseTokens + (TokensPerBlock * totalBlocks);

        return totalTokens;
    }
}
