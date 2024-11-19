using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace osu.Native.Bindings;

public static class OsuNative
{
    private const string LIB_PATH = "C:\\Users\\mini\\source\\repos\\minisbett\\osu-native\\artifacts\\publish\\osu.Native\\release\\osu.Native.dll";

    #region Beatmap

    [DllImport(LIB_PATH, EntryPoint = "Beatmap_CreateFromFile", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Beatmap_CreateFromFile([MarshalAs(UnmanagedType.LPStr)] string filePath, out int contextId);

    [DllImport(LIB_PATH, EntryPoint = "Beatmap_CreateFromText", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Beatmap_CreateFromText([MarshalAs(UnmanagedType.LPStr)] string text, out int contextId);

    [DllImport(LIB_PATH, EntryPoint = "Beatmap_Destroy", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Beatmap_Destroy(int contextId);

    #endregion

    #region Difficulty

    [DllImport(LIB_PATH, EntryPoint = "Difficulty_ComputeOsu", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Difficulty_ComputeOsu(int beatmapContextId, out OsuDifficultyAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Difficulty_ComputeTaiko", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Difficulty_ComputeTaiko(int beatmapContextId, out TaikoDifficultyAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Difficulty_ComputeCatch", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Difficulty_ComputeCatch(int beatmapContextId, out CatchDifficultyAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Difficulty_ComputeMania", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Difficulty_ComputeMania(int beatmapContextId, out ManiaDifficultyAttributes attributes);

    #endregion

    #region Performance

    [DllImport(LIB_PATH, EntryPoint = "Performance_ComputeOsu", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Performance_ComputeOsu(int beatmapContextId, OsuDifficultyAttributes diffAttributes, OsuScore score,
                                                    out OsuPerformanceAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Performance_ComputeTaiko", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Performance_ComputeTaiko(int beatmapContextId, TaikoDifficultyAttributes diffAttributes, TaikoScore score,
                                                      out TaikoPerformanceAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Performance_ComputeCatch", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Performance_ComputeCatch(int beatmapContextId, CatchDifficultyAttributes diffAttributes, CatchScore score,
                                                      out CatchPerformanceAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Performance_ComputeMania", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Performance_ComputeMania(int beatmapContextId, ManiaDifficultyAttributes diffAttributes, ManiaScore score,
                                                      out ManiaPerformanceAttributes attributes);

    #endregion
}
