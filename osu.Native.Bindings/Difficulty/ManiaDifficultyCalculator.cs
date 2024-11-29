// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Native.Bindings.Structures;
using osu.Native.Bindings.Structures.Difficulty;
using osu.Native.Bindings.Structures.Performance;
using osu.Native.Bindings.Structures.Scores;
using System.IO;

namespace osu.Native.Bindings.Difficulty;

public class ManiaDifficultyCalculator : DifficultyCalculator<ManiaDifficultyAttributes, ManiaPerformanceAttributes, ManiaScore>
{
    public ManiaDifficultyCalculator(FileInfo file) : base(file) { }

    public ManiaDifficultyCalculator(string text) : base(text) { }

    public override ManiaDifficultyAttributes CalculateDifficulty(Mod[] mods)
    {
        ManiaDifficultyAttributes attributes = default;
        OsuNative.Execute(() => OsuNative.Difficulty_ComputeMania(_beatmapId, JsonConvert.SerializeObject(mods), out attributes));
        return attributes;
    }

    public override ManiaPerformanceAttributes CalculatePerformance(ManiaDifficultyAttributes diffAttributes, ManiaScore score)
    {
        ManiaPerformanceAttributes attributes = default;
        OsuNative.Execute(() => OsuNative.Performance_ComputeMania(_beatmapId, diffAttributes, score.ToNative(), out attributes));
        return attributes;
    }
}