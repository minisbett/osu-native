// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps.Legacy;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

namespace osu.Native.EntryPoints;

public unsafe static class DifficultyEntryPoints
{
    [UnmanagedCallersOnly(EntryPoint = "Difficulty_ComputeOsu", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeOsu(int beatmapContextId, uint mods, OsuDifficultyAttributes* diffAttributes)
    {
        ErrorCode error = ComputeDifficulty(beatmapContextId, new OsuRuleset(), mods, out IDifficultyAttributes attributes);
        if (error > ErrorCode.Success)
            return error;

        *diffAttributes = (OsuDifficultyAttributes)attributes;

        return ErrorCode.Success;
    }

    [UnmanagedCallersOnly(EntryPoint = "Difficulty_ComputeTaiko", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeTaiko(int beatmapContextId, uint mods, TaikoDifficultyAttributes* diffAttributes)
    {
        ErrorCode error = ComputeDifficulty(beatmapContextId, new TaikoRuleset(), mods, out IDifficultyAttributes attributes);
        if (error > ErrorCode.Success)
            return error;

        *diffAttributes = (TaikoDifficultyAttributes)attributes;

        return ErrorCode.Success;
    }

    [UnmanagedCallersOnly(EntryPoint = "Difficulty_ComputeCatch", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeCatch(int beatmapContextId, uint mods, CatchDifficultyAttributes* diffAttributes)
    {
        ErrorCode error = ComputeDifficulty(beatmapContextId, new CatchRuleset(), mods, out IDifficultyAttributes attributes);
        if (error > ErrorCode.Success)
            return error;

        *diffAttributes = (CatchDifficultyAttributes)attributes;

        return ErrorCode.Success;
    }

    [UnmanagedCallersOnly(EntryPoint = "Difficulty_ComputeMania", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeMania(int beatmapContextId, uint mods, ManiaDifficultyAttributes* diffAttributes)
    {
        ErrorCode error = ComputeDifficulty(beatmapContextId, new ManiaRuleset(), mods, out IDifficultyAttributes attributes);
        if (error > ErrorCode.Success)
            return error;

        *diffAttributes = (ManiaDifficultyAttributes)attributes;

        return ErrorCode.Success;
    }

    private static ErrorCode ComputeDifficulty(int beatmapContextId, Ruleset ruleset, uint mods, out IDifficultyAttributes attributes)
    {
        try
        {
            FlatWorkingBeatmap beatmap = Contexts.Beatmaps.Resolve(beatmapContextId);
            Mod[] rulesetMods = ruleset.ConvertFromLegacyMods((LegacyMods)mods).ToArray();
            DifficultyCalculator calculator = ruleset.CreateDifficultyCalculator(beatmap);
            
            attributes = calculator.Calculate(rulesetMods);
            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            attributes = null!;
            return Logger.Error(ErrorCodeHelper.FromException(ex), ex.ToString());
        }
    }
}