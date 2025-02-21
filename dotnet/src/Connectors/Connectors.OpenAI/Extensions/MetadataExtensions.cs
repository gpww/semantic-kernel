// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Azure.AI.OpenAI;

namespace Microsoft.SemanticKernel.Connectors.OpenAI;
public static class MetadataExtensions
{
    public static IReadOnlyList<ChatTokenLogProbabilityInfo> GetChatTokenLogProbabilities(this IReadOnlyDictionary<string, object?> metadata)
    {
        if (metadata != null && metadata.TryGetValue("LogProbabilityInfo", out var logProbabilityInfo))
        {
            var ccLP = logProbabilityInfo as ChatChoiceLogProbabilityInfo;
            if (ccLP?.TokenLogProbabilityResults?.Count > 0)
            {
                return ccLP.TokenLogProbabilityResults.FirstOrDefault()?.TopLogProbabilityEntries;
            }
        }
        return Array.Empty<ChatTokenLogProbabilityInfo>();
    }
}
