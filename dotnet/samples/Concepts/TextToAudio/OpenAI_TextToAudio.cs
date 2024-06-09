// Copyright (c) Microsoft. All rights reserved.

using System;
using System.IO;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.TextToAudio;
using Silver;

namespace TextToAudio;

/// <summary>
/// Represents a class that demonstrates audio processing functionality.
/// </summary>
public sealed class OpenAI_TextToAudio(ITestOutputHelper output) : BaseTest(output)
{
    private const string TextToAudioModel = "tts-1-hd";

    //[Fact(Skip = "Uncomment the line to write the audio file output before running this test.")]
    [Fact]
    public async Task TextToAudioAsync()
    {
        var httpClient = Concepts.MyUtilities.BuildHttpClient("https://localhost:5003", null, 60);

        // Create a kernel with OpenAI text to audio service
        var kernel = Kernel.CreateBuilder()
            .AddOpenAITextToAudio(
                modelId: TextToAudioModel,
                apiKey: "sk-w07M88L/hI0QzrjRPONymznrnot5mRFymX5Hos1uSSw=", httpClient: httpClient)
            .Build();

        var textToAudioService = kernel.GetRequiredService<ITextToAudioService>();

        string sampleText = "你好，我叫Jhon。我是一名软件工程师。我正在进行一个将文本转换为音频的项目。";

        // Set execution settings (optional)
        OpenAITextToAudioExecutionSettings executionSettings = new()
        {
            Voice = "onyx", // The voice to use when generating the audio.
                             // Supported voices are alloy, echo, fable, onyx, nova, and shimmer.
            ResponseFormat = "mp3", // The format to audio in.
                                    // Supported formats are mp3, opus, aac, and flac.
            Speed = 1.0f // The speed of the generated audio.
                         // Select a value from 0.25 to 4.0. 1.0 is the default.
        };

        // Convert text to audio
        AudioContent audioContent = await textToAudioService.GetAudioContentAsync(sampleText, executionSettings);
        // Save audio content to a file
        if (audioContent.Data.HasValue)
        {
            await File.WriteAllBytesAsync("test.mp3", audioContent.Data.Value.ToArray());
        }
    }
}
