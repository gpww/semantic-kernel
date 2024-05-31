// Copyright (c) Microsoft. All rights reserved.

using Examples;

namespace 临时测试;

internal class Program
{
    private static void Main(string[] args)
    {
        Example86_ChatHistorySerialization test = new(null);
        test.SerializeChatHistoryWithSKContentTypes();
    }
}
