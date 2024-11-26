// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Taiko;
using osu.Game.Scoring;
using osu.Native.Helpers;
using System.Runtime.InteropServices;

namespace osu.Native.Structures.Scores;

/// <summary>
/// Represents score information for the osu!taiko ruleset in the context of PP calculation.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct TaikoScore : IScore
{
    public char* Mods;
    public int MaxCombo;
    public int CountGood;
    public int CountMiss;

    /// <inheritdoc/>
    public readonly ScoreInfo ToScoreInfo(FlatWorkingBeatmap workingBeatmap)
    {
        Ruleset ruleset = new TaikoRuleset();
        IBeatmap beatmap = workingBeatmap.GetPlayableBeatmap(ruleset.RulesetInfo);
        int countGreat = beatmap.GetMaxCombo() - CountGood - CountMiss;

        return new ScoreInfo(beatmap.BeatmapInfo, ruleset.RulesetInfo)
        {
            Mods = ModsHelper.ParseMods(ruleset, new(Mods)),
            MaxCombo = MaxCombo,
            Accuracy = (double)(2 * countGreat + CountGood) / (2 * (countGreat + CountGood + CountMiss)),
            Statistics = new()
            {
                { HitResult.Great, countGreat },
                { HitResult.Ok, CountGood },
                { HitResult.Meh, 0 },
                { HitResult.Miss, CountMiss },
            }
        };
    }
}
