// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Planning;
using Microsoft.SemanticKernel.Plugins.Core;

namespace Planners;

public class FunctionCallStepwisePlanning(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public async Task RunAsync()
    {
        string[] questions =
        [
            "What is the current hour number, plus 5?",
            "What is 387 minus 22? Email the solution to John and Mary.",
            "Write a limerick, translate it to Spanish, and send it to Jane",
        ];

        var kernel = InitializeKernel();

        var options = new FunctionCallingStepwisePlannerOptions
        {
            MaxIterations = 15,
            MaxTokens = 4000,
        };
        var planner = new Microsoft.SemanticKernel.Planning.FunctionCallingStepwisePlanner(options);

        //To achieve the goal of finding the current hour number and adding 5 to it, we can follow these steps:

        //1.Use the `TimePlugin_HourNumber` function to get the current hour number.
        //2.Use the `MathPlugin_Add` function to add 5 to the current hour number obtained from step 1.

        //Let's execute these steps:

        //Step 1: Use `TimePlugin_HourNumber` to get the current hour number.

        //Step 2: Use `MathPlugin_Add` with the current hour number as the "value" and 5 as the "amount" to add.

        //After performing these steps, we will have the current hour number plus 5.However, since I cannot execute functions, I will describe the final step to communicate the result back to the user:

        //Step 3: Use the `UserInteraction_SendFinalAnswer` function to send the final answer back to the user. The final answer will be the result obtained from step 2.

        foreach (var question in questions)
        {
            FunctionCallingStepwisePlannerResult result = await planner.ExecuteAsync(kernel, question);
            Console.WriteLine($"Q: {question}\nA: {result.FinalAnswer}");

            // You can uncomment the line below to see the planner's process for completing the request.
            // Console.WriteLine($"Chat history:\n{System.Text.Json.JsonSerializer.Serialize(result.ChatHistory)}");
        }
    }

    /// <summary>
    /// Initialize the kernel and load plugins.
    /// </summary>
    /// <returns>A kernel instance</returns>
    private static Kernel InitializeKernel()
    {
        var builder = Kernel.CreateBuilder();
        builder.AddOneAPITextGeneration("gpt-4-1106-preview", "sk-GJlwir4bB0UYkb02Fa71F161120c45569641FbE59cEd7b78", "https://api.ezchat.top/v1/");
        var kernel = builder.Build();
        //.AddAzureOpenAIChatCompletion(
        //    deploymentName: TestConfiguration.AzureOpenAI.ChatDeploymentName,
        //    endpoint: TestConfiguration.AzureOpenAI.Endpoint,
        //    apiKey: TestConfiguration.AzureOpenAI.ApiKey,
        //    modelId: TestConfiguration.AzureOpenAI.ChatModelId)

        kernel.ImportPluginFromType<Plugins.EmailPlugin>();
        kernel.ImportPluginFromType<MathPlugin>();
        kernel.ImportPluginFromType<TimePlugin>();

        return kernel;
    }
}
