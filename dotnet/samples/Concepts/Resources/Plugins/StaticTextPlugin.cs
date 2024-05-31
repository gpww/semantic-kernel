// Copyright (c) Microsoft. All rights reserved.

using System;
using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace Plugins;

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

    [KernelFunction, Description("My test function")]
    public string Test([Description("测试自定义类型")] CustomType input2, KernelArguments context)
    {
        return $"测试传入CustomType input2，input2.Name = {input2.Name}";
    }
    [KernelFunction, Description("记录当前运行时间1")]
    public string RecordTime1()
    {
        //Console.WriteLine(DateTime.Now);
        ////休眠5秒
        //System.Threading.Thread.Sleep(5000);
        //Console.WriteLine(DateTime.Now);
        return DateTime.Now.ToString();
    }

    [KernelFunction, Description("记录当前运行时间2")]
    public string RecordTime2()
    {
        //Console.WriteLine(DateTime.Now);
        ////休眠5秒
        System.Threading.Thread.Sleep(1000);
        //Console.WriteLine(DateTime.Now);
        return DateTime.Now.ToString();
    }
}

public class CustomType
{
    public string Name { get; set; }
}
