// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel.Text;

namespace Microsoft.SemanticKernel.Connectors.OpenAI;
public sealed partial class OpenAIPromptExecutionSettings
{
    ///// <summary> Whether to return log probabilities of the output tokens or not.
    ///// If true, returns the log probabilities of each output token returned in the `content` of `message`.
    ///// This option is currently not available on the `gpt-4-vision-preview` model.
    ///// </summary>
    //[JsonPropertyName("logprobs")]
    //public bool? EnableLogProbabilities { get; set; }

    ///// <summary> An integer between 0 and 5 specifying the number of most likely tokens to return at each token position, each with an associated log probability.
    ///// `logprobs` must be set to `true` if this parameter is used.
    ///// </summary>
    //[JsonPropertyName("top_logprobs")]
    //public int? LogProbabilitiesPerToken { get; set; }

    public static OpenAIPromptExecutionSettings Deserialize(string json)
    {
        return System.Text.Json.JsonSerializer.Deserialize<OpenAIPromptExecutionSettings>(json, JsonOptionsCache.ReadPermissive);
    }
}
