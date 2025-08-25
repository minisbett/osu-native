using osu.Game.Rulesets.Osu.Difficulty;

namespace osu.Native.Structures.Performance;

/// <summary>
/// Represents the Osu performance attributes (<see cref="OsuPerformanceAttributes"/>) on the native layer.
/// </summary>
internal struct NativeOsuPerformanceAttributes(OsuPerformanceAttributes attributes)
{
    public double Total = attributes.Total;
    public double Aim = attributes.Aim;
    public double Speed = attributes.Speed;
    public double Accuracy = attributes.Accuracy;
    public double Flashlight = attributes.Flashlight;
    public double EffectiveMissCount = attributes.EffectiveMissCount;
    public double? SpeedDeviation = attributes.SpeedDeviation;
}
