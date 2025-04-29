using osu.Game.Beatmaps;
using osu.Game.IO;
using osu.Native.Compiler;
using System.Text;
using Decoder = osu.Game.Beatmaps.Formats.Decoder;

namespace osu.Native.Objects;

internal unsafe partial class BeatmapObject : IOsuNativeObject<FlatWorkingBeatmap>
{
  private static NativeBeatmap Create(FlatWorkingBeatmap beatmap)
  {
    int objectId = ObjectContainer<FlatWorkingBeatmap>.Add(beatmap);
    return new NativeBeatmap
    {
      ObjectId = objectId
    };
  }

  [OsuNativeFunction]
  private static ErrorCode CreateFromFile(char* filePathPtr, NativeBeatmap* beatmap)
  {
    string filePath = new(filePathPtr);
    if (!File.Exists(filePath))
      return ErrorCode.BeatmapFileNotFound;

    FlatWorkingBeatmap workingBeatmap = new(filePath);

    *beatmap = Create(workingBeatmap);

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  private static ErrorCode CreateFromText(char* beatmapTextPtr, NativeBeatmap* beatmap)
  {
    string text = new(beatmapTextPtr);
    using MemoryStream ms = new(Encoding.UTF8.GetBytes(text));
    using LineBufferedReader reader = new(ms);
    FlatWorkingBeatmap workingBeatmap = new(Decoder.GetDecoder<Beatmap>(reader).Decode(reader));

    *beatmap = Create(workingBeatmap);

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  private static ErrorCode GetTitle(NativeBeatmap nativeBeatmap, char* buffer, int* bufferSize)
    => BufferHelper.String(nativeBeatmap.Resolve().Metadata.Title, buffer, bufferSize);

  [OsuNativeFunction]
  private static ErrorCode GetArtist(NativeBeatmap nativeBeatmap, char* buffer, int* bufferSize)
    => BufferHelper.String(nativeBeatmap.Resolve().Metadata.Artist, buffer, bufferSize);

  [OsuNativeFunction]
  private static ErrorCode GetVersion(NativeBeatmap nativeBeatmap, char* buffer, int* bufferSize)
    => BufferHelper.String(nativeBeatmap.Resolve().BeatmapInfo.DifficultyName, buffer, bufferSize);
}
