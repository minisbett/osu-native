// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Native.Bindings.Structures.Difficulty;
using osu.Native.Bindings.Structures.Performance;
using osu.Native.Bindings.Structures.Scores;
using System;
using System.Runtime.InteropServices;

namespace osu.Native.Bindings;

public static class OsuNative
{
    private const string LIB_PATH = @"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\osu.Native\osu.Native.dll";

    #region Beatmap

    [DllImport(LIB_PATH, EntryPoint = "Beatmap_CreateFromFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Beatmap_CreateFromFile(string filePath, out int id);

    [DllImport(LIB_PATH, EntryPoint = "Beatmap_CreateFromText", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Beatmap_CreateFromText(string text, out int id);

    [DllImport(LIB_PATH, EntryPoint = "Beatmap_Destroy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Beatmap_Destroy(int id);

    #endregion

    #region Difficulty

    [DllImport(LIB_PATH, EntryPoint = "Difficulty_ComputeOsu", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Difficulty_ComputeOsu(int beatmapId, string mods, out OsuDifficultyAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Difficulty_ComputeTaiko", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Difficulty_ComputeTaiko(int beatmapId, string mods, out TaikoDifficultyAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Difficulty_ComputeCatch", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Difficulty_ComputeCatch(int beatmapId, string mods, out CatchDifficultyAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Difficulty_ComputeMania", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Difficulty_ComputeMania(int beatmapId, string mods, out ManiaDifficultyAttributes attributes);

    #endregion

    #region Performance

    [DllImport(LIB_PATH, EntryPoint = "Performance_ComputeOsu", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Performance_ComputeOsu(int beatmapId, OsuDifficultyAttributes diffAttributes, OsuScore.Native score,
                                                    out OsuPerformanceAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Performance_ComputeTaiko", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Performance_ComputeTaiko(int beatmapId, TaikoDifficultyAttributes diffAttributes, TaikoScore.Native score,
                                                      out TaikoPerformanceAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Performance_ComputeCatch", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Performance_ComputeCatch(int beatmapId, CatchDifficultyAttributes diffAttributes, CatchScore.Native score,
                                                      out CatchPerformanceAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Performance_ComputeMania", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Performance_ComputeMania(int beatmapId, ManiaDifficultyAttributes diffAttributes, ManiaScore.Native score,
                                                      out ManiaPerformanceAttributes attributes);

    #endregion

    #region Other

    [DllImport(LIB_PATH, EntryPoint = "_GetLastError", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern string? GetLastError();

    #endregion

    /// <summary>
    /// Utility method for wrapping an osu-native function call and converting an errous result into a .NET exception.
    /// </summary>
    /// <param name="func">The osu-native function call.</param>
    /// <exception cref="OsuNativeException">The resulting exception, if the error code is not <see cref="ErrorCode.Success"/>.</exception>
    public static void Execute(Func<ErrorCode> func)
    {
        ErrorCode code = func();
        if (code != ErrorCode.Success)
            throw new OsuNativeException(code, GetLastError() ?? throw new NullReferenceException("Could not retrieve error message: GetLastError() is null."));
    }
}

/// <summary>
/// Represents an error returned from osu-native, with it's error code and error message received via <see cref="OsuNative.GetLastErrorMessage"/>.
/// </summary>
/// <param name="code">The error code.</param>
/// <param name="msg">The error message.</param>
public class OsuNativeException(ErrorCode code, string msg) : Exception($"{code}: {msg}")
{
    /// <summary>
    /// The error code.
    /// </summary>
    public ErrorCode Code { get; } = code;
}
