using osu.Game.Rulesets.Taiko.Difficulty;

namespace osu.Native.Structures.Performance;

/// <summary>
/// Represents the Taiko performance attributes (<see cref="TaikoPerformanceAttributes"/>) on the native layer.
/// </summary>
public readonly struct NativeTaikoPerformanceAttributes(TaikoPerformanceAttributes attributes)
{
    public readonly double Total = attributes.Total;
    public readonly double Difficulty = attributes.Difficulty;
    public readonly double Accuracy = attributes.Accuracy;
    public readonly double? EstimatedUnstableRate = attributes.EstimatedUnstableRate;
}
