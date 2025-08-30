using osu.Game.Rulesets.Catch.Difficulty;

namespace osu.Native.Structures.Performance;

/// <summary>
/// Represents the Catch performance attributes (<see cref="CatchPerformanceAttributes"/>) on the native layer.
/// </summary>
public struct NativeCatchPerformanceAttributes(CatchPerformanceAttributes attributes)
{
    public double Total = attributes.Total;
}
