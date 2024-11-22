// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Native.Bindings.Models;
using osu.Native.Bindings.Models.Mania;
using System.IO;

namespace osu.Native.Bindings.Difficulty;

public class ManiaDifficultyCalculator : DifficultyCalculator<ManiaDifficultyAttributes, ManiaPerformanceAttributes, ManiaScore>
{
    public ManiaDifficultyCalculator(FileInfo file) : base(file) { }

    public ManiaDifficultyCalculator(string text) : base(text) { }

    public override ManiaDifficultyAttributes CalculateDifficulty(Mod[] mods)
    {
        OsuNative.Difficulty_ComputeMania(_beatmapContextId, JsonConvert.SerializeObject(mods), out ManiaDifficultyAttributes attributes);
        return attributes;
    }

    public override ManiaPerformanceAttributes CalculatePerformance(ManiaDifficultyAttributes diffAttributes, ManiaScore score)
    {
        OsuNative.Performance_ComputeMania(_beatmapContextId, diffAttributes, score.ToNative(), out ManiaPerformanceAttributes attributes);
        return attributes;
    }
}