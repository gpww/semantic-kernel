// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel.Connectors.OpenAI;
using Silver;

namespace Concepts;
internal class MyUtilities
{
    public static HttpClient BuildHttpClient(string newBaseUrl, string proxy, int timeoutSeconds)
    {
        var changeOpenAIBaseUrlHandler = new ChangeOpenAIBaseUrlHandler(newBaseUrl);
        return WebHelper.BuildHttpClient(changeOpenAIBaseUrlHandler, proxy, timeoutSeconds);
    }
}
