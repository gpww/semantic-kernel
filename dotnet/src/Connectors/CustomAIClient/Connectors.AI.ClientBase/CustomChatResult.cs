// Copyright (c) Microsoft. All rights reserved.

using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Orchestration;
using static Microsoft.SemanticKernel.AI.ChatCompletion.ChatHistory;

namespace Connectors.AI.CustomClientBase;

public class CustomChatResult : IChatResult, ITextResult, IChatStreamingResult, ITextStreamingResult
{
    private readonly AuthorRole _role;
    private ChatMessageBase _message;
    public ModelResult ModelResult { get; private set; }

    public CustomChatResult(AuthorRole role, string? content)
    {
        this._role = role;
        this._message = new ChatMessage(role, content);
        this.ModelResult = new ModelResult(content);
    }
    public CustomChatResult(AuthorRole role, IAsyncEnumerable<string> stream)
    {
        this._role = role;
        this._stream = stream;
    }
    private readonly IAsyncEnumerable<string> _stream;

    public Task<ChatMessageBase> GetChatMessageAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(this._message);
    }

    public Task<string> GetCompletionAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(ModelResult.GetResult<string>());
    }

    public async IAsyncEnumerable<ChatMessageBase> GetStreamingChatMessageAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var bufferedOutput = new StringBuilder();
        await foreach (var chunk in this._stream)
        {
            bufferedOutput.Append(chunk);
            yield return new ChatMessage(this._role, chunk);
        }
        this._message = new ChatMessage(this._role, bufferedOutput.ToString());
        this.ModelResult = new ModelResult(bufferedOutput.ToString());
    }

    public async IAsyncEnumerable<string> GetCompletionStreamingAsync(CancellationToken cancellationToken = default)
    {
        var bufferedOutput = new StringBuilder();
        await foreach (var chunk in this._stream)
        {
            bufferedOutput.Append(chunk);
            yield return chunk;
        }
        this._message = new ChatMessage(this._role, bufferedOutput.ToString());
        this.ModelResult = new ModelResult(bufferedOutput.ToString());
    }
}
