﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace osu.Native.Bindings.Models;

public class Mod(string acronym, Dictionary<string, object>? settings = null)
{
    [JsonProperty("acronym")]
    public string Acronym { get; } = acronym;

    [JsonProperty("settings")]
    public Dictionary<string, object> Settings { get; } = settings ?? [];
}
