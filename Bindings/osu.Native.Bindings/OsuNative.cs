using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace osu.Native.Bindings;

public static partial class OsuNative
{
    private const string LIB_PATH = "C:\\Users\\mini\\source\\repos\\minisbett\\osu-native\\artifacts\\publish\\osu.Native\\release\\osu.Native.dll";

    #region Beatmap

    [LibraryImport(LIB_PATH, EntryPoint = "Beatmap_CreateFromFile")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Beatmap_CreateFromFile([MarshalAs(UnmanagedType.LPStr)] string filePath, out int contextId);

    [LibraryImport(LIB_PATH, EntryPoint = "Beatmap_CreateFromText")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Beatmap_CreateFromText([MarshalAs(UnmanagedType.LPStr)] string text, out int contextId);

    [LibraryImport(LIB_PATH, EntryPoint = "Beatmap_Destroy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Beatmap_Destroy(int contextId);

    #endregion

    #region Difficulty

    [LibraryImport(LIB_PATH, EntryPoint = "Difficulty_ComputeOsu")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Difficulty_ComputeOsu(int beatmapContextId, uint mods, out OsuDifficultyAttributes attributes);

    [LibraryImport(LIB_PATH, EntryPoint = "Difficulty_ComputeTaiko")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Difficulty_ComputeTaiko(int beatmapContextId, uint mods, out TaikoDifficultyAttributes attributes);

    [LibraryImport(LIB_PATH, EntryPoint = "Difficulty_ComputeCatch")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Difficulty_ComputeCatch(int beatmapContextId, uint mods, out CatchDifficultyAttributes attributes);

    [LibraryImport(LIB_PATH, EntryPoint = "Difficulty_ComputeMania")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Difficulty_ComputeMania(int beatmapContextId, uint mods, out ManiaDifficultyAttributes attributes);

    #endregion

    #region Performance

    [LibraryImport(LIB_PATH, EntryPoint = "Performance_ComputeOsu")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Performance_ComputeOsu(int beatmapContextId, OsuDifficultyAttributes diffAttributes, uint mods, OsuScore score,
                                                     out OsuPerformanceAttributes attributes);

    [LibraryImport(LIB_PATH, EntryPoint = "Performance_ComputeTaiko")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Performance_ComputeTaiko(int beatmapContextId, TaikoDifficultyAttributes diffAttributes, uint mods, TaikoScore score,
                                                       out TaikoPerformanceAttributes attributes);

    [LibraryImport(LIB_PATH, EntryPoint = "Performance_ComputeCatch")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Performance_ComputeCatch(int beatmapContextId, CatchDifficultyAttributes diffAttributes, uint mods, CatchScore score,
                                                       out CatchPerformanceAttributes attributes);

    [LibraryImport(LIB_PATH, EntryPoint = "Performance_ComputeMania")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Performance_ComputeMania(int beatmapContextId, ManiaDifficultyAttributes diffAttributes, uint mods, ManiaScore score,
                                                       out ManiaPerformanceAttributes attributes);

    #endregion
}
