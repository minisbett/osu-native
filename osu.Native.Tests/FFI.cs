using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using osu.Native.Structures;

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
    public static partial ErrorCode Mod_Create(string acronym, out NativeMod nativeMod);

    [LibraryImport("osu.Native-aot.dll", StringMarshalling = StringMarshalling.Utf8)]
    public static partial ErrorCode Mod_SetSettingBool(int modHandleId, string key, [MarshalAs(UnmanagedType.I1)] bool value);

    [LibraryImport("osu.Native-aot.dll", StringMarshalling = StringMarshalling.Utf8)]
    public static partial ErrorCode Mod_SetSettingInteger(int modHandleId, string key, int value);

    [LibraryImport("osu.Native-aot.dll", StringMarshalling = StringMarshalling.Utf8)]
    public static partial ErrorCode Mod_SetSettingFloat(int modHandleId, string key, float value);

    [LibraryImport("osu.Native-aot.dll", StringMarshalling = StringMarshalling.Utf8)]
    public static partial ErrorCode Mod_Debug(int modHandleId);

    #endregion

    #region ModsCollection

    [LibraryImport("osu.Native-aot.dll")]
    public static partial ErrorCode ModsCollection_Create(out NativeModsCollection nativeModsCollection);

    [LibraryImport("osu.Native-aot.dll")]
    public static partial ErrorCode ModsCollection_Add(int modsCollectionHandleId, int modHandleId);

    [LibraryImport("osu.Native-aot.dll")]
    public static partial ErrorCode ModsCollection_Remove(int modsCollectionHandleId, int modHandleId);

    #endregion

    #region Beatmap

    [LibraryImport("osu.Native-aot.dll", StringMarshalling = StringMarshalling.Utf8)]
    public static partial ErrorCode Beatmap_CreateFromFile(string file, out NativeBeatmap nativeBeatmap);

    [LibraryImport("osu.Native-aot.dll")]
    public static partial ErrorCode Beatmap_GetTitle(int beatmapHandleId, byte* buffer, int* bufferSize);

    [LibraryImport("osu.Native-aot.dll")]
    public static partial ErrorCode Beatmap_GetArtist(int beatmapHandleId, byte* buffer, int* bufferSize);

    [LibraryImport("osu.Native-aot.dll")]
    public static partial ErrorCode Beatmap_GetVersion(int beatmapHandleId, byte* buffer, int* bufferSize);

    #endregion

    #region Ruleset

    [LibraryImport("osu.Native-aot.dll")]
    public static partial ErrorCode Ruleset_CreateFromId(int rulesetHandleId, out NativeRuleset ruleset);

    #endregion
}