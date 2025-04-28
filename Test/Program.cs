using System.Runtime.InteropServices;

[DllImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll",
  EntryPoint = "Beatmap_CreateFromFile", CallingConvention = CallingConvention.Cdecl)]
static extern byte Beatmap_CreateFromFile(string file, out NativeBeatmap nativeBeatmap);


[DllImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll",
  EntryPoint = "Beatmap_GetTitle", CallingConvention = CallingConvention.Cdecl)]
static extern unsafe byte Beatmap_GetTitle(NativeBeatmap nativeBeatmap, char* buffer, int* bufferSize);


byte e = Beatmap_CreateFromFile(@"C:\Users\mini\Desktop\t.osu", out NativeBeatmap nativeBeatmap);

unsafe
{
  int size = 0;
  Beatmap_GetTitle(nativeBeatmap, null, &size);
  char[] buffer = new char[size];
  fixed (char* p = buffer)
  {
    Beatmap_GetTitle(nativeBeatmap, p, &size);
  }

  Console.WriteLine(new string(buffer));
}


public struct NativeBeatmap
{
  public int ObjectId;
}