// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Native.Bindings.Models;
using osu.Native.Bindings.Structures.Difficulty;
using osu.Native.Bindings.Structures.Performance;
using osu.Native.Bindings.Structures.Scores;
using System.IO;

namespace osu.Native.Bindings.Difficulty;

public class OsuDifficultyCalculator : DifficultyCalculator<OsuDifficultyAttributes, OsuPerformanceAttributes, OsuScore>
{
    public OsuDifficultyCalculator(FileInfo file) : base(file) { }

    public OsuDifficultyCalculator(string text) : base(text) { }

    public override OsuDifficultyAttributes CalculateDifficulty(Mod[] mods)
    {
        OsuDifficultyAttributes attributes = default;
        OsuNative.Execute(() => OsuNative.Difficulty_ComputeOsu(_beatmapId, JsonConvert.SerializeObject(mods), out attributes));
        return attributes;
    }

    public override OsuPerformanceAttributes CalculatePerformance(OsuDifficultyAttributes diffAttributes, OsuScore score)
    {
        OsuPerformanceAttributes attributes = default;
        OsuNative.Execute(() => OsuNative.Performance_ComputeOsu(_beatmapId, diffAttributes, score.ToNative(), out attributes));
        return attributes;
    }
}