// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Mania.Objects;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using osu.Native.Helpers;
using System.Linq;
using System.Runtime.InteropServices;

namespace osu.Native.Structures.Scores;

/// <summary>
/// Represents score information for the osu!mania ruleset in the context of PP calculation.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct ManiaScore : IScore
{
    public char* Mods;
    public int CountGreat;
    public int CountGood;
    public int CountOk;
    public int CountMeh;
    public int CountMiss;

    /// <summary>
    /// On osu!lazer, hold notes provide two judgements instead of one. Therefore, a differentiation between osu!lazer and osu!stable needs to be made here.
    /// </summary>
    public bool IsLazer;

    /// <inheritdoc/>
    public readonly ScoreInfo ToScoreInfo(FlatWorkingBeatmap workingBeatmap)
    {
        Ruleset ruleset = new ManiaRuleset();
        IBeatmap beatmap = workingBeatmap.GetPlayableBeatmap(ruleset.RulesetInfo);
        int totalHits = beatmap.HitObjects.Count + (IsLazer ? 2 : 1) * beatmap.HitObjects.Count(ho => ho is HoldNote);
        int countPerfect = totalHits - CountMiss - CountMeh - CountOk - CountGood - CountGreat;

        return new ScoreInfo(beatmap.BeatmapInfo, ruleset.RulesetInfo)
        {
            // Mania does not use the maximum combo or accuracy in PP calculation (it calculates the accuracy itself), therefore it is omitted here.
            Mods = ModsHelper.ParseMods(ruleset, new(Mods)),
            Statistics = new()
            {
                { HitResult.Perfect, countPerfect },
                { HitResult.Great, CountGreat },
                { HitResult.Good, CountGood },
                { HitResult.Ok, CountOk },
                { HitResult.Meh, CountMeh },
                { HitResult.Miss, CountMiss }
            }
        };
    }
}