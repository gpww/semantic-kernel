﻿// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using RepoUtils;

/**
 * This example shows how to use GPT3.5 Chat model for prompts and semantic functions.
 */

// ReSharper disable once InconsistentNaming
public static class Example27_SemanticFunctionsUsingChatGPT
{
    public static async Task RunAsync()
    {
        Console.WriteLine("======== Using Chat GPT model for text completion ========");

        IKernel kernel = new KernelBuilder().WithLogger(ConsoleLogger.Log).Build();

        // Note: we use Chat Completion and GPT 3.5 Turbo
        kernel.Config.AddOpenAIChatCompletionService("gpt-3.5-turbo", Env.Var("OPENAI_API_KEY"));

        var func = kernel.CreateSemanticFunction(
            "List the 3 planets closest to '{{$input}}', excluding moons, using bullet points.");

        var result = await func.InvokeAsync("Jupiter");
        Console.WriteLine(result);

        /*
        Output:
           - Saturn
           - Uranus
        */
    }
}
