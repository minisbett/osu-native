using osu.Game.Rulesets.Osu.Difficulty;
using osu.Game.Rulesets.Osu;
using osu.Native.Structs;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets;
using osu.Game.Scoring;
using System;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Beatmaps.Legacy;
using System.Linq;
using System.Collections.Generic;
using osu.Game.Rulesets.Scoring;

namespace osu.Native.EntryPoints;

public static unsafe class PerformanceEntryPoints
{
    [UnmanagedCallersOnly(EntryPoint = "Performance_ComputeOsu", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeDifficultyOsu(int beatmapContextId, NativeOsuDifficultyAttributes diffAttributes, uint mods, int combo,
                                                 OsuHitStatistics hitStatistics, NativeOsuPerformanceAttributes* perfAttributes)
    {
        if (!Context.Beatmaps.TryGetValue(beatmapContextId, out FlatWorkingBeatmap? beatmap))
            return Logger.Error(ErrorCode.ContextNotFound, "No beatmap with the specified context ID found.");

        try
        {
            Ruleset ruleset = new OsuRuleset();
            Mod[] rulesetMods = ruleset.ConvertFromLegacyMods((LegacyMods)mods).ToArray();
            PerformanceCalculator calculator = ruleset.CreatePerformanceCalculator()!;

            int totalResults = beatmap.GetPlayableBeatmap(ruleset.RulesetInfo).HitObjects.Count;
            Dictionary<HitResult, int> statistics = new()
            {
                { HitResult.Great, totalResults - hitStatistics.Count100 - hitStatistics.Count50 - hitStatistics.CountMiss },
                { HitResult.Ok, hitStatistics.Count100 },
                { HitResult.Meh, hitStatistics.Count50 },
                { HitResult.Miss, hitStatistics.CountMiss },
                { HitResult.LargeTickMiss, hitStatistics.CountLargeTickMiss },
                { HitResult.SliderTailHit, diffAttributes.SliderCount - hitStatistics.CountSliderTailMiss }
            };

            double accuracy = (300 * statistics[HitResult.Great] + 100 * statistics[HitResult.Ok] + 50 * statistics[HitResult.Meh] + statistics[HitResult.Miss])
                         / (totalResults * 300d);

            ScoreInfo score = new()
            {
                BeatmapInfo = beatmap.BeatmapInfo,
                Ruleset = ruleset.RulesetInfo,
                Mods = rulesetMods,
                MaxCombo = combo,
                Accuracy = accuracy,
                Statistics = statistics
            };

            PerformanceAttributes attributes = calculator.Calculate(score, StructHelper.StructToDifficultyAttributes(diffAttributes));
            *perfAttributes = StructHelper.PerformanceAttributesToStruct((OsuPerformanceAttributes)attributes);

            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            return Logger.Error(ErrorCode.Failure, ex.ToString());
        }
    }
}
