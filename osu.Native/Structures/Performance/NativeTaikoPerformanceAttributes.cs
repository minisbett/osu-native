using osu.Game.Rulesets.Taiko.Difficulty;

namespace osu.Native.Structures.Performance;

/// <summary>
/// Represents the Taiko performance attributes (<see cref="TaikoPerformanceAttributes"/>) on the native layer.
/// </summary>
public struct NativeTaikoPerformanceAttributes(TaikoPerformanceAttributes attributes)
{
    public double Total = attributes.Total;
    public double Difficulty = attributes.Difficulty;
    public double Accuracy = attributes.Accuracy;
    public double? EstimatedUnstableRate = attributes.EstimatedUnstableRate;
}
