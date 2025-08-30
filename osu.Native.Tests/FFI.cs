using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using osu.Native.Objects;

namespace osu.Native.Tests;

// The osu.Native-aot.dll file is included via a CopyToOutputDirectory in the project file.

/// <summary>
/// Contains the P/Invoke signatures for interacting with the osu-native.
/// </summary>
public static unsafe partial class FFI
{
    [LibraryImport("osu.Native-aot.dll")]
    [return: MarshalUsing(typeof(Utf8StringMarshaller))]
    public static partial string ErrorHandler_GetLastMessage();

    #region Mods

    [LibraryImport("osu.Native-aot.dll", StringMarshalling = StringMarshalling.Utf8)]
    public static partial ErrorCode Mod_Create(string acronym, out int modHandle);

    [LibraryImport("osu.Native-aot.dll", StringMarshalling = StringMarshalling.Utf8)]
    public static partial ErrorCode Mod_SetSetting(int modHandle, string key, double value);

    [LibraryImport("osu.Native-aot.dll", StringMarshalling = StringMarshalling.Utf8)]
    public static partial ErrorCode Mod_Debug(int modHandle);

    #endregion

    #region ModsCollection

    [LibraryImport("osu.Native-aot.dll")]
    public static partial ErrorCode ModsCollection_Create(out int modsHandle);

    [LibraryImport("osu.Native-aot.dll")]
    public static partial ErrorCode ModsCollection_Add(int modsHandle, int modHandle);

    [LibraryImport("osu.Native-aot.dll")]
    public static partial ErrorCode ModsCollection_Remove(int modsHandle, int modHandle);

    #endregion

    #region Beatmap

    [LibraryImport("osu.Native-aot.dll", StringMarshalling = StringMarshalling.Utf8)]
    public static partial ErrorCode Beatmap_CreateFromFile(string file, out NativeBeatmap nativeBeatmap);

    [LibraryImport("osu.Native-aot.dll")]
    public static partial ErrorCode Beatmap_GetTitle(int beatmapHandle, byte* buffer, int* bufferSize);

    [LibraryImport("osu.Native-aot.dll")]
    public static partial ErrorCode Beatmap_GetArtist(int beatmapHandle, byte* buffer, int* bufferSize);

    [LibraryImport("osu.Native-aot.dll")]
    public static partial ErrorCode Beatmap_GetVersion(int beatmapHandle, byte* buffer, int* bufferSize);

    #endregion

    #region Ruleset

    [LibraryImport("osu.Native-aot.dll")]
    public static partial ErrorCode Ruleset_CreateFromId(int rulesetId, out NativeRuleset ruleset);

    #endregion
}