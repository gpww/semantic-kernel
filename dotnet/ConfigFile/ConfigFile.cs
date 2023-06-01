// Copyright (c) Microsoft. All rights reserved.

using Microsoft.Extensions.Configuration;

public static class ConfigFile
{
    public static IConfiguration Settings
    {
        get
        {
            return s_config.GetSection("Settings");
        }
    }
    private static readonly IConfiguration s_config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
}
