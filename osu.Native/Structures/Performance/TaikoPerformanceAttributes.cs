// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.InteropServices;

#if IS_BINDINGS
namespace osu.Native.Bindings.Structures.Performance;
#else
namespace osu.Native.Structures.Performance;
#endif

/// <summary>
/// Represents the calculated performance attributes for an osu!mania score.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct TaikoPerformanceAttributes
{
    public double Total;
    public double Difficulty;
    public double Accuracy;
    public double EffectiveMissCount;
    public double? EstimatedUnstableRate;

#if !IS_BINDINGS
    public static implicit operator TaikoPerformanceAttributes(Game.Rulesets.Taiko.Difficulty.TaikoPerformanceAttributes attributes)
    {
        return new TaikoPerformanceAttributes
        {
            Total = attributes.Total,
            Difficulty = attributes.Difficulty,
            Accuracy = attributes.Accuracy,
            EffectiveMissCount = attributes.EffectiveMissCount,
            EstimatedUnstableRate = attributes.EstimatedUnstableRate
        };
    }
#endif
}