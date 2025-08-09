using osu.Game.Beatmaps;
using osu.Game.IO;
using osu.Native.Compiler;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using Decoder = osu.Game.Beatmaps.Formats.Decoder;

namespace osu.Native.Objects;

internal unsafe partial class BeatmapObject : IOsuNativeObject<FlatWorkingBeatmap>
{
  [OsuNativeField]
  private readonly float _approachRate;

  [OsuNativeField]
  private readonly float _drainRate;

  [OsuNativeField]
  private readonly float _overallDifficulty;

  [OsuNativeField]
  private readonly float _circleSize;

  [OsuNativeField]
  private readonly double _sliderMultiplier;

  [OsuNativeField]
  private readonly double _sliderTickRate;

  private static NativeBeatmap Create(FlatWorkingBeatmap beatmap)
  {
    ManagedObjectHandle<FlatWorkingBeatmap> handle = ManagedObjectRegistry<FlatWorkingBeatmap>.Register(beatmap);

    return new()
    {
      Handle = handle,
      ApproachRate = beatmap.BeatmapInfo.Difficulty.ApproachRate,
      DrainRate = beatmap.BeatmapInfo.Difficulty.DrainRate,
      OverallDifficulty = beatmap.BeatmapInfo.Difficulty.OverallDifficulty,
      CircleSize = beatmap.BeatmapInfo.Difficulty.CircleSize,
      SliderMultiplier = beatmap.BeatmapInfo.Difficulty.SliderMultiplier,
      SliderTickRate = beatmap.BeatmapInfo.Difficulty.SliderTickRate,
    };
  }

  [OsuNativeFunction]
  private static ErrorCode CreateFromFile(byte* filePathPtr, NativeBeatmap* nativeBeatmapPtr)
  {
    string? filePath = Utf8StringMarshaller.ConvertToManaged(filePathPtr);
    if (!File.Exists(filePath))
      return ErrorCode.BeatmapFileNotFound;

    FlatWorkingBeatmap beatmap = new(filePath);

    *nativeBeatmapPtr = Create(beatmap);

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  private static ErrorCode CreateFromText(byte* beatmapTextPtr, NativeBeatmap* nativeBeatmapPtr)
  {
    string text = Utf8StringMarshaller.ConvertToManaged(beatmapTextPtr) ?? "";

    using MemoryStream ms = new(Encoding.UTF8.GetBytes(text));
    using LineBufferedReader reader = new(ms);
    FlatWorkingBeatmap workingBeatmap = new(Decoder.GetDecoder<Beatmap>(reader).Decode(reader));

    *nativeBeatmapPtr = Create(workingBeatmap);

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  private static ErrorCode GetTitle(ManagedObjectHandle<FlatWorkingBeatmap> beatmapHandle, byte* buffer, int* bufferSize)
    => NativeHelper.StringBuffer(beatmapHandle.Resolve().Metadata.Title, buffer, bufferSize);

  [OsuNativeFunction]
  private static ErrorCode GetArtist(ManagedObjectHandle<FlatWorkingBeatmap> beatmapHandle, byte* buffer, int* bufferSize)
    => NativeHelper.StringBuffer(beatmapHandle.Resolve().Metadata.Artist, buffer, bufferSize);

  [OsuNativeFunction]
  private static ErrorCode GetVersion(ManagedObjectHandle<FlatWorkingBeatmap> beatmapHandle, byte* buffer, int* bufferSize)
    => NativeHelper.StringBuffer(beatmapHandle.Resolve().BeatmapInfo.DifficultyName, buffer, bufferSize);
}
