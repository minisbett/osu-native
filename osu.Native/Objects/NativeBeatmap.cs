// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.IO;
using osu.Native.Helpers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System;
using Decoder = osu.Game.Beatmaps.Formats.Decoder;

namespace osu.Native.Objects;

/// <summary>
/// The native representation of a <see cref="FlatWorkingBeatmap"/>.
/// </summary>
public readonly struct NativeBeatmap : INativeObject<FlatWorkingBeatmap>
{
    /// <inheritdoc/>
    public int Id { get; private init; }

    /// <summary>
    /// Creates a native beatmap referring to the managed object based on the specified <see cref="FlatWorkingBeatmap"/>.
    /// </summary>
    /// <param name="obj">The beatmap.</param>
    /// <returns>The native object.</returns>
    private static NativeBeatmap Create(FlatWorkingBeatmap obj)
    {
        int id = ObjectContainer<FlatWorkingBeatmap>.Add(obj);
        return new NativeBeatmap { Id = id };
    }


    /// <summary>
    /// Creates a <see cref="FlatWorkingBeatmap"/> from the specified path and returns the native object referencing it.
    /// </summary>
    /// <param name="filePathPtr">The path to the .osu file.</param>
    /// <param name="beatmap">The native object referencing the beatmap object.</param>
    [UnmanagedCallersOnly(EntryPoint = "Beatmap_CreateFromFile", CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe ErrorCode CreateFromFile(char* filePathPtr, NativeBeatmap* beatmap)
    {
        try
        {
            string filePath = new(filePathPtr);
            FlatWorkingBeatmap workingBeatmap = new(filePath); // Throws FileNotFoundException if filePath cannot be found
            *beatmap = Create(workingBeatmap);
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
    private static unsafe ErrorCode CreateFromText(char* textPtr, NativeBeatmap* beatmap)
    {
        try
        {
            string text = new(textPtr);
            using MemoryStream ms = new(Encoding.UTF8.GetBytes(text));
            using LineBufferedReader reader = new(ms);
            FlatWorkingBeatmap workingBeatmap = new(Decoder.GetDecoder<Beatmap>(reader).Decode(reader));
            *beatmap = Create(workingBeatmap);
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
    private static ErrorCode Destroy(NativeBeatmap beatmap)
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

