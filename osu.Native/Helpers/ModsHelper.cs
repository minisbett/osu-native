// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Rulesets;
using osu.Game.Online.API;
using osu.Game.Rulesets.Mods;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace osu.Native.Helpers;

public static class ModsHelper
{
    /// <summary>
    /// Parses the mods and their settings in the specified <see cref="APIMod"/>[] JSON string and returns their ruleset instances.
    /// If the specified JSON string is empty, an empty array of mods is returned.
    /// </summary>
    /// <param name="ruleset">The ruleset.</param>
    /// <param name="json">The <see cref="APIMod"/>[] JSON string.</param>
    /// <returns>A ruleset instances of the specified mods.</returns>
    public static Mod[] ParseMods(Ruleset ruleset, string json)
    {
        if (string.IsNullOrEmpty(json))
            return [];

        try
        {
            APIMod[] mods = JsonSerializer.Deserialize(json, APIModSourceGenerationContext.Default.APIModArray)!;
            return [.. mods.Select(m => m.ToMod(ruleset))];
        }
        catch (Exception ex)
        {
            throw new ModsParsingFailedException(ex.Message);
        }
    }
}

/// <summary>
/// The source generation context for the JSON parsing of <see cref="Game.Online.API.APIMod"/>.
/// This is necessary as reflection-based serialization is not available with NativeAOT.
/// </summary>
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)] // "acronym"/"settings" vs "Acronym"/"Settings"
[JsonSerializable(typeof(APIMod[]))]
[JsonSerializable(typeof(string))] // Acronym
[JsonSerializable(typeof(Dictionary<string, object>))] // Settings
public partial class APIModSourceGenerationContext : JsonSerializerContext;

/// <summary>
/// Indicates that parsing an <see cref="APIMod"/>[] JSON failed.
/// </summary>
public class ModsParsingFailedException(string message) : Exception(message);