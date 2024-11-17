// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System;
using System.IO;
using osu.Game.Beatmaps;
using System.Text;
using osu.Game.IO;
using Decoder = osu.Game.Beatmaps.Formats.Decoder;

namespace osu.Native.EntryPoints;

public static unsafe class BeatmapEntryPoints
{
    [UnmanagedCallersOnly(EntryPoint = "Beatmap_ParseFile", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ParseFile(char* filePathPtr, int* id)
    {
        string filePath = Marshal.PtrToStringUTF8((IntPtr)filePathPtr) ?? string.Empty;

        if (filePath == null)
            return Logger.Error(ErrorCode.FileNotFound, "Beatmap file not found.");

        try
        {
            FlatWorkingBeatmap beatmap = new(filePath);
            *id = Interlocked.Increment(ref Context.NextBeatmapContextId);
            Context.Beatmaps[*id] = beatmap;

            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            return Logger.Error(ErrorCode.Failure, ex.Message);
        }


    }
    [UnmanagedCallersOnly(EntryPoint = "Beatmap_ParseText", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode ParseText(char* textPtr, int* id)
    {
        string text = Marshal.PtrToStringUTF8((IntPtr)textPtr) ?? string.Empty;

        try
        {
            using MemoryStream ms = new(Encoding.UTF8.GetBytes(text));
            using LineBufferedReader reader = new(ms);
            FlatWorkingBeatmap beatmap = new(Decoder.GetDecoder<Beatmap>(reader).Decode(reader));

            *id = Interlocked.Increment(ref Context.NextBeatmapContextId);
            Context.Beatmaps[*id] = beatmap;

            return ErrorCode.Success;
        }
        catch (Exception ex)
        {
            return Logger.Error(ErrorCode.Failure, ex.Message);
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "Beatmap_Destroy", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode Destroy(int id)
    {
        Context.Beatmaps.Remove(id);

        return ErrorCode.Success;
    }
}
