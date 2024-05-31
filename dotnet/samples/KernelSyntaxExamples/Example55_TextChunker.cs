// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Kusto.Cloud.Platform.Distributed;
using Microsoft.DeepDev;
using Microsoft.KernelMemory.AI.OpenAI;
using Microsoft.ML.Tokenizers;
using Microsoft.SemanticKernel.Text;
using Resources;
using SharpToken;
using TiktokenSharp;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.SemanticKernel.Text.TextChunker;

namespace Examples;

public class Example55_TextChunker : BaseTest
{
    [Fact]
    public void RunExample()
    {
        WriteLine("=== Text chunking ===");

        var lines = TextChunker.SplitPlainTextLines(Text, 40);
        var paragraphs = TextChunker.SplitPlainTextParagraphs(lines, 120);

        WriteParagraphsToConsole(paragraphs);
    }

    [Fact]
    public void TestAppContextBaseDirectory()
    {
        Output.WriteLine(AppContext.BaseDirectory);
    }

    [Theory]
    [InlineData(TokenCounterType.SharpToken)]
    [InlineData(TokenCounterType.MicrosoftML)]
    [InlineData(TokenCounterType.DeepDev)]
    [InlineData(TokenCounterType.KernelMemoryTokenCounter)]
    [InlineData(TokenCounterType.TiktokenSharp)]
    public void ComputTokenCountByDifferentCounter(TokenCounterType counterType)
    {
        WriteLine($"=== Text Token Counting with a custom({counterType}) token counter ===");
        var sw = new Stopwatch();
        sw.Start();
        var tokenCounter = s_tokenCounterFactory(counterType);
        WriteLine($"TokenCout = {tokenCounter(Text)}");
        sw.Stop();
        WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");
    }
    [Theory]
    [InlineData(TokenCounterType.SharpToken)]
    [InlineData(TokenCounterType.MicrosoftML)]
    [InlineData(TokenCounterType.MicrosoftMLRoberta)]
    [InlineData(TokenCounterType.DeepDev)]
    [InlineData(TokenCounterType.KernelMemoryTokenCounter)]
    [InlineData(TokenCounterType.TiktokenSharp)]
    public void RunExampleForTokenCounterType(TokenCounterType counterType)
    {
        WriteLine($"=== Text chunking with a custom({counterType}) token counter ===");
        var sw = new Stopwatch();
        sw.Start();
        var tokenCounter = s_tokenCounterFactory(counterType);

        var lines = TextChunker.SplitPlainTextLines(Text, 40, tokenCounter);
        var paragraphs = TextChunker.SplitPlainTextParagraphs(lines, 120, tokenCounter: tokenCounter);

        sw.Stop();
        WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");
        WriteParagraphsToConsole(paragraphs);
    }

    [Fact]
    public void RunExampleWithHeader()
    {
        WriteLine("=== Text chunking with chunk header ===");

        var lines = TextChunker.SplitPlainTextLines(Text, 40);
        var paragraphs = TextChunker.SplitPlainTextParagraphs(lines, 150, chunkHeader: "DOCUMENT NAME: test.txt\n\n");

        WriteParagraphsToConsole(paragraphs);
    }

    private void WriteParagraphsToConsole(List<string> paragraphs)
    {
        for (var i = 0; i < paragraphs.Count; i++)
        {
            WriteLine(paragraphs[i]);

            if (i < paragraphs.Count - 1)
            {
                WriteLine("------------------------");
            }
        }
    }

    public enum TokenCounterType
    {
        SharpToken,
        MicrosoftML,
        DeepDev,
        MicrosoftMLRoberta,
        KernelMemoryTokenCounter,
        TiktokenSharp
    }

    /// <summary>
    /// Custom token counter implementation using SharpToken.
    /// Note: SharpToken is used for demonstration purposes only, it's possible to use any available or custom tokenization logic.
    /// </summary>
    private static TokenCounter SharpTokenTokenCounter => (string input) =>
    {
        // Initialize encoding by encoding name
        var encoding = GptEncoding.GetEncoding("cl100k_base");

        // Initialize encoding by model name
        // var encoding = GptEncoding.GetEncodingForModel("gpt-4");

        var tokens = encoding.Encode(input);

        return tokens.Count;
    };

    /// <summary>
    /// MicrosoftML token counter implementation.
    /// </summary>
    private static TokenCounter MicrosoftMLTokenCounter => (string input) =>
    {
        Tokenizer tokenizer = new(new Bpe());
        var tokens = tokenizer.Encode(input).Tokens;

        return tokens.Count;
    };

    private static TokenCounter KernelMemoryTokenCounter => (string input) =>
    {
        return DefaultGPTTokenizer.StaticCountTokens(input);
    };

    private static TokenCounter TiktokenSharpCounter => (string input) =>
    {
        TikToken.PBEFileDirectory = "bpe";
        TikToken tikToken = TikToken.GetEncoding("cl100k_base");
        return tikToken.Encode(input).Count;
    };

    /// <summary>
    /// MicrosoftML token counter implementation using Roberta and local vocab
    /// </summary>
    private static TokenCounter MicrosoftMLRobertaTokenCounter => (string input) =>
    {
        var encoder = EmbeddedResource.ReadStream("EnglishRoberta.encoder.json");
        var vocab = EmbeddedResource.ReadStream("EnglishRoberta.vocab.bpe");
        var dict = EmbeddedResource.ReadStream("EnglishRoberta.dict.txt");

        if (encoder is null || vocab is null || dict is null)
        {
            throw new FileNotFoundException("Missing required resources");
        }

        EnglishRoberta model = new(encoder, vocab, dict);

        model.AddMaskSymbol(); // Not sure what this does, but it's in the example
        Tokenizer tokenizer = new(model, new RobertaPreTokenizer());
        var tokens = tokenizer.Encode(input).Tokens;

        return tokens.Count;
    };

    /// <summary>
    /// DeepDev token counter implementation.
    /// </summary>
    private static TokenCounter DeepDevTokenCounter => (string input) =>
    {
        // Initialize encoding by encoding name
        var tokenizer = TokenizerBuilder.CreateByEncoderNameAsync("cl100k_base").GetAwaiter().GetResult();

        // Initialize encoding by model name
        // var tokenizer = TokenizerBuilder.CreateByModelNameAsync("gpt-4").GetAwaiter().GetResult();

        var tokens = tokenizer.Encode(input, new HashSet<string>());
        return tokens.Count;
    };

    private static readonly Func<TokenCounterType, TokenCounter> s_tokenCounterFactory = (TokenCounterType counterType) =>
        counterType switch
        {
            TokenCounterType.SharpToken => (string input) => SharpTokenTokenCounter(input),
            TokenCounterType.MicrosoftML => (string input) => MicrosoftMLTokenCounter(input),
            TokenCounterType.DeepDev => (string input) => DeepDevTokenCounter(input),
            TokenCounterType.MicrosoftMLRoberta => (string input) => MicrosoftMLRobertaTokenCounter(input),
            TokenCounterType.KernelMemoryTokenCounter => (string input) => KernelMemoryTokenCounter(input),
            TokenCounterType.TiktokenSharp => (string input) => TiktokenSharpCounter(input),
            _ => throw new ArgumentOutOfRangeException(nameof(counterType), counterType, null),
        };

    private const string Text = @"你是对的，乙应该是三角形 \( \triangle AJF \)。让我们重新审视图形和题目中的描述：

题目中给出：
- \(ABC\) 为等边三角形，面积为 400。
- \(D\)、\(E\)、\(F\) 分别是三边的中点。
- 已知 \(甲\) 和 \(乙\) 的面积和为 143。

根据图中的标记和位置：
- \(甲\) 是三角形 \( \triangle ADJ \)。
- \(乙\) 是三角形 \( \triangle AJF \)。

阴影部分的五边形面积可以通过减去 \( \triangle ADJ \) 和 \( \triangle AJF \) 的面积从整个等边三角形的面积中得到。

等边三角形 \(ABC\) 的面积为 400，已知 \(甲\) 和 \(乙\) 的面积和为 143，因此阴影部分五边形的面积为：

\[400 - 143 = 257\]

因此，阴影部分五边形的面积为 257。";

    private const string Text2 = @"The city of Venice, located in the northeastern part of Italy,
is renowned for its unique geographical features. Built on more than 100 small islands in a lagoon in the
Adriatic Sea, it has no roads, just canals including the Grand Canal thoroughfare lined with Renaissance and
Gothic palaces. The central square, Piazza San Marco, contains St. Mark's Basilica, which is tiled with Byzantine
mosaics, and the Campanile bell tower offering views of the city's red roofs.

The Amazon Rainforest, also known as Amazonia, is a moist broadleaf tropical rainforest in the Amazon biome that
covers most of the Amazon basin of South America. This basin encompasses 7 million square kilometers, of which
5.5 million square kilometers are covered by the rainforest. This region includes territory belonging to nine nations
and 3.4 million square kilometers of uncontacted tribes. The Amazon represents over half of the planet's remaining
rainforests and comprises the largest and most biodiverse tract of tropical rainforest in the world.

The Great Barrier Reef is the world's largest coral reef system composed of over 2,900 individual reefs and 900 islands
stretching for over 2,300 kilometers over an area of approximately 344,400 square kilometers. The reef is located in the
Coral Sea, off the coast of Queensland, Australia. The Great Barrier Reef can be seen from outer space and is the world's
biggest single structure made by living organisms. This reef structure is composed of and built by billions of tiny organisms,
known as coral polyps.";

    public Example55_TextChunker(ITestOutputHelper output) : base(output)
    {
    }
}
