﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Osu.Difficulty;
using osu.Game.Rulesets.Osu;
using osu.Native.Structs;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets;
using System;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Beatmaps.Legacy;
using System.Linq;
using osu.Native.Helpers;
using NuGet.Packaging.Rules;
using System.Xml.Linq;
using osu.Game.Rulesets.Taiko.Difficulty;
using osu.Game.Rulesets.Taiko;
using osu.Game.Rulesets.Catch.Difficulty;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania.Difficulty;
using osu.Game.Rulesets.Mania;

namespace osu.Native.EntryPoints;

public static unsafe class PerformanceEntryPoints
{
    [UnmanagedCallersOnly(EntryPoint = "Performance_ComputeOsu", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeOsu(int beatmapContextId, OsuDifficultyAttributes diffAttributes, uint mods, OsuScore score, OsuPerformanceAttributes* perfAttributes)
    {
        ErrorCode error = ComputePerformance<OsuRuleset>(beatmapContextId, diffAttributes, mods, score, out IPerformanceAttributes attributes);
        if (error > ErrorCode.Success)
            return error;

        *perfAttributes = (OsuPerformanceAttributes)attributes;

        return ErrorCode.Success;
    }

    [UnmanagedCallersOnly(EntryPoint = "Performance_ComputeTaiko", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeTaiko(int beatmapContextId, TaikoDifficultyAttributes diffAttributes, uint mods, TaikoScore score, TaikoPerformanceAttributes* perfAttributes)
    {
        ErrorCode error = ComputePerformance<TaikoRuleset>(beatmapContextId, diffAttributes, mods, score, out IPerformanceAttributes attributes);
        if (error > ErrorCode.Success)
            return error;

        *perfAttributes = (TaikoPerformanceAttributes)attributes;

        Console.WriteLine("[osu-native] Difficulty PP: " + (*perfAttributes).Difficulty);
        Console.WriteLine("[osu-native] Accuracy PP: " + (*perfAttributes).Accuracy);
        Console.WriteLine("[osu-native] Effective Misscount: " + (*perfAttributes).EffectiveMissCount);
        Console.WriteLine("[osu-native] Estimated Unstable Rate: " + (*perfAttributes).EstimatedUnstableRate);
        Console.WriteLine("[osu-native] Total PP: " + (*perfAttributes).Total);

        return ErrorCode.Success;
    }

    [UnmanagedCallersOnly(EntryPoint = "Performance_ComputeCatch", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeCatch(int beatmapContextId, CatchDifficultyAttributes diffAttributes, uint mods, CatchScore score, CatchPerformanceAttributes* perfAttributes)
    {
        ErrorCode error = ComputePerformance<CatchRuleset>(beatmapContextId, diffAttributes, mods, score, out IPerformanceAttributes attributes);
        if (error > ErrorCode.Success)
            return error;

        *perfAttributes = (CatchPerformanceAttributes)attributes;

        return ErrorCode.Success;
    }

    [UnmanagedCallersOnly(EntryPoint = "Performance_ComputeMania", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeMania(int beatmapContextId, ManiaDifficultyAttributes diffAttributes, uint mods, ManiaScore score, ManiaPerformanceAttributes* perfAttributes)
    {
        ErrorCode error = ComputePerformance<ManiaRuleset>(beatmapContextId, diffAttributes, mods, score, out IPerformanceAttributes attributes);
        if (error > ErrorCode.Success)
            return error;

        *perfAttributes = (ManiaPerformanceAttributes)attributes;

        return ErrorCode.Success;
    }

    private static ErrorCode ComputePerformance<TRuleset>(int beatmapContextId, IDifficultyAttributes diffAttributes, uint mods, IScore score,
                                                out IPerformanceAttributes attributes) where TRuleset : Ruleset, new()
    {
        try
        {
            Ruleset ruleset = new TRuleset();
            FlatWorkingBeatmap beatmap = Contexts.Beatmaps.Resolve(beatmapContextId);
            Mod[] rulesetMods = ruleset.ConvertFromLegacyMods((LegacyMods)mods).ToArray();
            PerformanceCalculator calculator = ruleset.CreatePerformanceCalculator()!;

            attributes = calculator.Calculate(score.ToScoreInfo(ruleset, beatmap, rulesetMods), diffAttributes);
            Console.WriteLine(attributes.Total);
            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            attributes = null!;
            return Logger.Error(ErrorCodeHelper.FromException(ex), ex.ToString());
        }
    }
}
