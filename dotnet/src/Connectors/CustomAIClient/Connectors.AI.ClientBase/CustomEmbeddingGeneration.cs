// Copyright (c) Microsoft. All rights reserved.

using System.Diagnostics.CodeAnalysis;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;

namespace Connectors.AI;

[Experimental("SKEXP0011")]
public abstract class CustomEmbeddingGeneration : ITextEmbeddingGenerationService
{
    protected CustomEmbeddingGeneration(string serviceName, string modelId)
    {
        this.ServiceName = serviceName;
    }
    protected CustomAIClient _myClient;
    public string ServiceName { get; set; }

    public IReadOnlyDictionary<string, object?> Attributes => this._myClient.Attributes;

    public abstract Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, Kernel? kernel = null, CancellationToken cancellationToken = default);
}
