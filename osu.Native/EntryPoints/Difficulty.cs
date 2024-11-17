using osu.Game.Beatmaps.Legacy;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using osu.Game.Rulesets.Difficulty;
using osu.Native.Structs;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Taiko;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Osu.Difficulty;
using osu.Game.Rulesets.Taiko.Difficulty;
using osu.Game.Rulesets.Catch.Difficulty;
using osu.Game.Rulesets.Mania.Difficulty;

namespace osu.Native.EntryPoints;

public unsafe static class DifficultyEntryPoints
{
    [UnmanagedCallersOnly(EntryPoint = "Difficulty_ComputeOsu", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeDifficultyOsu(int beatmapContextId, uint mods, NativeOsuDifficultyAttributes* attributes)
    {
        ErrorCode error = ComputeDifficulty(beatmapContextId, new OsuRuleset(), mods, out var diffAttributes);
        if (error > ErrorCode.Success)
            return error;

        *attributes = StructHelper.DifficultyAttributesToStruct((OsuDifficultyAttributes)diffAttributes);

        return ErrorCode.Success;
    }

    [UnmanagedCallersOnly(EntryPoint = "Difficulty_ComputeTaiko", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeDifficultyTaiko(int beatmapContextId, uint mods, NativeTaikoDifficultyAttributes* diffAttributes)
    {
        ErrorCode error = ComputeDifficulty(beatmapContextId, new TaikoRuleset(), mods, out var attributes);
        if (error > ErrorCode.Success)
            return error;

        *diffAttributes = StructHelper.DifficultyAttributesToStruct((TaikoDifficultyAttributes)attributes);

        return ErrorCode.Success;
    }

    [UnmanagedCallersOnly(EntryPoint = "Difficulty_ComputeCatch", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeDifficulty(int beatmapContextId, uint mods, NativeCatchDifficultyAttributes* diffAttributes)
    {
        ErrorCode error = ComputeDifficulty(beatmapContextId, new CatchRuleset(), mods, out var attributes);
        if (error > ErrorCode.Success)
            return error;

        *diffAttributes = StructHelper.DifficultyAttributesToStruct((CatchDifficultyAttributes)attributes);

        return ErrorCode.Success;
    }

    [UnmanagedCallersOnly(EntryPoint = "Difficulty_ComputeMania", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ComputeDifficultyMania(int beatmapContextId, uint mods, NativeManiaDifficultyAttributes* diffAttributes)
    {
        ErrorCode error = ComputeDifficulty(beatmapContextId, new ManiaRuleset(), mods, out var attributes);
        if (error > ErrorCode.Success)
            return error;

        *diffAttributes = StructHelper.DifficultyAttributesToStruct((ManiaDifficultyAttributes)attributes);

        return ErrorCode.Success;
    }

    private static ErrorCode ComputeDifficulty(int beatmapContextId, Ruleset ruleset, uint mods, out DifficultyAttributes attributes)
    {
        attributes = null!;
        if (!Context.Beatmaps.TryGetValue(beatmapContextId, out FlatWorkingBeatmap? beatmap))
            return Logger.Error(ErrorCode.ContextNotFound, "No beatmap with the specified context ID found.");

        try
        {
            Mod[] rulesetMods = ruleset.ConvertFromLegacyMods((LegacyMods)mods).ToArray();
            DifficultyCalculator calculator = ruleset.CreateDifficultyCalculator(beatmap);
            attributes = calculator.Calculate(rulesetMods);

            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            return Logger.Error(ErrorCode.Failure, ex.ToString());
        }
    }
}