// Copyright (c) Microsoft. All rights reserved.

using System.ComponentModel;
using System.Globalization;
using Microsoft.SemanticKernel;

namespace Functions;

// This example shows how to use kernel arguments when invoking functions.
public class Arguments(ITestOutputHelper output) : BaseTest(output)
{
    public Example03_Arguments(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task RunAsync()
    {
        Console.WriteLine("======== Arguments ========");

        var httpClient = WebHelper.BuildHttpClient();
        // Create kernel.
        IKernelBuilder builder = Kernel.CreateBuilder();
        builder.AddOpenAIChatCompletion("gpt-3.5-turbo", TestConfiguration.OpenAI.ApiKey, serviceId: serviceKey1, httpClient: httpClient);
        builder.Services.AddSingleton<IAIServiceSelector>(new AIServiceSelector()); // Use the custom AI service selector to select the GPT model
        builder.AddOpenAIChatCompletion("gpt-4", TestConfiguration.OpenAI.ApiKey, serviceId: serviceKey2, httpClient: httpClient);
        //builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));
        //builder.Services.AddLogging(services => services.AddSerilog().SetMinimumLevel(LogLevel.Trace));
        Kernel kernel = builder.Build();

        //Kernel kernel = new();
        var textPlugin = kernel.ImportPluginFromType<StaticTextPlugin>("StaticTextPlugin");

        kernel.ImportPluginFromType<TimePlugin>(TimePlugin.PluginName);

        var customType = new CustomType() { Name = "CustomType" };

        var arguments = new KernelArguments()
        {
            ["input"] = "Today is: ",
            ["day"] = DateTimeOffset.Now.ToString("dddd", CultureInfo.CurrentCulture),
            ["input2"] = customType
        };

        // ** Different ways of executing functions with arguments **

        // Specify and get the value type as generic parameter
        string? resultValue = await kernel.InvokeAsync<string>(textPlugin["AppendDay"], arguments);
        Console.WriteLine($"string -> {resultValue}");

        // Define hooks
        void MyRenderingHandler(object? sender, PromptRenderingEventArgs e)
        {
            Console.WriteLine($"{e.Function.Name} : Prompt Rendering Handler - Triggered");
            //e.Arguments["style"] = "Seinfeld";
        }

        void MyRenderedHandler(object? sender, PromptRenderedEventArgs e)
        {
            Console.WriteLine("RenderedPrompt:");
            Console.WriteLine(StringHelper.LineSeparator);
            Console.WriteLine(e.RenderedPrompt);
            Console.WriteLine(StringHelper.LineSeparator);
        }
        kernel.PromptRendered += MyRenderedHandler;
        kernel.PromptRendering += MyRenderingHandler;

        // Specify the type from the FunctionResult
        Console.WriteLine($"FunctionResult.GetValue<string>() -> {functionResult.GetValue<string>()}");

        // FunctionResult.ToString() automatically converts the result to string
        Console.WriteLine($"FunctionResult.ToString() -> {functionResult}");
    }

    public sealed class StaticTextPlugin
    {
        [KernelFunction, Description("Change all string chars to uppercase")]
        public static string Uppercase([Description("Text to uppercase")] string input) =>
            input.ToUpperInvariant();

        [KernelFunction, Description("Append the day variable")]
        public static string AppendDay(
            [Description("Text to append to")] string input,
            [Description("Value of the day to append")] string day) =>
            input + day;
    }
}
