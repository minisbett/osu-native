using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;


unsafe
{
  Native.Beatmap_CreateFromFile(@"C:\Users\mini\Desktop\a.osu", out NativeBeatmap nativeBeatmap);
  Console.WriteLine($"ID: {nativeBeatmap.ObjectId}");
  Console.WriteLine($"AR: {nativeBeatmap.ApproachRate}");
  Console.WriteLine($"HP: {nativeBeatmap.DrainRate}");
  Console.WriteLine($"OD: {nativeBeatmap.OverallDifficulty}");
  Console.WriteLine($"CS: {nativeBeatmap.CircleSize}");
  Console.WriteLine($"SM: {nativeBeatmap.SliderMultiplier}");
  Console.WriteLine($"ST: {nativeBeatmap.SliderTickRate}");
  Console.WriteLine($"AR: {nativeBeatmap.ApproachRate}");

  nativeBeatmap.ObjectId = 2;

  int size;
  int error = Native.Beatmap_GetTitle(nativeBeatmap, null, &size);
  Console.WriteLine($"Error code: {error}");
  Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

  nativeBeatmap.ObjectId = 1;

  error = Native.Beatmap_GetTitle(nativeBeatmap, null, &size);
  Console.WriteLine($"Error code: {error}");
  Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

  byte[] buffer = new byte[size];
  fixed (byte* p = buffer)
    Native.Beatmap_GetTitle(nativeBeatmap, p, &size);

  Console.WriteLine($"Title: {Encoding.UTF8.GetString(buffer)}");

  error = Native.Mod_Create("Test Acronym", out NativeMod nativeMod);
  Console.WriteLine(nativeMod.ObjectId);
  Console.WriteLine($"Error code: {error}");
  Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");
  error = Native.Mod_SetSetting(nativeMod, "Test Setting", 7.27);
  Console.WriteLine($"Error code: {error}");
  Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

  error = Native.ModsCollection_Create(out NativeModsCollection nativeModsCollection);
  Console.WriteLine($"Error code: {error}");
  Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

  error = Native.ModsCollection_Add(nativeModsCollection, nativeMod);
  Console.WriteLine($"Error code: {error}");
  Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

  Native.Mod_Debug(nativeMod);
  Native.ModsCollection_Debug(nativeModsCollection);

  error = Native.ModsCollection_Remove(nativeModsCollection, nativeMod);
  Console.WriteLine($"Error code: {error}");
  Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

  Native.ModsCollection_Debug(nativeModsCollection);
}

public struct NativeBeatmap
{
  public int ObjectId;
  public float ApproachRate;
  public float DrainRate;
  public float OverallDifficulty;
  public float CircleSize;
  public double SliderMultiplier;
  public double SliderTickRate;
}

public struct NativeMod
{
  public int ObjectId;
}

public struct NativeModsCollection
{
  public int ObjectId;
}

public static unsafe partial class Native
{
  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll")]
  [return: MarshalUsing(typeof(Utf8NoFreeStringMarshaller))]
  public static partial string ErrorHandler_GetLastMessage();

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll",
    StringMarshalling = StringMarshalling.Utf8)]
  public static partial sbyte Beatmap_CreateFromFile(string file, out NativeBeatmap nativeBeatmap);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll")]
  public static partial sbyte Beatmap_GetTitle(NativeBeatmap nativeBeatmap, byte* buffer, int* bufferSize);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll")]
  public static partial sbyte Beatmap_GetArtist(NativeBeatmap nativeBeatmap, byte* buffer, int* bufferSize);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll")]
  public static partial sbyte Beatmap_GetVersion(NativeBeatmap nativeBeatmap, byte* buffer, int* bufferSize);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll",
    StringMarshalling = StringMarshalling.Utf8)]
  public static partial sbyte Mod_Create(string acronym, out NativeMod nativeMod);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll",
    StringMarshalling = StringMarshalling.Utf8)]
  public static partial sbyte Mod_SetSetting(NativeMod nativeMod, string key, double value);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll")]
  public static partial sbyte ModsCollection_Create(out NativeModsCollection nativeModsCollection);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll")]
  public static partial sbyte ModsCollection_Add(NativeModsCollection nativeMod, NativeMod mod);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll")]
  public static partial sbyte ModsCollection_Remove(NativeModsCollection nativeMod, NativeMod mod);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll")]
  public static partial sbyte Mod_Debug(NativeMod mod);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll")]
  public static partial sbyte ModsCollection_Debug(NativeModsCollection nativeMod);
}

[CustomMarshaller(typeof(string), MarshalMode.Default, typeof(Utf8NoFreeStringMarshaller))]
public static unsafe class Utf8NoFreeStringMarshaller
{
  public static string? ConvertToManaged(byte* unmanaged) => Utf8StringMarshaller.ConvertToManaged(unmanaged);
}