﻿// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.HuggingFace.Core;
using Microsoft.SemanticKernel.Http;
using Microsoft.SemanticKernel.Services;

namespace Microsoft.SemanticKernel.Connectors.HuggingFace;

/// <summary>
/// HuggingFace chat completion service.
/// </summary>
public sealed class HuggingFaceChatCompletionService : IChatCompletionService
{
    private Dictionary<string, object?> AttributesInternal { get; } = [];
    private HuggingFaceMessageApiClient Client { get; }
    public string ServiceName { get; set; }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?> Attributes => this.AttributesInternal;

    /// <summary>
    /// Initializes a new instance of the <see cref="HuggingFaceChatCompletionService"/> class.
    /// </summary>
    /// <param name="model">The HuggingFace model for the chat completion service.</param>
    /// <param name="endpoint">The uri endpoint including the port where HuggingFace server is hosted</param>
    /// <param name="apiKey">Optional API key for accessing the HuggingFace service.</param>
    /// <param name="httpClient">Optional HTTP client to be used for communication with the HuggingFace API.</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public HuggingFaceChatCompletionService(
        string model,
        Uri? endpoint = null,
        string? apiKey = null,
        HttpClient? httpClient = null,
        ILoggerFactory? loggerFactory = null)
    {
        Verify.NotNullOrWhiteSpace(model);

        var clientEndpoint = endpoint ?? httpClient?.BaseAddress
            ?? throw new ArgumentNullException(nameof(endpoint), "Chat completion service requires a valid endpoint provided explicitly or via HTTP client base address");

        this.Client = new HuggingFaceMessageApiClient(
            modelId: model,
            endpoint: clientEndpoint,
            apiKey: apiKey,
            httpClient: HttpClientProvider.GetHttpClient(httpClient),
            logger: loggerFactory?.CreateLogger(this.GetType()) ?? NullLogger.Instance
        );

        this.AttributesInternal.Add(AIServiceExtensions.ModelIdKey, model);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        => this.Client.CompleteChatMessageAsync(chatHistory, executionSettings, cancellationToken);

    /// <inheritdoc />
    public IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        => this.Client.StreamCompleteChatMessageAsync(chatHistory, executionSettings, cancellationToken);
}
