using System.Runtime.InteropServices.Marshalling;
using System.Text;
using osu.Game.Beatmaps;
using osu.Game.IO;
using osu.Native.Compiler;
using Decoder = osu.Game.Beatmaps.Formats.Decoder;

namespace osu.Native.Objects;

/// <summary>
/// Represents a beatmap object (<see cref="FlatWorkingBeatmap"/>).
/// </summary>
internal unsafe partial class BeatmapObject : IOsuNativeObject<FlatWorkingBeatmap>
{
    /// <summary>
    /// The online ID of the ruleset of the beatmap.
    /// </summary>
    [OsuNativeField]
    private readonly int _rulesetId;

    /// <summary>
    /// The approach rate (AR) of the beatmap.
    /// </summary>
    [OsuNativeField]
    private readonly float _approachRate;

    /// <summary>
    /// The drain rate (HP) of the beatmap.
    /// </summary>
    [OsuNativeField]
    private readonly float _drainRate;

    /// <summary>
    /// The overall difficulty (OD) of the beatmap.
    /// </summary>
    [OsuNativeField]
    private readonly float _overallDifficulty;

    /// <summary>
    /// The circle size (CS) of the beatmap.
    /// </summary>
    [OsuNativeField]
    private readonly float _circleSize;

    /// <summary>
    /// The slider multiplier of the beatmap.
    /// </summary>
    [OsuNativeField]
    private readonly double _sliderMultiplier;

    /// <summary>
    /// The slider tick rate of the beatmap.
    /// </summary>
    [OsuNativeField]
    private readonly double _sliderTickRate;

    private static NativeBeatmap Create(FlatWorkingBeatmap beatmap)
    {
        return new()
        {
            RulesetId = beatmap.BeatmapInfo.Ruleset.OnlineID,
            Handle = ManagedObjectStore.Store(beatmap),
            ApproachRate = beatmap.BeatmapInfo.Difficulty.ApproachRate,
            DrainRate = beatmap.BeatmapInfo.Difficulty.DrainRate,
            OverallDifficulty = beatmap.BeatmapInfo.Difficulty.OverallDifficulty,
            CircleSize = beatmap.BeatmapInfo.Difficulty.CircleSize,
            SliderMultiplier = beatmap.BeatmapInfo.Difficulty.SliderMultiplier,
            SliderTickRate = beatmap.BeatmapInfo.Difficulty.SliderTickRate,
        };
    }

    /// <summary>
    /// Creates an instance of a <see cref="FlatWorkingBeatmap"/> from the beatmap file at the specified path.
    /// </summary>
    /// <param name="filePathPtr">The path to the beatmap file.</param>
    /// <param name="nativeBeatmapPtr">A pointer to write the resulting native beatmap object to.</param>
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

    /// <summary>
    /// Creates an instance of a <see cref="FlatWorkingBeatmap"/> from the specified beatmap text.
    /// </summary>
    /// <param name="beatmapTextPtr">The beatmap text.</param>
    /// <param name="nativeBeatmapPtr">A pointer to write the resulting native beatmap object to.</param>
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

    /// <summary>
    /// Writes the title of the beatmap to the provided buffer.
    /// </summary>
    /// <param name="beatmapHandle">The handle of the beatmap to retrieve the title of.</param>
    /// <param name="buffer">The buffer to write the title into.</param>
    /// <param name="bufferSize">The size of the provided buffer.</param>
    [OsuNativeFunction]
    private static ErrorCode GetTitle(ManagedObjectHandle<FlatWorkingBeatmap> beatmapHandle, byte* buffer, int* bufferSize)
      => NativeHelper.StringBuffer(beatmapHandle.Resolve().Metadata.Title, buffer, bufferSize);


    /// <summary>
    /// Writes the artist of the beatmap to the provided buffer.
    /// </summary>
    /// <param name="beatmapHandle">The handle of the beatmap to retrieve the artist of.</param>
    /// <param name="buffer">The buffer to write the artist into.</param>
    /// <param name="bufferSize">The size of the provided buffer.</param>
    [OsuNativeFunction]
    private static ErrorCode GetArtist(ManagedObjectHandle<FlatWorkingBeatmap> beatmapHandle, byte* buffer, int* bufferSize)
      => NativeHelper.StringBuffer(beatmapHandle.Resolve().Metadata.Artist, buffer, bufferSize);


    /// <summary>
    /// Writes the difficulty name of the beatmap to the provided buffer.
    /// </summary>
    /// <param name="beatmapHandle">The handle of the beatmap to retrieve difficulty name of.</param>
    /// <param name="buffer">The buffer to write the difficulty name into.</param>
    /// <param name="bufferSize">The size of the provided buffer.</param>
    [OsuNativeFunction]
    private static ErrorCode GetVersion(ManagedObjectHandle<FlatWorkingBeatmap> beatmapHandle, byte* buffer, int* bufferSize)
      => NativeHelper.StringBuffer(beatmapHandle.Resolve().BeatmapInfo.DifficultyName, buffer, bufferSize);
}
