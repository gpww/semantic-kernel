// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.Http;
using Microsoft.SemanticKernel.TextGeneration;

namespace Microsoft.SemanticKernel.Connectors.OpenAI;
public static class OpenAIServiceCollectionExtensions
{
    /// <summary>
    /// Add unified OpenAI chat completion service to the list.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="serviceKey">The Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey of the service.</param>
    /// <param name="modelId">OpenAI model name, see https://platform.openai.com/docs/models</param>
    /// <param name="apiKey">OpenAI API key, see https://platform.openai.com/account/api-keys</param>
    /// <param name="endPoint">Public OpenAI Endpoint, default = https://api.openai.com/v1</param>
    /// <param name="httpClient">The HttpClient to use with this service.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddOneAPITextGeneration(
        this IKernelBuilder builder,
        string serviceKey,
        string modelId,
        string apiKey,
        string? endPoint = "https://api.openai.com/v1",
        HttpClient? httpClient = null)
    {
        Verify.NotNull(builder);
        Verify.NotNullOrWhiteSpace(modelId);
        Verify.NotNullOrWhiteSpace(apiKey);

        OpenAIChatCompletionService factory(IServiceProvider serviceProvider, object? _)
        {
            httpClient = HttpClientProvider.GetHttpClient(httpClient, serviceProvider);
            var options = ClientCore.GetOpenAIClientOptions(httpClient);
            var client = new CustomClient.OpenAIClient2(apiKey, options, endPoint);

            var service = new OpenAIChatCompletionService(modelId, client, serviceProvider.GetService<ILoggerFactory>());
            service.ServiceName = serviceKey;
            return service;
        }

        builder.Services.AddKeyedSingleton<IChatCompletionService>(serviceKey, (Func<IServiceProvider, object?, OpenAIChatCompletionService>)factory);
        builder.Services.AddKeyedSingleton<ITextGenerationService>(serviceKey, (Func<IServiceProvider, object?, OpenAIChatCompletionService>)factory);

        return builder;
    }

    /// <summary>
    /// Build with AI Service of type TService
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder WithAIService<TService>(
    this IKernelBuilder builder, object? serviceKey, TService implementationInstance) where TService : class
    {
        builder.Services.AddKeyedSingleton<TService>(serviceKey, implementationInstance);
        return builder;
    }

    /// <summary>
    /// Adds the OpenAI text embeddings service to the list.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="serviceKey">The Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ServiceKey of the service.</param>
    /// <param name="modelId">OpenAI model name, see https://platform.openai.com/docs/models</param>
    /// <param name="apiKey">OpenAI API key, see https://platform.openai.com/account/api-keys</param>
    /// <param name="endPoint">Public OpenAI Endpoint, default = https://api.openai.com/v1</param>
    /// <param name="httpClient">The HttpClient to use with this service.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    [Experimental("SKEXP0011")]
    public static IKernelBuilder AddOneAPITextEmbedding(
        this IKernelBuilder builder,
        string serviceKey,
        string modelId,
        string apiKey,
        string? endPoint = "https://api.openai.com/v1",
        HttpClient? httpClient = null)
    {
        Verify.NotNull(builder);
        Verify.NotNullOrWhiteSpace(modelId);
        Verify.NotNullOrWhiteSpace(apiKey);

        OpenAITextEmbeddingGenerationService factory(IServiceProvider serviceProvider, object? _)
        {
            httpClient = HttpClientProvider.GetHttpClient(httpClient, serviceProvider);
            var options = ClientCore.GetOpenAIClientOptions(httpClient);
            var client = new CustomClient.OpenAIClient2(apiKey, options, endPoint);

            var service = new OpenAITextEmbeddingGenerationService(modelId, client, serviceProvider.GetService<ILoggerFactory>());
            service.ServiceName = serviceKey;
            return service;
        }

        builder.Services.AddKeyedSingleton<ITextEmbeddingGenerationService>(serviceKey, factory);

        return builder;
    }
}
