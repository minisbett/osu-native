// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using osu.Game.Beatmaps;
using osu.Game.IO;
using osu.Native.Objects;
using Decoder = osu.Game.Beatmaps.Formats.Decoder;
using osu.Native.Helpers;

namespace osu.Native.EntryPoints;

public static unsafe class Beatmap
{
    /// <summary>
    /// Creates a <see cref="FlatWorkingBeatmap"/> from the specified path and returns the native object referencing it.
    /// </summary>
    /// <param name="filePathPtr">The path to the .osu file.</param>
    /// <param name="beatmap">The native object referencing the beatmap object.</param>
    [UnmanagedCallersOnly(EntryPoint = "Beatmap_CreateFromFile", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode CreateFromFile(char* filePathPtr, NativeObject<FlatWorkingBeatmap>* beatmap)
    {
        try
        {
            string filePath = new(filePathPtr);
            FlatWorkingBeatmap workingBeatmap = new(filePath); // Throws FileNotFoundException if filePath cannot be found
            *beatmap = NativeObject<FlatWorkingBeatmap>.Create(workingBeatmap);
            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            ErrorHandler.SetLastError(ex.ToString());
            return ErrorHelper.FromException(ex);
        }
    }

    /// <summary>
    /// Creates a <see cref="FlatWorkingBeatmap"/> from the .osu file content and returns the native object referencing it.
    /// </summary>
    /// <param name="textPtr">The .osu file content.</param>
    /// <param name="beatmap">The native object referencing the beatmap object.</param>
    [UnmanagedCallersOnly(EntryPoint = "Beatmap_CreateFromText", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode CreateFromText(char* textPtr, NativeObject<FlatWorkingBeatmap>* beatmap)
    {
        try
        {
            string text = new(textPtr);
            using MemoryStream ms = new(Encoding.UTF8.GetBytes(text));
            using LineBufferedReader reader = new(ms);
            FlatWorkingBeatmap workingBeatmap = new(Decoder.GetDecoder<Game.Beatmaps.Beatmap>(reader).Decode(reader));

            *beatmap = NativeObject<FlatWorkingBeatmap>.Create(workingBeatmap);
            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            ErrorHandler.SetLastError(ex.ToString());
            return ErrorHelper.FromException(ex);
        }
    }

    /// <summary>
    /// Destroys the <see cref="FlatWorkingBeatmap"/> referenced by the specified native object.
    /// </summary>
    /// <param name="beatmap">The native object referencing the beatmap object.</param>
    [UnmanagedCallersOnly(EntryPoint = "Beatmap_Destroy", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode Destroy(NativeObject<FlatWorkingBeatmap> beatmap)
    {
        try
        {
            beatmap.Destroy();
            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            ErrorHandler.SetLastError(ex.ToString());
            return ErrorHelper.FromException(ex);
        }
    }
}
