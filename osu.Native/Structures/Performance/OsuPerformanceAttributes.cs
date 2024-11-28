// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.InteropServices;

#if IS_BINDINGS
namespace osu.Native.Bindings.Structures.Performance;
#else
namespace osu.Native.Structures.Performance;
#endif

[StructLayout(LayoutKind.Sequential)]
public struct OsuPerformanceAttributes
{
    public double Total;
    public double Aim;
    public double Speed;
    public double Accuracy;
    public double Flashlight;
    public double EffectiveMissCount;

#if !IS_BINDINGS
    public static implicit operator OsuPerformanceAttributes(Game.Rulesets.Osu.Difficulty.OsuPerformanceAttributes attributes)
    {
        return new OsuPerformanceAttributes
        {
            Total = attributes.Total,
            Aim = attributes.Aim,
            Speed = attributes.Speed,
            Accuracy = attributes.Accuracy,
            Flashlight = attributes.Flashlight,
            EffectiveMissCount = attributes.EffectiveMissCount
        };
    }
#endif
}