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
/// A difficulty and performance calculator for osu!mania.
/// </summary>
public class ManiaPPCalculator : PPCalculator<ManiaDifficultyAttributes, ManiaPerformanceAttributes, ManiaScore>
{
    /// <inheritdoc/>
    public ManiaPPCalculator(FileInfo file) : base(file) { }

    /// <inheritdoc/>
    public ManiaPPCalculator(string text) : base(text) { }

    /// <inheritdoc/>
    public override ManiaDifficultyAttributes CalculateDifficulty(Mod[] mods)
    {
        ManiaDifficultyAttributes attributes = default;
        OsuNative.Execute(() => OsuNative.Difficulty_CalculateMania(_beatmapId, JsonConvert.SerializeObject(mods), out attributes));
        return attributes;
    }

    /// <inheritdoc/>
    public override ManiaPerformanceAttributes CalculatePerformance(ManiaScore score)
    {
        ManiaPerformanceAttributes attributes = default;
        OsuNative.Execute(() => OsuNative.Performance_CalculateMania(_beatmapId, CalculateDifficulty(score.Mods), score.ToNative(), out attributes));
        return attributes;
    }

    /// <inheritdoc/>
    public override ManiaPerformanceAttributes CalculatePerformance(ManiaDifficultyAttributes diffAttributes, ManiaScore score)
    {
        ManiaPerformanceAttributes attributes = default;
        OsuNative.Execute(() => OsuNative.Performance_CalculateMania(_beatmapId, diffAttributes, score.ToNative(), out attributes));
        return attributes;
    }
}