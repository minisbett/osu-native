using System.Runtime.InteropServices;
using System.Text;


sbyte error = Native.Beatmap_CreateFromFile(@"C:\Users\mini\Desktop\t.osu", out NativeBeatmap nativeBeatmap);
Console.WriteLine(error);
unsafe
{
  int size = 1024;
  byte[] buffer = new byte[size];
  fixed (byte* p = buffer)
  {
    error = Native.Beatmap_GetTitle(nativeBeatmap, p, &size);
  }

  Console.WriteLine(Encoding.UTF8.GetString(buffer));
}

public struct NativeBeatmap
{
  public int ObjectId;
}

public static unsafe partial class Native
{
  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll",
    StringMarshalling = StringMarshalling.Utf8)]
  public static partial sbyte Beatmap_CreateFromFile(string file, out NativeBeatmap nativeBeatmap);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll")]
  public static partial sbyte Beatmap_GetTitle(NativeBeatmap nativeBeatmap, byte* buffer, int* bufferSize);
}