// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Catch.Objects;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Mania.Objects;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Taiko;
using osu.Game.Scoring;
using osu.Native.Helpers;
using System.Linq;
using System.Runtime.InteropServices;

namespace osu.Native.Structs;

/// <summary>
/// The common class for all structs representing score information in the context of PP calculation.
/// </summary>
public unsafe interface IScore
{
    public char* Mods { get; }

    /// <summary>
    /// Builds a <see cref="ScoreInfo"/> object with the available score information.
    /// </summary>
    /// <param name="beatmap">The beatmap the score corresponds to.</param>
    /// <returns>The built <see cref="ScoreInfo"/> object.</returns>
    public ScoreInfo ToScoreInfo(FlatWorkingBeatmap beatmap);
}

/// <summary>
/// Represents score information for the osu! ruleset in the context of PP calculation.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct OsuScore : IScore
{
    public char* Mods { get; }
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

/// <summary>
/// Represents score information for the osu!taiko ruleset in the context of PP calculation.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct TaikoScore : IScore
{
    public char* Mods { get; }
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

/// <summary>
/// Represents score information for the osu!catch ruleset in the context of PP calculation.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct CatchScore : IScore
{
    public char* Mods { get; }
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

/// <summary>
/// Represents score information for the osu!mania ruleset in the context of PP calculation.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct ManiaScore : IScore
{
    public char* Mods { get; }
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