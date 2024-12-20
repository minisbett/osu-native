﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Native.Bindings.Structures.Difficulty;
using osu.Native.Bindings.Structures.Performance;
using osu.Native.Bindings.Structures.Scores;
using System;
using System.Runtime.InteropServices;

namespace osu.Native.Bindings;

public static class OsuNative
{
    private const string LIB_PATH = @"osu.Native";

    #region Imports

    #region Beatmap

    [DllImport(LIB_PATH, EntryPoint = "Beatmap_CreateFromFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Beatmap_CreateFromFile(string filePath, out int id);

    [DllImport(LIB_PATH, EntryPoint = "Beatmap_CreateFromText", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Beatmap_CreateFromText(string text, out int id);

    [DllImport(LIB_PATH, EntryPoint = "Beatmap_Destroy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Beatmap_Destroy(int id);

    #endregion

    #region Difficulty

    [DllImport(LIB_PATH, EntryPoint = "Difficulty_CalculateOsu", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Difficulty_CalculateOsu(int beatmapId, string mods, out OsuDifficultyAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Difficulty_CalculateTaiko", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Difficulty_CalculateTaiko(int beatmapId, string mods, out TaikoDifficultyAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Difficulty_CalculateCatch", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Difficulty_CalculateCatch(int beatmapId, string mods, out CatchDifficultyAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Difficulty_CalculateMania", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Difficulty_CalculateMania(int beatmapId, string mods, out ManiaDifficultyAttributes attributes);

    #endregion

    #region Performance

    [DllImport(LIB_PATH, EntryPoint = "Performance_CalculateOsu", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Performance_CalculateOsu(int beatmapId, OsuDifficultyAttributes diffAttributes, OsuScore.Native score,
                                                    out OsuPerformanceAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Performance_CalculateTaiko", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Performance_CalculateTaiko(int beatmapId, TaikoDifficultyAttributes diffAttributes, TaikoScore.Native score,
                                                      out TaikoPerformanceAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Performance_CalculateCatch", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Performance_CalculateCatch(int beatmapId, CatchDifficultyAttributes diffAttributes, CatchScore.Native score,
                                                      out CatchPerformanceAttributes attributes);

    [DllImport(LIB_PATH, EntryPoint = "Performance_CalculateMania", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern ErrorCode Performance_CalculateMania(int beatmapId, ManiaDifficultyAttributes diffAttributes, ManiaScore.Native score,
                                                      out ManiaPerformanceAttributes attributes);

    #endregion

    #region Other

    [DllImport(LIB_PATH, EntryPoint = "_GetLastError", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static unsafe extern char* GetLastError();

    #endregion

    #endregion

    /// <summary>
    /// Utility method for wrapping an osu-native function call and converting an errous result into a .NET exception.
    /// </summary>
    /// <param name="func">The osu-native function call.</param>
    /// <exception cref="OsuNativeException">The resulting exception, if the error code is not <see cref="ErrorCode.Success"/>.</exception>
    public static unsafe void Execute(Func<ErrorCode> func)
    {
        ErrorCode code = func();
        if (code != ErrorCode.Success)
            throw new OsuNativeException(code, new string(GetLastError()));
    }
}

/// <summary>
/// Represents an error returned from osu-native, with it's error code and error message received via <see cref="OsuNative.GetLastError"/>.
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
