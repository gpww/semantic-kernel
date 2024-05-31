// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Azure.AI.OpenAI;

namespace Microsoft.SemanticKernel.Connectors.OpenAI;
public static class MetadataExtensions
{
    public static IReadOnlyList<ChatTokenLogProbabilityInfo> GetChatTokenLogProbabilities(this IReadOnlyDictionary<string, object?> metadata)
    {
        var logProbabilityInfo = metadata["LogProbabilityInfo"] as ChatChoiceLogProbabilityInfo;
        return logProbabilityInfo.TokenLogProbabilityResults.First().TopLogProbabilityEntries;
    }
}
