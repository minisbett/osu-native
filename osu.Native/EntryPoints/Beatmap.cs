// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;
using System.IO;
using osu.Game.Beatmaps;
using System.Text;
using osu.Game.IO;
using Decoder = osu.Game.Beatmaps.Formats.Decoder;
using osu.Native.Helpers;

namespace osu.Native.EntryPoints;

public static unsafe class BeatmapEntryPoints
{
    /// <summary>
    /// Creates a <see cref="FlatWorkingBeatmap"/> from the specified path and returns the associated context ID.
    /// </summary>
    /// <param name="filePathPtr">The path to the .osu file.</param>
    /// <param name="id">The context ID associated with the created beatmap object.</param>
    [UnmanagedCallersOnly(EntryPoint = "Beatmap_CreateFromFile", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode CreateFromFile(char* filePathPtr, int* id)
    {
        string filePath = Marshal.PtrToStringUTF8((IntPtr)filePathPtr) ?? string.Empty;

        try
        {
            *id = Contexts.Beatmaps.Create(new FlatWorkingBeatmap(filePath)); // Throws FileNotFoundException if filePath cannot be found
            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            return Logger.Error(ex);
        }
    }

    /// <summary>
    /// Creates a <see cref="FlatWorkingBeatmap"/> from the .osu file content and returns the associated context ID.
    /// </summary>
    /// <param name="textPtr">The .osu file content.</param>
    /// <param name="id">The context ID associated with the created beatmap object.</param>
    [UnmanagedCallersOnly(EntryPoint = "Beatmap_CreateFromText", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode CreateFromText(char* textPtr, int* id)
    {
        string text = Marshal.PtrToStringUTF8((IntPtr)textPtr) ?? string.Empty;

        try
        {
            using MemoryStream ms = new(Encoding.UTF8.GetBytes(text));
            using LineBufferedReader reader = new(ms);
            FlatWorkingBeatmap beatmap = new(Decoder.GetDecoder<Beatmap>(reader).Decode(reader));

            *id = Contexts.Beatmaps.Create(beatmap);
            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            return Logger.Error(ex);
        }
    }

    /// <summary>
    /// Destroys the <see cref="FlatWorkingBeatmap"/> associated with the specified context ID.
    /// </summary>
    /// <param name="id">The context ID associated with the beatmap object.</param>
    [UnmanagedCallersOnly(EntryPoint = "Beatmap_Destroy", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode Destroy(int id)
    {
        try
        {
            Contexts.Beatmaps.Destroy(id);
            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            return Logger.Error(ex);
        }
    }
}
