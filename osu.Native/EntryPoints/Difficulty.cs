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
using osu.Game.Rulesets.Osu.Difficulty;
using osu.Game.Rulesets.Taiko.Difficulty;
using osu.Game.Rulesets.Catch.Difficulty;
using osu.Game.Rulesets.Mania.Difficulty;
using osu.Native.Helpers;
using osu.Native.Objects;

namespace osu.Native.EntryPoints;

public unsafe static class Difficulty
{
    /// <summary>
    /// Computes the difficulty attributes of the specified beatmap in the osu! ruleset.
    /// </summary>
    /// <param name="beatmap">The native object referencing the beatmap.</param>
    /// <param name="diffAttributes">The computed difficulty attributes.</param>
    [UnmanagedCallersOnly(EntryPoint = "Difficulty_ComputeOsu", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeOsu(NativeObject<FlatWorkingBeatmap> beatmap, char* mods, OsuDifficultyAttributes* diffAttributes)
    {
        ErrorCode error = ComputeDifficulty<OsuRuleset>(beatmap, mods, out IDifficultyAttributes attributes);
        if (error > ErrorCode.Success)
            return error;

        *diffAttributes = (OsuDifficultyAttributes)attributes;

        return ErrorCode.Success;
    }

    /// <summary>
    /// Computes the difficulty attributes of the specified beatmap in the osu!taiko ruleset.
    /// </summary>
    /// <param name="beatmap">The native object referencing the beatmap.</param>
    /// <param name="diffAttributes">The computed difficulty attributes.</param>
    [UnmanagedCallersOnly(EntryPoint = "Difficulty_ComputeTaiko", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeTaiko(NativeObject<FlatWorkingBeatmap> beatmap, char* mods, TaikoDifficultyAttributes* diffAttributes)
    {
        ErrorCode error = ComputeDifficulty<TaikoRuleset>(beatmap, mods, out IDifficultyAttributes attributes);
        if (error > ErrorCode.Success)
            return error;

        *diffAttributes = (TaikoDifficultyAttributes)attributes;

        return ErrorCode.Success;
    }

    /// <summary>
    /// Computes the difficulty attributes of the specified beatmap in the osu!catch ruleset.
    /// </summary>
    /// <param name="beatmap">The native object referencing the beatmap.</param>
    /// <param name="diffAttributes">The computed difficulty attributes.</param>
    [UnmanagedCallersOnly(EntryPoint = "Difficulty_ComputeCatch", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeCatch(NativeObject<FlatWorkingBeatmap> beatmap, char* mods, CatchDifficultyAttributes* diffAttributes)
    {
        ErrorCode error = ComputeDifficulty<CatchRuleset>(beatmap, mods, out IDifficultyAttributes attributes);
        if (error > ErrorCode.Success)
            return error;

        *diffAttributes = (CatchDifficultyAttributes)attributes;

        return ErrorCode.Success;
    }

    /// <summary>
    /// Computes the difficulty attributes of the specified beatmap in the osu!mania ruleset.
    /// </summary>
    /// <param name="beatmap">The native object referencing the beatmap.</param>
    /// <param name="diffAttributes">The computed difficulty attributes.</param>
    [UnmanagedCallersOnly(EntryPoint = "Difficulty_ComputeMania", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeMania(NativeObject<FlatWorkingBeatmap> beatmap, char* mods, ManiaDifficultyAttributes* diffAttributes)
    {
        ErrorCode error = ComputeDifficulty<ManiaRuleset>(beatmap, mods, out IDifficultyAttributes attributes);
        if (error > ErrorCode.Success)
            return error;

        *diffAttributes = (ManiaDifficultyAttributes)attributes;

        return ErrorCode.Success;
    }

    private static ErrorCode ComputeDifficulty<TRuleset>(NativeObject<FlatWorkingBeatmap> beatmap, char* mods, out IDifficultyAttributes attributes)
        where TRuleset : Ruleset, new()
    {
        try
        {
            Ruleset ruleset = new TRuleset();
            Mod[] rulesetMods = ModsHelper.ParseMods(ruleset, new(mods));

            FlatWorkingBeatmap workingBeatmap = beatmap.Resolve();
            DifficultyCalculator calculator = ruleset.CreateDifficultyCalculator(workingBeatmap);
            attributes = calculator.Calculate(rulesetMods);

            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            attributes = null!;
            ErrorHandler.SetLastError(ex.Message);
            return ErrorHelper.FromException(ex);
        }
    }
}