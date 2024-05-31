// Copyright (c) Microsoft. All rights reserved.

using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using RepoUtils;
using Xunit;
using Xunit.Abstractions;
using Silver;
using System;

namespace Examples;

public class Example43_GetModelResult : BaseTest
{
    [Fact]
    public async Task GetTokenUsageMetadataAsync()
    {
        Console.WriteLine("======== Inline Function Definition + Invocation ========");
        var httpClient = WebHelper.BuildHttpClient();
        // Create kernel
        Kernel kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(
                modelId: TestConfiguration.OpenAI.ChatModelId,
                apiKey: TestConfiguration.OpenAI.ApiKey,
                httpClient: httpClient)
            .Build();

        // Create function
        const string FunctionDefinition = "Hi, give me 5 book suggestions about: {{$input}}";
        KernelFunction myFunction = kernel.CreateFunctionFromPrompt(FunctionDefinition);

        // Invoke function through kernel
        FunctionResult result = await kernel.InvokeAsync(myFunction, new() { ["input"] = "travel" });

        // Display results
        WriteLine(result.GetValue<string>());
        WriteLine(result.Metadata?["Usage"]?.AsJson());
        WriteLine();
    }

    public Example43_GetModelResult(ITestOutputHelper output) : base(output)
    {
    }
}
//Running Example43_GetModelResult...
//======== Inline Function Definition + Invocation ========
//1. "On the Road" by Jack Kerouac - Follow the adventures of Sal Paradise as he embarks on a cross-country road trip, exploring America's landscapes and encountering an array of colorful characters.

//2. "In Patagonia" by Bruce Chatwin - Join Chatwin as he ventures through the stunning and remote landscapes of Patagonia, blending travelogue with personal reflection to capture the essence of this enigmatic region.

//3. "A Walk in the Woods" by Bill Bryson - Bryson takes you along on his humorous and insightful journey along the Appalachian Trail, providing an entertaining account of his encounters with nature, fellow hikers, and the challenges of long-distance hiking.

//4. "Eat, Pray, Love" by Elizabeth Gilbert - Follow Gilbert's soul-searching adventure across three countries as she seeks to rediscover herself through food in Italy, spirituality in India, and love in Bali.

//5. "The Alchemist" by Paulo Coelho - Although not strictly a travel book, this allegorical novel tells the story of a young shepherd who embarks on a journey in search of his personal legend, ultimately discovering the importance of following one's dreams and the hidden treasures that lie along the way.
//{
//  "CompletionTokens": 247,
//  "PromptTokens": 18,
//  "TotalTokens": 265
//}
