// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Taiko;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania;
using osu.Native.Helpers;
using osu.Native.Objects;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Functions;

public unsafe static class Difficulty
{
    /// <summary>
    /// Computes the difficulty attributes of the specified beatmap in the osu! ruleset.
    /// </summary>
    /// <param name="beatmap">The native object referencing the beatmap.</param>
    /// <param name="diffAttributes">The computed difficulty attributes.</param>
    [UnmanagedCallersOnly(EntryPoint = "Difficulty_ComputeOsu", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeOsu(NativeBeatmap beatmap, char* mods, OsuDifficultyAttributes* diffAttributes)
    {
        try
        {
            Ruleset ruleset = new OsuRuleset();
            Mod[] rulesetMods = ModsHelper.ParseMods(ruleset, new(mods));

            FlatWorkingBeatmap workingBeatmap = beatmap.Resolve();
            DifficultyCalculator calculator = ruleset.CreateDifficultyCalculator(workingBeatmap);
            *diffAttributes = (OsuDifficultyAttributes)calculator.Calculate(rulesetMods)!;

            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            ErrorHandler.SetLastError(ex.ToString());
            return ErrorHelper.FromException(ex);
        }
    }

    /// <summary>
    /// Computes the difficulty attributes of the specified beatmap in the osu!taiko ruleset.
    /// </summary>
    /// <param name="beatmap">The native object referencing the beatmap.</param>
    /// <param name="diffAttributes">The computed difficulty attributes.</param>
    [UnmanagedCallersOnly(EntryPoint = "Difficulty_ComputeTaiko", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeTaiko(NativeBeatmap beatmap, char* mods, TaikoDifficultyAttributes* diffAttributes)
    {
        try
        {
            Ruleset ruleset = new TaikoRuleset();
            Mod[] rulesetMods = ModsHelper.ParseMods(ruleset, new(mods));

            FlatWorkingBeatmap workingBeatmap = beatmap.Resolve();
            DifficultyCalculator calculator = ruleset.CreateDifficultyCalculator(workingBeatmap);
            *diffAttributes = (TaikoDifficultyAttributes)calculator.Calculate(rulesetMods)!;

            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            ErrorHandler.SetLastError(ex.ToString());
            return ErrorHelper.FromException(ex);
        }
    }

    /// <summary>
    /// Computes the difficulty attributes of the specified beatmap in the osu!catch ruleset.
    /// </summary>
    /// <param name="beatmap">The native object referencing the beatmap.</param>
    /// <param name="diffAttributes">The computed difficulty attributes.</param>
    [UnmanagedCallersOnly(EntryPoint = "Difficulty_ComputeCatch", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeCatch(NativeBeatmap beatmap, char* mods, CatchDifficultyAttributes* diffAttributes)
    {
        try
        {
            Ruleset ruleset = new CatchRuleset();
            Mod[] rulesetMods = ModsHelper.ParseMods(ruleset, new(mods));

            FlatWorkingBeatmap workingBeatmap = beatmap.Resolve();
            DifficultyCalculator calculator = ruleset.CreateDifficultyCalculator(workingBeatmap);
            *diffAttributes = (CatchDifficultyAttributes)calculator.Calculate(rulesetMods)!;

            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            ErrorHandler.SetLastError(ex.ToString());
            return ErrorHelper.FromException(ex);
        }
    }

    /// <summary>
    /// Computes the difficulty attributes of the specified beatmap in the osu!mania ruleset.
    /// </summary>
    /// <param name="beatmap">The native object referencing the beatmap.</param>
    /// <param name="diffAttributes">The computed difficulty attributes.</param>
    [UnmanagedCallersOnly(EntryPoint = "Difficulty_ComputeMania", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeMania(NativeBeatmap beatmap, char* mods, ManiaDifficultyAttributes* diffAttributes)
    {
        try
        {
            Ruleset ruleset = new ManiaRuleset();
            Mod[] rulesetMods = ModsHelper.ParseMods(ruleset, new(mods));

            FlatWorkingBeatmap workingBeatmap = beatmap.Resolve();
            DifficultyCalculator calculator = ruleset.CreateDifficultyCalculator(workingBeatmap);
            *diffAttributes = (ManiaDifficultyAttributes)calculator.Calculate(rulesetMods)!;

            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            ErrorHandler.SetLastError(ex.ToString());
            return ErrorHelper.FromException(ex);
        }
    }
}