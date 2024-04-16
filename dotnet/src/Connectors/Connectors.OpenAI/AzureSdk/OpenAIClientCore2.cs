using System.Net.Http;
using Azure.Core;
using Microsoft.Extensions.Logging;

namespace Microsoft.SemanticKernel.Connectors.OpenAI;
/// <summary>
/// 我的扩展代码，主要是为了给OpenAIClient构造函数增加endPoint，以便使用第三方的OpenAI服务
/// </summary>
internal partial class OpenAIClientCore : ClientCore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenAIClientCore"/> class.
    /// </summary>
    /// <param name="modelId">Model name.</param>
    /// <param name="apiKey">OpenAI API Key.</param>
    /// <param name="endPoint">Public OpenAI Endpoint, default = https://api.openai.com/v1</param>
    /// <param name="organization">OpenAI Organization Id (usually optional).</param>
    /// <param name="httpClient">Custom <see cref="HttpClient"/> for HTTP requests.</param>
    /// <param name="logger">The <see cref="ILoggerFactory"/> to use for logging. If null, no logging will be performed.</param>
    internal OpenAIClientCore(
        string modelId,
        string apiKey,
        string? endPoint = null,
        string? organization = null,
        HttpClient? httpClient = null,
        ILogger? logger = null) : base(logger)
    {
        Verify.NotNullOrWhiteSpace(modelId);
        Verify.NotNullOrWhiteSpace(apiKey);

        this.DeploymentOrModelName = modelId;

        var options = GetOpenAIClientOptions(httpClient);

        if (!string.IsNullOrWhiteSpace(organization))
        {
            options.AddPolicy(new AddHeaderRequestPolicy("OpenAI-Organization", organization!), HttpPipelinePosition.PerCall);
        }

        //this.Client = new OpenAIClient(apiKey, options);
        this.Client = new CustomClient.OpenAIClient2(apiKey, options, endPoint);
    }
}
