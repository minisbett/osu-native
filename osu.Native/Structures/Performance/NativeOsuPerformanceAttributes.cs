using osu.Game.Rulesets.Osu.Difficulty;

namespace osu.Native.Structures.Performance;

/// <summary>
/// Represents the Osu performance attributes (<see cref="OsuPerformanceAttributes"/>) on the native layer.
/// </summary>
public readonly struct NativeOsuPerformanceAttributes(OsuPerformanceAttributes attributes)
{
    public readonly double Total = attributes.Total;
    public readonly double Aim = attributes.Aim;
    public readonly double Speed = attributes.Speed;
    public readonly double Accuracy = attributes.Accuracy;
    public readonly double Flashlight = attributes.Flashlight;
    public readonly double EffectiveMissCount = attributes.EffectiveMissCount;
    public readonly double? SpeedDeviation = attributes.SpeedDeviation;
    public readonly double ComboBasedEstimatedMissCount = attributes.ComboBasedEstimatedMissCount;
    public readonly double? ScoreBasedEstimatedMissCount = attributes.ScoreBasedEstimatedMissCount;
    public readonly double AimEstimatedSliderBreaks = attributes.AimEstimatedSliderBreaks;
    public readonly double SpeedEstimatedSliderBreaks = attributes.SpeedEstimatedSliderBreaks;
}
