using osu.Game.Beatmaps;
using osu.Game.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Decoder = osu.Game.Beatmaps.Formats.Decoder;

namespace osu.Native.Objects;

internal unsafe partial struct NativeBeatmap : INativeObject<FlatWorkingBeatmap>
{
  public int ObjectId { get; private init; }

  private static NativeBeatmap Create(FlatWorkingBeatmap beatmap)
  {
    int objectId = ObjectContainer<FlatWorkingBeatmap>.Add(beatmap);
    return new NativeBeatmap
    {
      ObjectId = objectId
    };
  }

  [UnmanagedCallersOnly(EntryPoint = "Beatmap_CreateFromFile", CallConvs = [typeof(CallConvCdecl)])]
  private static unsafe ErrorCode CreateFromFile(char* filePathPtr, NativeBeatmap* beatmap)
  {
    try
    {
      string filePath = new(filePathPtr);
      if (!File.Exists(filePath))
        return ErrorCode.BeatmapFileNotFound;

      FlatWorkingBeatmap workingBeatmap = new(filePath);

      *beatmap = Create(workingBeatmap);

      return ErrorCode.Success;
    }
    catch (Exception ex)
    {
      return ErrorHandler.Handle(ex);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "Beatmap_CreateFromText", CallConvs = [typeof(CallConvCdecl)])]
  private static unsafe ErrorCode CreateFromText(char* beatmapTextPtr, NativeBeatmap* beatmap)
  {
    try
    {
      string text = new(beatmapTextPtr);
      using MemoryStream ms = new(Encoding.UTF8.GetBytes(text));
      using LineBufferedReader reader = new(ms);
      FlatWorkingBeatmap workingBeatmap = new(Decoder.GetDecoder<Beatmap>(reader).Decode(reader));

      *beatmap = Create(workingBeatmap);

      return ErrorCode.Success;
    }
    catch (Exception ex)
    {
      return ErrorHandler.Handle(ex);
    }
  }
}
