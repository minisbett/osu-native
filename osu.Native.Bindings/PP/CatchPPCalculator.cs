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
/// A difficulty and performance calculator for osu!catch.
/// </summary>
public class CatchPPCalculator : PPCalculator<CatchDifficultyAttributes, CatchPerformanceAttributes, CatchScore>
{
    /// <inheritdoc/>
    public CatchPPCalculator(FileInfo file) : base(file) { }

    /// <inheritdoc/>
    public CatchPPCalculator(string text) : base(text) { }

    /// <inheritdoc/>
    public override CatchDifficultyAttributes CalculateDifficulty(Mod[] mods)
    {
        CatchDifficultyAttributes attributes = default;
        OsuNative.Execute(() => OsuNative.Difficulty_CalculateCatch(_beatmapId, JsonConvert.SerializeObject(mods), out attributes));
        return attributes;
    }

    /// <inheritdoc/>
    public override CatchPerformanceAttributes CalculatePerformance(CatchDifficultyAttributes diffAttributes, CatchScore score)
    {
        CatchPerformanceAttributes attributes = default;
        OsuNative.Execute(() => OsuNative.Performance_CalculateCatch(_beatmapId, diffAttributes, score.ToNative(), out attributes));
        return attributes;
    }
}
