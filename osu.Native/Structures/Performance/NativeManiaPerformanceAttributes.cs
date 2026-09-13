using osu.Game.Rulesets.Mania.Difficulty;

namespace osu.Native.Structures.Performance;

/// <summary>
/// Represents the Mania performance attributes (<see cref="ManiaPerformanceAttributes"/>) on the native layer.
/// </summary>
public readonly struct NativeManiaPerformanceAttributes(ManiaPerformanceAttributes attributes)
{
    public readonly double Total = attributes.Total;
    public readonly double Difficulty = attributes.Difficulty;
}
