// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.SemanticKernel.Connectors.OpenAI;
public class ChangeOpenAIBaseUrlHandler : HttpClientHandler
{
    private static string CheckEndPoint(string endPoint)
    {
        if (endPoint.EndsWith("/"))
        {
            endPoint = endPoint.TrimEnd('/');
        }
        if (!Regex.IsMatch(endPoint, @"/v\d+$"))//如果末尾没有 /v数字 这种版本号，就加上/v1
        {
            endPoint += "/v1";
        }
        return endPoint;
    }

    private readonly string _newBaseUrl;
    private const string PublicOpenAIEndpoint = "https://api.openai.com/v1";
    public ChangeOpenAIBaseUrlHandler(string newBaseUrl)
    {
        this._newBaseUrl = CheckEndPoint(newBaseUrl);
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // 修改请求的URL
        var requestUri = request.RequestUri.ToString();
        if (requestUri.StartsWith(PublicOpenAIEndpoint))
        {
            var newUri = new Uri(requestUri.Replace(PublicOpenAIEndpoint, this._newBaseUrl));
            request.RequestUri = newUri;
        }
        // 调用基类的SendAsync方法发送修改后的请求
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
