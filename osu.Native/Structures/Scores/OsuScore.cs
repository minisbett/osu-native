// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using osu.Native.Helpers;
using System.Linq;
using System.Runtime.InteropServices;

namespace osu.Native.Structures.Scores;

/// <summary>
/// Represents score information for the osu! ruleset in the context of PP calculation.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct OsuScore : IScore
{
    public char* Mods;
    public int MaxCombo;
    public int Count100;
    public int Count50;
    public int CountMiss;
    public int CountLargeTickMiss;
    public int CountSliderTailMiss;

    /// <inheritdoc/>
    public readonly ScoreInfo ToScoreInfo(FlatWorkingBeatmap workingBeatmap)
    {
        Ruleset ruleset = new OsuRuleset();
        IBeatmap beatmap = workingBeatmap.GetPlayableBeatmap(ruleset.RulesetInfo);
        int count300 = beatmap.HitObjects.Count - Count100 - Count50 - CountMiss;

        return new ScoreInfo(beatmap.BeatmapInfo, ruleset.RulesetInfo)
        {
            Mods = ModsHelper.ParseMods(ruleset, new(Mods)),
            MaxCombo = MaxCombo,
            Accuracy = (double)(300 * count300 + 100 * Count100 + 50 * Count50 + CountMiss) / (beatmap.HitObjects.Count * 300),
            Statistics = new()
            {
                { HitResult.Great, count300 },
                { HitResult.Ok, Count100 },
                { HitResult.Meh, Count50 },
                { HitResult.Miss, CountMiss },
                { HitResult.LargeTickMiss, CountLargeTickMiss },
                { HitResult.SliderTailHit, beatmap.HitObjects.Count(x => x is Slider) - CountSliderTailMiss }
            }
        };
    }
}
