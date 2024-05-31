// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Concurrent;
using Microsoft.KernelMemory.AI.OpenAI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Connectors.AI;

/// <summary>
/// 保证线程安全的聊天记录
/// </summary>
public class CustomChatHistory : ChatHistory
{
    public override string ToString()
    {
        return this.ToJsonString();
        //var chatHistory = this.Select(m => new
        //{
        //    role = m.Role.Label,
        //    content = m.Content
        //});

        //return JsonConvert.SerializeObject(chatHistory);
    }

    private readonly ConcurrentQueue<ChatMessageContent> _messages = new();//解决并发问题
    public CustomChatHistory() : base()
    {
    }
    private ChatMessageContent _systemMessage = null;//因为Dequeue会把元素移除，所以需要一个变量保存系统身份设定消息
    public CustomChatHistory(string? systemMessage)
    {
        if (!string.IsNullOrEmpty(systemMessage))
        {
            this._systemMessage = new ChatMessageContent(AuthorRole.System, systemMessage);
        }
    }

    public override void Add(ChatMessageContent item)
    {
        //base.Add(item);
        this._messages.Enqueue(item);

        if (item.TokenCount == 0)//如果不是来自OneAPI, 则消息没有计算过 token 数量
        {
            item.TokenCount = item.ComputeTokenCount();
        }
    }

    public override void UpdateSystemMessage(string? content)
    {
        if (!string.IsNullOrEmpty(content) && this._systemMessage?.Content != content)
        {
            this._systemMessage = new ChatMessageContent(AuthorRole.System, content);
            this._systemMessage.TokenCount = TikToken.GetCount(content);
        }
    }
    public override ChatMessageContent? SystemMessage => this._systemMessage;
    public override IEnumerator<ChatMessageContent> GetEnumerator()
    {
        if (this._systemMessage != null)
            yield return this._systemMessage;
        foreach (var msg in this._messages)
        {
            yield return msg;
        }
    }

    /// <summary>
    /// 从最新的消息开始
    /// </summary>
    private IEnumerable<ChatMessageContent> FromNew2Old
    {
        get
        {
            foreach (var msg in this._messages.Reverse())
            {
                yield return msg;
            }
            if (this._systemMessage != null)
                yield return this._systemMessage;
        }
    }

    public override int Count => this._messages.Count;
    public override bool TryPopOldMessage(out ChatMessageContent msg)
    {
        if (this._messages.TryDequeue(out var m))
        {
            msg = m;
            return true;
        }
        msg = null;
        return false;
    }

    public override int GetRecentMessageCount(int tokenLimit = int.MaxValue)
    {
        var n = 0;
        int tokenCount = 0;
        foreach (var msg in this.FromNew2Old)//从最新的消息开始
        {
            tokenCount += msg.TokenCount;
            if (tokenCount >= tokenLimit)
            {
                break;
            }
            n++;//说明这条记录没有超过限制，可以加入最近聊天记录
        }
        return n;
    }

    /// <summary>
    /// 排除 SystemMessage 的短期聊天记录消息，tokenLimit = 0 的时候返回 user 的最后一条消息
    /// </summary>
    public override IEnumerable<ChatMessageContent> GetRecentRecords(int tokenLimit = int.MaxValue)
    {
        int tokenCount = 0;
        foreach (var msg in this.FromNew2Old)//从最新的消息开始
        {
            yield return msg;
            tokenCount += msg.TokenCount;
            if (tokenCount >= tokenLimit)
            {
                break;
            }
        }
    }

    public override void Clear()
    {
        base.Clear();
        this._messages.Clear();
    }
}

//public class GlmChatResult : IChatResult, ITextResult, IChatStreamingResult, ITextStreamingResult
//{
//    private readonly AuthorRole _role;
//    private ChatMessageBase _message;
//    public ModelResult ModelResult { get; private set; }

//    public GlmChatResult(AuthorRole role, string content)
//    {
//        this._role = role;
//        this._message = new ChatMessage(role, content);
//        this.ModelResult = new ModelResult(content);
//    }
//    public GlmChatResult(AuthorRole role, IEnumerable<string> stream)
//    {
//        this._role = role;
//        this._stream = stream;
//    }
//    private readonly IEnumerable<string> _stream;

//    public Task<ChatMessageBase> GetChatMessageAsync(CancellationToken cancellationToken = default)
//    {
//        return Task.FromResult(this._message);
//    }

//    public Task<string> GetCompletionAsync(CancellationToken cancellationToken = default)
//    {
//        return Task.FromResult(ModelResult.GetResult<string>());
//    }

//    public async IAsyncEnumerable<ChatMessageBase> GetStreamingChatMessageAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
//    {
//        var bufferedOutput = new StringBuilder();
//        foreach (var chunk in this._stream)
//        {
//            bufferedOutput.Append(chunk);
//            yield return new ChatMessage(this._role, chunk);
//        }
//        this._message = new ChatMessage(this._role, bufferedOutput.ToString());
//        this.ModelResult = new ModelResult(bufferedOutput.ToString());
//    }

//    public async IAsyncEnumerable<string> GetCompletionStreamingAsync(CancellationToken cancellationToken = default)
//    {
//        var bufferedOutput = new StringBuilder();
//        foreach (var chunk in this._stream)
//        {
//            bufferedOutput.Append(chunk);
//            yield return chunk;
//        }
//        this._message = new ChatMessage(this._role, bufferedOutput.ToString());
//        this.ModelResult = new ModelResult(bufferedOutput.ToString());
//    }
//}
