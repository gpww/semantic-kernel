// Copyright (c) Microsoft. All rights reserved.

using System;
using Azure.AI.OpenAI;

namespace Microsoft.SemanticKernel.Connectors.OpenAI.CustomClient;
/// <summary>
/// OpenAIClient是连接Azure和OpenAI的客户端，通过_isConfiguredForAzureOpenAI指定连哪个
/// 因为AzureSDK中无法修改，只能这里继承一个新的类
/// </summary>
public class OpenAIClient2 : OpenAIClient
{
    public OpenAIClient2(string openAIApiKey, OpenAIClientOptions options, string? endPoint = null)
    : base(new Uri(string.IsNullOrWhiteSpace(endPoint) ? PublicOpenAIEndpoint : CheckEndPoint(endPoint)), CreateDelegatedToken(openAIApiKey), options)
    {
        this._isConfiguredForAzureOpenAI = false;
        this._openAIApiKey = openAIApiKey;
    }
    private static string CheckEndPoint(string endPoint)
    {
        if (endPoint.EndsWith("/"))
        {
            endPoint = endPoint.TrimEnd('/');
        }

        if (!endPoint.EndsWith("/v1"))
        {
            endPoint += "/v1";
        }

        return endPoint;
    }

    private string _openAIApiKey;
}
