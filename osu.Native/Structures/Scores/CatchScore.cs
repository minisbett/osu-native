// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Catch.Objects;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using osu.Native.Helpers;
using System.Linq;
using System.Runtime.InteropServices;

namespace osu.Native.Structures.Scores;

/// <summary>
/// Represents score information for the osu!catch ruleset in the context of PP calculation.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct CatchScore : IScore
{
    public char* Mods;
    public int MaxCombo;
    public int CountDroplets;
    public int CountTinyDroplets;
    public int CountTinyMisses;
    public int CountMiss;

    /// <inheritdoc/>
    public readonly ScoreInfo ToScoreInfo(FlatWorkingBeatmap workingBeatmap)
    {
        Ruleset ruleset = new CatchRuleset();
        IBeatmap beatmap = workingBeatmap.GetPlayableBeatmap(ruleset.RulesetInfo);
        int maxDroplets = beatmap.HitObjects.OfType<JuiceStream>().Sum(s => s.NestedHitObjects.OfType<TinyDroplet>().Count());
        int maxFruits = beatmap.HitObjects.Sum(h => h is Fruit ? 1 : (h as JuiceStream)?.NestedHitObjects.Count(n => n is Fruit) ?? 0);
        int countFruits = maxFruits - (CountMiss - (maxDroplets - CountDroplets));

        return new ScoreInfo(beatmap.BeatmapInfo, ruleset.RulesetInfo)
        {
            Mods = ModsHelper.ParseMods(ruleset, new(Mods)),
            MaxCombo = MaxCombo,
            Accuracy = (double)(countFruits + CountDroplets + CountTinyDroplets) / (countFruits + CountDroplets + CountTinyDroplets + CountTinyMisses + CountMiss),
            Statistics = new()
            {
                { HitResult.Great, countFruits },
                { HitResult.LargeTickHit, CountDroplets },
                { HitResult.SmallTickHit, CountTinyDroplets },
                { HitResult.SmallTickMiss, CountTinyMisses },
                { HitResult.Miss, CountMiss },
            }
        };
    }
}
