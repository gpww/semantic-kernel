// Copyright (c) Microsoft. All rights reserved.

using System;
using Microsoft.Extensions.Configuration;

namespace RepoUtils;

#pragma warning disable CA1812 // instantiated by AddUserSecrets
internal sealed class Env
#pragma warning restore CA1812
{
    /// <summary>
    /// Simple helper used to load env vars and secrets like credentials,
    /// to avoid hard coding them in the sample code
    /// </summary>
    /// <param name="name">Secret name / Env var name</param>
    /// <returns>Value found in Secret Manager or Environment Variable</returns>
    internal static string Var(string name)
    {
        return ConfigFile.Settings[name];
    }
}
