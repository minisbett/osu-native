// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Native.Bindings.Models.Catch;
using osu.Native.Bindings.Models.Mania;
using osu.Native.Bindings.Models.Osu;
using osu.Native.Bindings.Models.Taiko;
using System.Runtime.InteropServices;

namespace osu.Native.Bindings;

public static class OsuNative
{
    private const string LIB_PATH = @"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\osu.Native\osu.Native.dll";

    [DllImport(LIB_PATH, EntryPoint = "GetLastErrorMessage", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.LPWStr)]
    public static extern string GetLastErrorMessage();

    #region Beatmap

    [DllImport(LIB_PATH, EntryPoint = "Beatmap_CreateFromFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Beatmap_CreateFromFile(string filePath, out int contextId);

    [DllImport(LIB_PATH, EntryPoint = "Beatmap_CreateFromText", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Beatmap_CreateFromText(string text, out int contextId);

    [DllImport(LIB_PATH, EntryPoint = "Beatmap_Destroy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Beatmap_Destroy(int contextId);

    #endregion

    #region Difficulty

    [DllImport(LIB_PATH, EntryPoint = "Difficulty_ComputeOsu", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    internal static extern ErrorCode Difficulty_ComputeOsu(int beatmapContextId, string mods, out OsuDifficultyAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Difficulty_ComputeTaiko", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    internal static extern ErrorCode Difficulty_ComputeTaiko(int beatmapContextId, string mods, out TaikoDifficultyAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Difficulty_ComputeCatch", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    internal static extern ErrorCode Difficulty_ComputeCatch(int beatmapContextId, string mods, out CatchDifficultyAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Difficulty_ComputeMania", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    internal static extern ErrorCode Difficulty_ComputeMania(int beatmapContextId, string mods, out ManiaDifficultyAttributes attributes);

    #endregion

    #region Performance

    [DllImport(LIB_PATH, EntryPoint = "Performance_ComputeOsu", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Performance_ComputeOsu(int beatmapContextId, OsuDifficultyAttributes diffAttributes, OsuScore.Native score,
                                                    out OsuPerformanceAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Performance_ComputeTaiko", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Performance_ComputeTaiko(int beatmapContextId, TaikoDifficultyAttributes diffAttributes, TaikoScore.Native score,
                                                      out TaikoPerformanceAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Performance_ComputeCatch", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Performance_ComputeCatch(int beatmapContextId, CatchDifficultyAttributes diffAttributes, CatchScore.Native score,
                                                      out CatchPerformanceAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Performance_ComputeMania", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Performance_ComputeMania(int beatmapContextId, ManiaDifficultyAttributes diffAttributes, ManiaScore.Native score,
                                                      out ManiaPerformanceAttributes attributes);

    #endregion
}
