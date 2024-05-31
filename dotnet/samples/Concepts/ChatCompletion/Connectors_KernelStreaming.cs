// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace ChatCompletion;

/// <summary>
/// This example shows how you can use Streaming with Kernel.
/// </summary>
/// <param name="output"></param>
public class Connectors_KernelStreaming(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public async Task RunAsync()
    {
        string apiKey = TestConfiguration.AzureOpenAI.ApiKey;
        string chatDeploymentName = TestConfiguration.AzureOpenAI.ChatDeploymentName;
        string chatModelId = TestConfiguration.AzureOpenAI.ChatModelId;
        string endpoint = TestConfiguration.AzureOpenAI.Endpoint;

        if (apiKey is null || chatDeploymentName is null || chatModelId is null || endpoint is null)
        {
            Console.WriteLine("Azure endpoint, apiKey, deploymentName or modelId not found. Skipping example.");
            return;
        }

        var kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(
                deploymentName: chatDeploymentName,
                endpoint: endpoint,
                serviceId: "AzureOpenAIChat",
                apiKey: apiKey,
                modelId: chatModelId)
            .Build();

        var funnyParagraphFunction = kernel.CreateFunctionFromPrompt("Write a funny paragraph about streaming", new OpenAIPromptExecutionSettings() { MaxTokens = 100, Temperature = 0.4, TopP = 1 });

        var roleDisplayed = false;

        Console.WriteLine("\n===  Prompt Function - Streaming ===\n");

        string fullContent = string.Empty;
        // Streaming can be of any type depending on the underlying service the function is using.
        await foreach (var update in kernel.InvokeStreamingAsync<OpenAIStreamingChatMessageContent>(funnyParagraphFunction))
        {
            // You will be always able to know the type of the update by checking the Type property.
            if (!roleDisplayed && update.Role.HasValue)
            {
                Console.WriteLine($"Role: {update.Role}");
                fullContent += $"Role: {update.Role}\n";
                roleDisplayed = true;
            }

            if (update.Content is { Length: > 0 })
            {
                fullContent += update.Content;
                Console.Write(update.Content);
            }
            //这是C# 9.0引入的模式匹配（Pattern Matching）的一种新语法，叫做属性模式（Property Pattern）和关系模式（Relational Pattern）。
            //`if (update.Content is { Length: > 0 })`这行代码做了两件事：
            //1.属性模式 `{ Length: > 0 }`：这个模式检查`update.Content`对象是否有一个`Length`属性，并且这个属性的值大于0。
            //2. `is`关键字：这个关键字用于检查对象是否匹配特定的模式。在这个例子中，它检查`update.Content`是否匹配属性模式`{ Length: > 0 }`。
            //所以，这个`if`语句的意思是：如果`update.Content`的`Length`属性大于0，那么执行`Console.Write(update.Content);`这行代码。
            //这种语法可以让你在一行代码中同时进行类型检查和属性值的检查，使得代码更加简洁。
        }

        Console.WriteLine("\n------  Streamed Content ------\n");
        Console.WriteLine(fullContent);
    }
}
