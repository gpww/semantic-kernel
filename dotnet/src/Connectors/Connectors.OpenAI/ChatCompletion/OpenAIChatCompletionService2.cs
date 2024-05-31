// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Services;
using Microsoft.SemanticKernel.TextGeneration;

namespace Microsoft.SemanticKernel.Connectors.OpenAI;

/// <summary>
/// OpenAI chat completion service.
/// </summary>
public partial class OpenAIChatCompletionService : IChatCompletionService, ITextGenerationService
{
    /// <summary>
    /// Create an instance of the OpenAI chat completion connector
    /// </summary>
    /// <param name="modelId">Model name</param>
    /// <param name="apiKey">OpenAI API Key</param>
    /// <param name="endPoint">Public OpenAI Endpoint, default = https://api.openai.com/v1</param>
    /// <param name="organization">OpenAI Organization Id (usually optional)</param>
    /// <param name="httpClient">Custom <see cref="HttpClient"/> for HTTP requests.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use for logging. If null, no logging will be performed.</param>
    public OpenAIChatCompletionService(
        string modelId,
        string apiKey,
        string endPoint = "https://api.openai.com/v1",
        string? organization = null,
        HttpClient? httpClient = null,
        ILoggerFactory? loggerFactory = null)
    {
        this._core = new(modelId, apiKey, endPoint, organization, httpClient, loggerFactory?.CreateLogger(typeof(OpenAIChatCompletionService)));

        this._core.AddAttribute(AIServiceExtensions.ModelIdKey, modelId);
        this._core.AddAttribute(OpenAIClientCore.OrganizationKey, organization);
    }
}
