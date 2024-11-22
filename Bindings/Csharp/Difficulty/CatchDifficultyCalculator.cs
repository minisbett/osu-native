// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Native.Bindings.Models;
using osu.Native.Bindings.Models.Catch;
using System.IO;

namespace osu.Native.Bindings.Difficulty;

public class CatchDifficultyCalculator : DifficultyCalculator<CatchDifficultyAttributes, CatchPerformanceAttributes, CatchScore>
{
    public CatchDifficultyCalculator(FileInfo file) : base(file) { }

    public CatchDifficultyCalculator(string text) : base(text) { }

    public override CatchDifficultyAttributes CalculateDifficulty(Mod[] mods)
    {
        OsuNative.Difficulty_ComputeCatch(_beatmapContextId, JsonConvert.SerializeObject(mods), out CatchDifficultyAttributes attributes);
        return attributes;
    }

    public override CatchPerformanceAttributes CalculatePerformance(CatchDifficultyAttributes diffAttributes, CatchScore score)
    {
        OsuNative.Performance_ComputeCatch(_beatmapContextId, diffAttributes, score.ToNative(), out CatchPerformanceAttributes attributes);
        return attributes;
    }
}
