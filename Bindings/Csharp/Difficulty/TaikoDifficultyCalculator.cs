// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Native.Bindings.Models;
using osu.Native.Bindings.Structures.Difficulty;
using osu.Native.Bindings.Structures.Performance;
using osu.Native.Bindings.Structures.Scores;
using System.IO;

namespace osu.Native.Bindings.Difficulty;

public class TaikoDifficultyCalculator : DifficultyCalculator<TaikoDifficultyAttributes, TaikoPerformanceAttributes, TaikoScore>
{
    public TaikoDifficultyCalculator(FileInfo file) : base(file) { }

    public TaikoDifficultyCalculator(string text) : base(text) { }

    public override TaikoDifficultyAttributes CalculateDifficulty(Mod[] mods)
    {
        TaikoDifficultyAttributes attributes = default;
        OsuNative.Execute(() => OsuNative.Difficulty_ComputeTaiko(_beatmapId, JsonConvert.SerializeObject(mods), out attributes));
        return attributes;
    }

    public override TaikoPerformanceAttributes CalculatePerformance(TaikoDifficultyAttributes diffAttributes, TaikoScore score)
    {
        TaikoPerformanceAttributes attributes = default;
        OsuNative.Execute(() => OsuNative.Performance_ComputeTaiko(_beatmapId, diffAttributes, score.ToNative(), out attributes));
        return attributes;
    }
}
