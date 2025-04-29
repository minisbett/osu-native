using System.Runtime.InteropServices;
using System.Text;

[DllImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll",
  EntryPoint = "Beatmap_CreateFromFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
static extern sbyte Beatmap_CreateFromFile(string file, out NativeBeatmap nativeBeatmap);


[DllImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll",
  EntryPoint = "Beatmap_GetTitle", CallingConvention = CallingConvention.Cdecl)]
static extern unsafe sbyte Beatmap_GetTitle(NativeBeatmap nativeBeatmap, byte* buffer, int* bufferSize);


sbyte error = Beatmap_CreateFromFile(@"C:\Users\mini\Desktop\t.osu", out NativeBeatmap nativeBeatmap);
Console.WriteLine(error);
unsafe
{
  int size = 0;
  error = Beatmap_GetTitle(nativeBeatmap, null, &size);
  byte[] buffer = new byte[size];
  fixed (byte* p = buffer)
  {
    error = Beatmap_GetTitle(nativeBeatmap, p, &size);
  }

  Console.WriteLine(Encoding.UTF8.GetString(buffer));
}


public struct NativeBeatmap
{
  public int ObjectId;
}