using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;

namespace osu.Native.Structures;

/// <summary>
/// Holds all information about a score that are relevant for performance calculations.
/// </summary>
public struct NativeScoreInfo
{
    public RulesetHandle RulesetHandle;
    public BeatmapHandle BeatmapHandle;
    public ModsCollectionHandle ModsHandle;
    public int MaxCombo;
    public double Accuracy;
    public long? LegacyTotalScore;
    public int CountMiss;
    public int CountMeh;
    public int CountOk;
    public int CountGood;
    public int CountGreat;
    public int CountPerfect;
    public int CountSliderTailHit;
    public int CountLargeTickMiss;

    /// <summary>
    /// Constructs a <see cref="ScoreInfo"/> from the native score information.
    /// </summary>
    /// <returns>The constructed <see cref="ScoreInfo"/>.</returns>
    public readonly ScoreInfo ToScoreInfo()
    {
        Ruleset ruleset = RulesetHandle.Resolve();
        FlatWorkingBeatmap beatmap = BeatmapHandle.Resolve();
        Mod[] mods = [.. ModsHandle.Resolve().Select(x => x.ToMod(ruleset))];

        return new()
        {
            BeatmapInfo = beatmap.BeatmapInfo,
            Mods = mods,
            MaxCombo = MaxCombo,
            Accuracy = Accuracy,
            LegacyTotalScore = LegacyTotalScore,
            Statistics =
            {
                [HitResult.Miss] = CountMiss,
                [HitResult.Meh] = CountMeh,
                [HitResult.Ok] = CountOk,
                [HitResult.Good] = CountGood,
                [HitResult.Great] = CountGreat,
                [HitResult.Perfect] = CountPerfect,
                [HitResult.SliderTailHit] = CountSliderTailHit,
                [HitResult.LargeTickMiss] = CountLargeTickMiss
            }
        };
    }
}
