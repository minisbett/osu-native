// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Native.Bindings.Structures;
using osu.Native.Bindings.Structures.Difficulty;
using osu.Native.Bindings.Structures.Performance;
using osu.Native.Bindings.Structures.Scores;
using System.IO;

namespace osu.Native.Bindings.PP;

/// <summary>
/// A difficulty and performance calculator for osu!standard.
/// </summary>
public class OsuPPCalculator : PPCalculator<OsuDifficultyAttributes, OsuPerformanceAttributes, OsuScore>
{
    /// <inheritdoc/>
    public OsuPPCalculator(FileInfo file) : base(file) { }

    /// <inheritdoc/>
    public OsuPPCalculator(string text) : base(text) { }

    /// <inheritdoc/>
    public override OsuDifficultyAttributes CalculateDifficulty(Mod[] mods)
    {
        OsuDifficultyAttributes attributes = default;
        OsuNative.Execute(() => OsuNative.Difficulty_CalculateOsu(_beatmapId, JsonConvert.SerializeObject(mods), out attributes));
        return attributes;
    }

    /// <inheritdoc/>
    public override OsuPerformanceAttributes CalculatePerformance(OsuDifficultyAttributes diffAttributes, OsuScore score)
    {
        OsuPerformanceAttributes attributes = default;
        OsuNative.Execute(() => OsuNative.Performance_CalculateOsu(_beatmapId, diffAttributes, score.ToNative(), out attributes));
        return attributes;
    }
}