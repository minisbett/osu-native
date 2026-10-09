using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using osu.Native.Structures;
using osu.Native.Structures.Difficulty;
using osu.Native.Structures.Performance;

namespace osu.Native.Tests;

internal static unsafe partial class NativeApi
{
    private const string library_name = "native/osu.Native";

    #region Error handling

    [LibraryImport(library_name, EntryPoint = "ErrorHandler_GetLastMessage")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint ErrorHandler_GetLastMessage();

    #endregion

    #region Rulesets

    [LibraryImport(library_name, EntryPoint = "Ruleset_CreateFromId")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode Ruleset_CreateFromId(int id, out NativeRuleset ruleset);

    [LibraryImport(library_name, EntryPoint = "Ruleset_Destroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode Ruleset_Destroy(uint handle);

    #endregion

    #region Beatmaps

    [LibraryImport(library_name, EntryPoint = "Beatmap_CreateFromText", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode Beatmap_CreateFromText(string text, out NativeBeatmap beatmap);

    [LibraryImport(library_name, EntryPoint = "Beatmap_Destroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode Beatmap_Destroy(uint handle);

    #endregion

    #region Mod collections

    [LibraryImport(library_name, EntryPoint = "ModsCollection_Create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode ModsCollection_Create(out NativeModsCollection collection);

    [LibraryImport(library_name, EntryPoint = "ModsCollection_Add")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode ModsCollection_Add(uint collection, uint mod);

    [LibraryImport(library_name, EntryPoint = "ModsCollection_Destroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode ModsCollection_Destroy(uint handle);

    #endregion

    #region Mods

    [LibraryImport(library_name, EntryPoint = "Mod_Create", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode Mod_Create(string acronym, out NativeMod mod);

    [LibraryImport(library_name, EntryPoint = "Mod_SetSettingBool", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode Mod_SetSettingBool(uint mod, string key, [MarshalAs(UnmanagedType.I1)] bool value);

    [LibraryImport(library_name, EntryPoint = "Mod_SetSettingInteger", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode Mod_SetSettingInteger(uint mod, string key, int value);

    [LibraryImport(library_name, EntryPoint = "Mod_SetSettingFloat", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode Mod_SetSettingFloat(uint mod, string key, float value);

    [LibraryImport(library_name, EntryPoint = "Mod_Destroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode Mod_Destroy(uint handle);

    #endregion

    #region Osu calculators

    [LibraryImport(library_name, EntryPoint = "OsuDifficultyCalculator_Create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode OsuDifficultyCalculator_Create(uint ruleset, uint beatmap, NativeOsuDifficultyCalculator* calculator);

    [LibraryImport(library_name, EntryPoint = "OsuDifficultyCalculator_Calculate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode OsuDifficultyCalculator_Calculate(uint calculator, uint mods, NativeOsuDifficultyAttributes* attributes);

    [LibraryImport(library_name, EntryPoint = "OsuDifficultyCalculator_CalculateTimed")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode OsuDifficultyCalculator_CalculateTimed(uint calculator, uint mods, NativeTimedOsuDifficultyAttributes* attributes, int* size);

    [LibraryImport(library_name, EntryPoint = "OsuDifficultyCalculator_Destroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode OsuDifficultyCalculator_Destroy(uint handle);

    [LibraryImport(library_name, EntryPoint = "OsuPerformanceCalculator_Create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode OsuPerformanceCalculator_Create(NativeOsuPerformanceCalculator* calculator);

    [LibraryImport(library_name, EntryPoint = "OsuPerformanceCalculator_Calculate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode OsuPerformanceCalculator_Calculate(uint calculator, NativeScoreInfo score, NativeOsuDifficultyAttributes difficulty, NativeOsuPerformanceAttributes* attributes);

    [LibraryImport(library_name, EntryPoint = "OsuPerformanceCalculator_Destroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode OsuPerformanceCalculator_Destroy(uint handle);

    #endregion

    #region Taiko calculators

    [LibraryImport(library_name, EntryPoint = "TaikoDifficultyCalculator_Create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode TaikoDifficultyCalculator_Create(uint ruleset, uint beatmap, NativeTaikoDifficultyCalculator* calculator);

    [LibraryImport(library_name, EntryPoint = "TaikoDifficultyCalculator_Calculate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode TaikoDifficultyCalculator_Calculate(uint calculator, uint mods, NativeTaikoDifficultyAttributes* attributes);

    [LibraryImport(library_name, EntryPoint = "TaikoDifficultyCalculator_CalculateTimed")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode TaikoDifficultyCalculator_CalculateTimed(uint calculator, uint mods, NativeTimedTaikoDifficultyAttributes* attributes, int* size);

    [LibraryImport(library_name, EntryPoint = "TaikoDifficultyCalculator_Destroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode TaikoDifficultyCalculator_Destroy(uint handle);

    [LibraryImport(library_name, EntryPoint = "TaikoPerformanceCalculator_Create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode TaikoPerformanceCalculator_Create(NativeTaikoPerformanceCalculator* calculator);

    [LibraryImport(library_name, EntryPoint = "TaikoPerformanceCalculator_Calculate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode TaikoPerformanceCalculator_Calculate(uint calculator, NativeScoreInfo score, NativeTaikoDifficultyAttributes difficulty, NativeTaikoPerformanceAttributes* attributes);

    [LibraryImport(library_name, EntryPoint = "TaikoPerformanceCalculator_Destroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode TaikoPerformanceCalculator_Destroy(uint handle);

    #endregion

    #region Catch calculators

    [LibraryImport(library_name, EntryPoint = "CatchDifficultyCalculator_Create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode CatchDifficultyCalculator_Create(uint ruleset, uint beatmap, NativeCatchDifficultyCalculator* calculator);

    [LibraryImport(library_name, EntryPoint = "CatchDifficultyCalculator_Calculate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode CatchDifficultyCalculator_Calculate(uint calculator, uint mods, NativeCatchDifficultyAttributes* attributes);

    [LibraryImport(library_name, EntryPoint = "CatchDifficultyCalculator_CalculateTimed")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode CatchDifficultyCalculator_CalculateTimed(uint calculator, uint mods, NativeTimedCatchDifficultyAttributes* attributes, int* size);

    [LibraryImport(library_name, EntryPoint = "CatchDifficultyCalculator_Destroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode CatchDifficultyCalculator_Destroy(uint handle);

    [LibraryImport(library_name, EntryPoint = "CatchPerformanceCalculator_Create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode CatchPerformanceCalculator_Create(NativeCatchPerformanceCalculator* calculator);

    [LibraryImport(library_name, EntryPoint = "CatchPerformanceCalculator_Calculate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode CatchPerformanceCalculator_Calculate(uint calculator, NativeScoreInfo score, NativeCatchDifficultyAttributes difficulty, NativeCatchPerformanceAttributes* attributes);

    [LibraryImport(library_name, EntryPoint = "CatchPerformanceCalculator_Destroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode CatchPerformanceCalculator_Destroy(uint handle);

    #endregion

    #region Mania calculators

    [LibraryImport(library_name, EntryPoint = "ManiaDifficultyCalculator_Create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode ManiaDifficultyCalculator_Create(uint ruleset, uint beatmap, NativeManiaDifficultyCalculator* calculator);

    [LibraryImport(library_name, EntryPoint = "ManiaDifficultyCalculator_Calculate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode ManiaDifficultyCalculator_Calculate(uint calculator, uint mods, NativeManiaDifficultyAttributes* attributes);

    [LibraryImport(library_name, EntryPoint = "ManiaDifficultyCalculator_CalculateTimed")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode ManiaDifficultyCalculator_CalculateTimed(uint calculator, uint mods, NativeTimedManiaDifficultyAttributes* attributes, int* size);

    [LibraryImport(library_name, EntryPoint = "ManiaDifficultyCalculator_Destroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode ManiaDifficultyCalculator_Destroy(uint handle);

    [LibraryImport(library_name, EntryPoint = "ManiaPerformanceCalculator_Create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode ManiaPerformanceCalculator_Create(NativeManiaPerformanceCalculator* calculator);

    [LibraryImport(library_name, EntryPoint = "ManiaPerformanceCalculator_Calculate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode ManiaPerformanceCalculator_Calculate(uint calculator, NativeScoreInfo score, NativeManiaDifficultyAttributes difficulty, NativeManiaPerformanceAttributes* attributes);

    [LibraryImport(library_name, EntryPoint = "ManiaPerformanceCalculator_Destroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ErrorCode ManiaPerformanceCalculator_Destroy(uint handle);

    #endregion
}