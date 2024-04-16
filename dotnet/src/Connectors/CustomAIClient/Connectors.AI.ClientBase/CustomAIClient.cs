using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Services;

namespace Connectors.AI;

public abstract class CustomAIClient
{
    protected CustomAIClient(string modelId)
    {
        this._modelId = modelId;
        this.Attributes.Add(AIServiceExtensions.ModelIdKey, modelId);
    }
    /// <summary>
    /// Storage for AI service attributes.
    /// </summary>
    internal Dictionary<string, object?> Attributes { get; } = new();

    public CustomAIClient(string modelId, string apiKey, string pythonExecutablePath) : this(modelId)
    {
        this._apiKey = apiKey;
        this._pythonExecutablePath = pythonExecutablePath;
    }
    protected readonly string _modelId;
    protected readonly string _apiKey;
    protected readonly string _pythonExecutablePath;

    public abstract Task<string> GetTextContentsAsync(string text, OpenAIPromptExecutionSettings requestSettings, Kernel kernel, CancellationToken cancellationToken);
    public abstract IAsyncEnumerable<string> GetStreamingTextContentsAsync(string text, OpenAIPromptExecutionSettings requestSettings, Kernel kernel, CancellationToken cancellationToken);

    public abstract IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default);

    public abstract Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default);

    public abstract Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(string modelId, IList<string> data, Kernel? kernel = null, CancellationToken cancellationToken = default);
}
