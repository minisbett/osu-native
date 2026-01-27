using osu.Game.Beatmaps;

namespace osu.Native.Structures;

/// <summary>
/// Represents a <see cref="FlatWorkingBeatmap"/> natively.
/// </summary>
public struct NativeBeatmap
{
    public BeatmapHandle Handle;
    public int RulesetId;
    public int BeatmapId;
    public float ApproachRate;
    public float DrainRate;
    public float OverallDifficulty;
    public float CircleSize;
    public double SliderMultiplier;
    public double SliderTickRate;
}
