﻿// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;

namespace Microsoft.SemanticKernel;

public partial class PromptExecutionSettings
{
    public void AddOrUpdateExtensionData(string key, object value)
    {
        this._extensionData ??= new Dictionary<string, object>();

        if (this._extensionData.ContainsKey(key))
        {
            this._extensionData[key] = value;
        }
        else
        {
            this._extensionData.Add(key, value);
        }
    }
}
