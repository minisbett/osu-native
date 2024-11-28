// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Taiko;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania;
using osu.Native.Objects;
using osu.Native.Helpers;
using osu.Native.Structures.Scores;
using osu.Native.Structures.Difficulty;
using osu.Native.Structures.Performance;

namespace osu.Native;

public static unsafe class Performance
{
    /// <summary>
    /// Computes the performance attributes of the specified beatmap and score with the specified difficulty attributes in the osu! ruleset.
    /// </summary>
    /// <param name="beatmap">The native object referencing the beatmap.</param>
    /// <param name="diffAttributes">The difficulty attributes.</param>
    /// <param name="score">The score information.</param>
    /// <param name="perfAttributes">The computed difficulty attributes.</param>
    [UnmanagedCallersOnly(EntryPoint = "Performance_ComputeOsu", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeOsu(NativeObject<FlatWorkingBeatmap> beatmap, OsuDifficultyAttributes diffAttributes, OsuScore score,
                                       OsuPerformanceAttributes* perfAttributes)
    {
        try
        {
            FlatWorkingBeatmap workingBeatmap = beatmap.Resolve();
            PerformanceCalculator calculator = new OsuRuleset().CreatePerformanceCalculator()!;
            *perfAttributes = (OsuPerformanceAttributes)calculator.Calculate(score.ToScoreInfo(workingBeatmap), diffAttributes)!;

            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            ErrorHandler.SetLastError(ex.ToString());
            return ErrorHelper.FromException(ex);
        }
    }


    /// <summary>
    /// Computes the performance attributes of the specified beatmap and score with the specified difficulty attributes in the osu!taiko ruleset.
    /// </summary>
    /// <param name="beatmap">The native object referencing the beatmap.</param>
    /// <param name="diffAttributes">The difficulty attributes.</param>
    /// <param name="score">The score information.</param>
    /// <param name="perfAttributes">The computed difficulty attributes.</param>
    [UnmanagedCallersOnly(EntryPoint = "Performance_ComputeTaiko", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeTaiko(NativeObject<FlatWorkingBeatmap> beatmap, TaikoDifficultyAttributes diffAttributes, TaikoScore score,
                                         TaikoPerformanceAttributes* perfAttributes)
    {
        try
        {
            FlatWorkingBeatmap workingBeatmap = beatmap.Resolve();
            PerformanceCalculator calculator = new TaikoRuleset().CreatePerformanceCalculator()!;
            *perfAttributes = (TaikoPerformanceAttributes)calculator.Calculate(score.ToScoreInfo(workingBeatmap), diffAttributes)!;

            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            ErrorHandler.SetLastError(ex.ToString());
            return ErrorHelper.FromException(ex);
        }
    }


    /// <summary>
    /// Computes the performance attributes of the specified beatmap and score with the specified difficulty attributes in the osu!catch ruleset.
    /// </summary>
    /// <param name="beatmap">The native object referencing the beatmap.</param>
    /// <param name="diffAttributes">The difficulty attributes.</param>
    /// <param name="score">The score information.</param>
    /// <param name="perfAttributes">The computed difficulty attributes.</param>
    [UnmanagedCallersOnly(EntryPoint = "Performance_ComputeCatch", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeCatch(NativeObject<FlatWorkingBeatmap> beatmap, CatchDifficultyAttributes diffAttributes, CatchScore score,
                                         CatchPerformanceAttributes* perfAttributes)
    {
        try
        {
            FlatWorkingBeatmap workingBeatmap = beatmap.Resolve();
            PerformanceCalculator calculator = new CatchRuleset().CreatePerformanceCalculator()!;
            *perfAttributes = (CatchPerformanceAttributes)calculator.Calculate(score.ToScoreInfo(workingBeatmap), diffAttributes)!;

            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            ErrorHandler.SetLastError(ex.ToString());
            return ErrorHelper.FromException(ex);
        }
    }

    /// <summary>
    /// Computes the performance attributes of the specified beatmap and score with the specified difficulty attributes in the osu!mania ruleset.
    /// </summary>
    /// <param name="beatmap">The native object referencing the beatmap.</param>
    /// <param name="diffAttributes">The difficulty attributes.</param>
    /// <param name="score">The score information.</param>
    /// <param name="perfAttributes">The computed difficulty attributes.</param>
    [UnmanagedCallersOnly(EntryPoint = "Performance_ComputeMania", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeMania(NativeObject<FlatWorkingBeatmap> beatmap, ManiaDifficultyAttributes diffAttributes, ManiaScore score,
                                         ManiaPerformanceAttributes* perfAttributes)
    {
        try
        {
            FlatWorkingBeatmap workingBeatmap = beatmap.Resolve();
            PerformanceCalculator calculator = new ManiaRuleset().CreatePerformanceCalculator()!;
            *perfAttributes = (ManiaPerformanceAttributes)calculator.Calculate(score.ToScoreInfo(workingBeatmap), diffAttributes)!;

            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            ErrorHandler.SetLastError(ex.ToString());
            return ErrorHelper.FromException(ex);
        }
    }
}
