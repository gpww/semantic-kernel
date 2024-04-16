// Copyright (c) Microsoft. All rights reserved.

using System;
using Azure.AI.OpenAI;

namespace Microsoft.SemanticKernel.Connectors.AI.OpenAI.CustomClient;
internal class OpenAIClientEx: OpenAIClient
{
    public OpenAIClientEx(string openAIApiKey, OpenAIClientOptions options, string? endPoint = null)
    : base(new Uri(string.IsNullOrWhiteSpace(endPoint) ? PublicOpenAIEndpoint : endPoint.TrimEnd('/')), CreateDelegatedToken(openAIApiKey), options)
    {
        this._isConfiguredForAzureOpenAI = false;
    }
}
