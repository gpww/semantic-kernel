// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.TextCompletion;
using RepoUtils;

/**
 * The following example shows how to use Semantic Kernel with Text Completion as streaming
 */
// ReSharper disable once InconsistentNaming
public static class Example32_StreamingCompletion
{
    public static async Task RunAsync()
    {
        //await AzureOpenAITextCompletionStreamAsync();
        await OpenAITextCompletionStreamAsync();
    }

    private static async Task AzureOpenAITextCompletionStreamAsync()
    {
        Console.WriteLine("======== Azure OpenAI - Text Completion - Raw Streaming ========");

        IKernel kernel = new KernelBuilder().WithLogger(ConsoleLogger.Log).Build();
        kernel.Config.AddAzureTextCompletionService(
            Env.Var("AZURE_OPENAI_DEPLOYMENT_NAME"),
            Env.Var("AZURE_OPENAI_ENDPOINT"),
            Env.Var("AZURE_OPENAI_KEY"));

        ITextCompletion textCompletion = kernel.GetService<ITextCompletion>();

        await TextCompletionStreamAsync(textCompletion);
    }

    private static async Task OpenAITextCompletionStreamAsync()
    {
        Console.WriteLine("======== Open AI - Text Completion - Raw Streaming ========");

        IKernel kernel = new KernelBuilder().WithLogger(ConsoleLogger.Log).Build();
        //kernel.Config.AddOpenAITextCompletionService("text-davinci-003", Env.Var("OPENAI_API_KEY"), serviceId: "text-davinci-003");
        kernel.Config.AddOpenAIChatCompletionService("gpt-3.5-turbo", Env.Var("OPENAI_API_KEY"));

        ITextCompletion textCompletion = kernel.GetService<ITextCompletion>();

        await TextCompletionStreamAsync(textCompletion);
    }

    private static async Task TextCompletionStreamAsync(ITextCompletion textCompletion)
    {
        var requestSettings = new CompleteRequestSettings()
        {
            MaxTokens = 512,
            Temperature = 0.7,
            TopP = 0.5
        };

        var prompt = "Write one paragraph why AI is awesome, in Chinese";

        Console.WriteLine("Prompt: " + prompt);
        await foreach (string message in textCompletion.CompleteStreamAsync(prompt, requestSettings))
        {
            Console.Write(message);
        }

        Console.WriteLine();
    }
}
//======== Open AI - Text Completion - Raw Streaming ========
//Prompt: Write one paragraph why AI is awesome
//AI, or artificial intelligence, is awesome because it has the potential to revolutionize the way we live and work. With AI, we can automate tedious and repetitive tasks, freeing up time for more creative and meaningful work. AI can also help us make better decisions by analyzing vast amounts of data and providing insights that humans may not have been able to uncover on their own. Additionally, AI has the potential to improve healthcare, transportation, and other industries by making processes more efficient and effective. Overall, AI has the power to make our lives easier, more productive, and more enjoyable.

//== DONE ==
