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
/// A difficulty and performance calculator for osu!taiko.
/// </summary>
public class TaikoPPCalculator : PPCalculator<TaikoDifficultyAttributes, TaikoPerformanceAttributes, TaikoScore>
{
    /// <inheritdoc/>
    public TaikoPPCalculator(FileInfo file) : base(file) { }

    /// <inheritdoc/>
    public TaikoPPCalculator(string text) : base(text) { }

    /// <inheritdoc/>
    public override TaikoDifficultyAttributes CalculateDifficulty(Mod[] mods)
    {
        TaikoDifficultyAttributes attributes = default;
        OsuNative.Execute(() => OsuNative.Difficulty_CalculateTaiko(_beatmapId, JsonConvert.SerializeObject(mods), out attributes));
        return attributes;
    }

    /// <inheritdoc/>
    public override TaikoPerformanceAttributes CalculatePerformance(TaikoDifficultyAttributes diffAttributes, TaikoScore score)
    {
        TaikoPerformanceAttributes attributes = default;
        OsuNative.Execute(() => OsuNative.Performance_CalculateTaiko(_beatmapId, diffAttributes, score.ToNative(), out attributes));
        return attributes;
    }
}
