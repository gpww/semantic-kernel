// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SemanticKernel.ChatCompletion;

/// <summary>
/// Provides a history of chat messages from a chat conversation.
/// </summary>
public partial class ChatHistory
{
    public virtual bool TryPopOldMessage(out ChatMessageContent msg)
    {
        throw new NotImplementedException();
    }

    public virtual ChatMessageContent? SystemMessage
    {
        get
        {
            if (this._messages?.Count == 0)
            {
                return null;
            }

            return this._messages.FirstOrDefault(m => m.Role == AuthorRole.System);
        }
    }
    public virtual ChatMessageContent Last => this._messages.LastOrDefault();//this.Count > 0 ? this._messages.Last() : null;
    public virtual ChatMessageContent First => this._messages.FirstOrDefault();
    public virtual void UpdateSystemMessage(string content)
    {
        throw new NotImplementedException();
    }
    public virtual int GetRecentMessageCount(int tokenLimit = int.MaxValue)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// 排除 SystemMessage 的短期聊天记录消息，保证 tokenLimit = 0 的时候返回 user 的最后一条消息
    /// </summary>
    public virtual IEnumerable<ChatMessageContent> GetRecentRecords(int tokenLimit = int.MaxValue)
    {
        throw new NotImplementedException();
    }
    public string GetRecentRecordsString(int tokenLimit = int.MaxValue)
    {
        var records = "";
        foreach (var r in this.GetRecentRecords(tokenLimit))
        {
            records += $"< message role = \"{r.Role}\" >{r.Content}</ message >\n";
        }
        return records;
    }
}
