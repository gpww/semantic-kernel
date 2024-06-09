// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.TextToAudio;

namespace Connectors.AI;

public class CustomTextToAudioService : ITextToAudioService
{
    public CustomTextToAudioService(string serviceName)
    {
        this.ServiceName = serviceName;
    }
    protected CustomAIClient _myClient;
    public IReadOnlyDictionary<string, object?> Attributes => this._myClient.Attributes;

    public string ServiceName { get; set; }

    public async Task<IReadOnlyList<AudioContent>> GetAudioContentsAsync(string text, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        return [await this._myClient.GetAudioContentAsync(text, executionSettings, kernel, cancellationToken)];
    }
}
