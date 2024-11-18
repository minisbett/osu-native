// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using System.Linq;
using System.Runtime.InteropServices;

namespace osu.Native.Structs;

public interface IScore
{
    public ScoreInfo ToScoreInfo(Ruleset ruleset, FlatWorkingBeatmap beatmap, Mod[] mods);
}

[StructLayout(LayoutKind.Sequential)]
public struct OsuScore : IScore
{
    public int MaxCombo;
    public int Count100;
    public int Count50;
    public int CountMiss;
    public int CountLargeTickMiss;
    public int CountSliderTailMiss;

    public readonly ScoreInfo ToScoreInfo(Ruleset ruleset, FlatWorkingBeatmap workingBeatmap, Mod[] mods)
    {
        IBeatmap beatmap = workingBeatmap.GetPlayableBeatmap(ruleset.RulesetInfo);
        int count300 = beatmap.HitObjects.Count - Count100 - Count50 - CountMiss;

        return new ScoreInfo(beatmap.BeatmapInfo, ruleset.RulesetInfo)
        {
            Mods = mods,
            MaxCombo = MaxCombo,
            Accuracy = (300 * count300 + 100 * Count100 + 50 * Count50 + CountMiss) / (beatmap.HitObjects.Count * 300d),
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

[StructLayout(LayoutKind.Sequential)]
public struct TaikoScore : IScore
{
    public int MaxCombo;
    public int CountGood;
    public int CountMiss;

    public ScoreInfo ToScoreInfo(Ruleset ruleset, FlatWorkingBeatmap beatmap, Mod[] mods)
    {
        throw new System.NotImplementedException();
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct CatchScore : IScore
{
    public int MaxCombo;
    public int CountDroplets;
    public int CountTinyDroplets;
    public int CountTinyMisses;
    public int CountMiss;

    public ScoreInfo ToScoreInfo(Ruleset ruleset, FlatWorkingBeatmap beatmap, Mod[] mods)
    {
        throw new System.NotImplementedException();
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct ManiaScore : IScore
{
    public int MaxCombo;
    public int CountGreat;
    public int CountGood;
    public int CountOk;
    public int CountMeh;
    public int CountMiss;

    public ScoreInfo ToScoreInfo(Ruleset ruleset, FlatWorkingBeatmap beatmap, Mod[] mods)
    {
        throw new System.NotImplementedException();
    }
}