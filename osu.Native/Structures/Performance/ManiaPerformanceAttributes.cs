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
public struct ManiaPerformanceAttributes
{
    public double Total;
    public double Difficulty;

#if !IS_BINDINGS
    public static implicit operator ManiaPerformanceAttributes(Game.Rulesets.Mania.Difficulty.ManiaPerformanceAttributes attributes)
    {
        return new ManiaPerformanceAttributes
        {
            Total = attributes.Total,
            Difficulty = attributes.Difficulty
        };
    }
#endif
}