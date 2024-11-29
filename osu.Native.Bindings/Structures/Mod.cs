// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace osu.Native.Bindings.Structures;

/// <summary>
/// Represents an osu! mod with its acronym and mod settings.
/// </summary>
/// <param name="acronym">The acronym (eg. "DT").</param>
/// <param name="settings">The mod settings (eg. "speed_change").</param>
public class Mod(string acronym, Dictionary<string, object>? settings = null)
{
    /// <summary>
    /// The acronym of the mod (eg. "DT").
    /// </summary>
    [JsonProperty("acronym")]
    public string Acronym { get; } = acronym;

    /// <summary>
    /// The mod settings (eg. "speed_change"). The setting-names must be written in snake_case and match the APIMod model format.
    /// For more information, take the osu! API v2 documentation as reference.
    /// </summary>
    [JsonProperty("settings")]
    public Dictionary<string, object> Settings { get; } = settings ?? [];
}
