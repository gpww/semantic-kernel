﻿// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Azure.AI.OpenAI;
using Azure.Core;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Services;

namespace Microsoft.SemanticKernel.Connectors.OpenAI;

/// <summary>
/// Core implementation for OpenAI clients, providing common functionality and properties.
/// </summary>
internal partial class OpenAIClientCore : ClientCore
{
    /// <summary>
    /// Gets the attribute name used to store the organization in the <see cref="IAIService.Attributes"/> dictionary.
    /// </summary>
    public static string OrganizationKey => "Organization";

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenAIClientCore"/> class.
    /// </summary>
    /// <param name="modelId">Model name.</param>
    /// <param name="apiKey">OpenAI API Key.</param>
    /// <param name="organization">OpenAI Organization Id (usually optional).</param>
    /// <param name="httpClient">Custom <see cref="HttpClient"/> for HTTP requests.</param>
    /// <param name="logger">The <see cref="ILoggerFactory"/> to use for logging. If null, no logging will be performed.</param>
    internal OpenAIClientCore(
        string modelId,
        string apiKey,
        string? organization = null,
        HttpClient? httpClient = null,
        ILogger? logger = null) : this(modelId, apiKey, null, organization, httpClient, logger)
    {
    }
    /// <summary>
    /// OpenAI / Azure OpenAI Client
    /// </summary>
    internal override OpenAIClient Client { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenAIClientCore"/> class using the specified OpenAIClient.
    /// Note: instances created this way might not have the default diagnostics settings,
    /// it's up to the caller to configure the client.
    /// </summary>
    /// <param name="modelId">Azure OpenAI model ID or deployment name, see https://learn.microsoft.com/azure/cognitive-services/openai/how-to/create-resource</param>
    /// <param name="openAIClient">Custom <see cref="OpenAIClient"/>.</param>
    /// <param name="logger">The <see cref="ILoggerFactory"/> to use for logging. If null, no logging will be performed.</param>
    internal OpenAIClientCore(
        string modelId,
        OpenAIClient openAIClient,
        ILogger? logger = null) : base(logger)
    {
        Verify.NotNullOrWhiteSpace(modelId);
        Verify.NotNull(openAIClient);

        this.DeploymentOrModelName = modelId;
        this.Client = openAIClient;
    }

    /// <summary>
    /// Logs OpenAI action details.
    /// </summary>
    /// <param name="callerMemberName">Caller member name. Populated automatically by runtime.</param>
    internal void LogActionDetails([CallerMemberName] string? callerMemberName = default)
    {
        if (this.Logger.IsEnabled(LogLevel.Information))
        {
            this.Logger.LogInformation("Action: {Action}. OpenAI Model ID: {ModelId}.", callerMemberName, this.DeploymentOrModelName);
        }
    }
}
