// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AudioToText;

namespace Connectors.AI;

public class CustomAudioToTextService : IAudioToTextService
{
    public CustomAudioToTextService(string serviceName)
    {
        this.ServiceName = serviceName;
    }
    protected CustomAIClient _myClient;
    public IReadOnlyDictionary<string, object?> Attributes => this._myClient.Attributes;

    public string ServiceName { get; set; }

    public async Task<IReadOnlyList<TextContent>> GetTextContentsAsync(AudioContent content, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        return [await this._myClient.GetTextContentsAsync(content, executionSettings, kernel, cancellationToken)];
    }
}
