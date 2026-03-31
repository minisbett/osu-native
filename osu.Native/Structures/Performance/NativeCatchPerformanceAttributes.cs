using osu.Game.Rulesets.Catch.Difficulty;

namespace osu.Native.Structures.Performance;

/// <summary>
/// Represents the Catch performance attributes (<see cref="CatchPerformanceAttributes"/>) on the native layer.
/// </summary>
public readonly struct NativeCatchPerformanceAttributes(CatchPerformanceAttributes attributes)
{
    public readonly double Total = attributes.Total;
}
