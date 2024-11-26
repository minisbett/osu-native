// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Osu.Difficulty;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Taiko.Difficulty;
using osu.Game.Rulesets.Taiko;
using osu.Game.Rulesets.Catch.Difficulty;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania.Difficulty;
using osu.Game.Rulesets.Mania;
using osu.Native.Objects;
using osu.Native.Helpers;
using osu.Native.Structures.Scores;

namespace osu.Native.EntryPoints;

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
        ErrorCode error = ComputePerformance<OsuRuleset>(beatmap, diffAttributes, score, out IPerformanceAttributes attributes);
        if (error > ErrorCode.Success)
            return error;

        *perfAttributes = (OsuPerformanceAttributes)attributes;

        return ErrorCode.Success;
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
        ErrorCode error = ComputePerformance<TaikoRuleset>(beatmap, diffAttributes, score, out IPerformanceAttributes attributes);
        if (error > ErrorCode.Success)
            return error;

        *perfAttributes = (TaikoPerformanceAttributes)attributes;

        return ErrorCode.Success;
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
        ErrorCode error = ComputePerformance<CatchRuleset>(beatmap, diffAttributes, score, out IPerformanceAttributes attributes);
        if (error > ErrorCode.Success)
            return error;

        *perfAttributes = (CatchPerformanceAttributes)attributes;

        return ErrorCode.Success;
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
        ErrorCode error = ComputePerformance<ManiaRuleset>(beatmap, diffAttributes, score, out IPerformanceAttributes attributes);
        if (error > ErrorCode.Success)
            return error;

        *perfAttributes = (ManiaPerformanceAttributes)attributes;

        return ErrorCode.Success;
    }

    private static ErrorCode ComputePerformance<TRuleset>(INativeObject<FlatWorkingBeatmap> beatmap, IDifficultyAttributes diffAttributes, IScore score,
                                                          out IPerformanceAttributes attributes) where TRuleset : Ruleset, new()
    {
        try
        {
            FlatWorkingBeatmap workingBeatmap = beatmap.Resolve();
            PerformanceCalculator calculator = new TRuleset().CreatePerformanceCalculator()!;
            attributes = calculator.Calculate(score.ToScoreInfo(workingBeatmap), diffAttributes);

            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            attributes = null!;
            ErrorHandler.SetLastError(ex.ToString());
            return ErrorHelper.FromException(ex);
        }
    }
}
