using osu.Game.Rulesets.Mania.Difficulty;

namespace osu.Native.Structures.Performance;

/// <summary>
/// Represents the Mania performance attributes (<see cref="ManiaPerformanceAttributes"/>) on the native layer.
/// </summary>
internal struct NativeManiaPerformanceAttributes(ManiaPerformanceAttributes attributes)
{
    public double Total = attributes.Total;
    public double Difficulty = attributes.Difficulty;
}
