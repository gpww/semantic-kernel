// Copyright (c) Microsoft. All rights reserved.

using System.Net;
using Azure.AI.OpenAI;
using Microsoft.SemanticKernel.Connectors.OpenAI.CustomClient;

namespace 临时测试;

internal class Program
{
    private static void Main(string[] args)
    {
        var handler = new CustomHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(10) // 设置10秒的超时
        };
    }
}

public class CustomHttpMessageHandler : HttpClientHandler
{
    private int _requestCount = 0;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _requestCount++;

        if (_requestCount > 1)
        {
            // 如果这是第二次（或更多次）请求，切换代理服务器
            this.Proxy = new WebProxy("http://localhost:7890");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
