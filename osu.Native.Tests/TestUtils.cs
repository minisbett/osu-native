using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using osu.Native.Objects;
using osu.Native.Structures;

namespace osu.Native.Tests;

/// <summary>
/// Provides utility functionality for tests.
/// </summary>
internal static unsafe class TestUtils
{
    /// <summary>
    /// Returns the binary content of the source file with the specified name.
    /// </summary>
    /// <param name="name">The resource name.</param>
    /// <returns>The resource binary.</returns>
    public static byte[] GetResource(string name)
    {
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"osu.Native.Tests.Resources.{name.Replace("/", ".")}")!;
        using MemoryStream ms = new();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    /// <summary>
    /// Returns the handle to a newly-created <see cref="NativeBeatmap"/>, parsed from the beatmap in the embedded resources with the specified filename.
    /// </summary>
    /// <param name="filename">The filename in the embedded resources.</param>
    /// <returns>The handle to the beatmap.</returns>
    public static NativeBeatmap CreateBeatmap(string filename)
    {
        byte[] beatmap = GetResource(filename);
        NativeBeatmap nativeBeatmap;
        fixed (byte* ptr = beatmap)
            BeatmapObject.CreateFromText(ptr, &nativeBeatmap);

        return nativeBeatmap;
    }

    /// <summary>
    /// Returns the handle to a newly-created <see cref="NativeModsCollection"/> holding the mods in the specified mod string (eg. HDDT).
    /// If <see langword="null"/> is specified, a null-handle is returned.
    /// </summary>
    /// <param name="mods">The mod string.</param>
    /// <returns>The handle to the mods collection.</returns>
    public static NativeModsCollection CreateNativeModsCollection(string? mods)
    {
        if (mods is null)
            return new() { Handle = new(0) };

        NativeModsCollection nativeModsCollection;
        ModsCollectionObject.Create(&nativeModsCollection);

        foreach (string acronym in mods.Chunk(2).Select(x => new string(x)))
        {
            NativeMod nativeMod;
            ModObject.Create(Utf8StringMarshaller.ConvertToUnmanaged(acronym), &nativeMod);
            ModsCollectionObject.Add(nativeModsCollection.Handle, nativeMod.Handle);
        }

        return nativeModsCollection;
    }
}
