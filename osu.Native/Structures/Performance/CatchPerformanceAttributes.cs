// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.InteropServices;

#if IS_BINDINGS
namespace osu.Native.Bindings.Structures.Performance;
#else
namespace osu.Native.Structures.Performance;
#endif

[StructLayout(LayoutKind.Sequential)]
public struct CatchPerformanceAttributes
{
    public double Total;

#if !IS_BINDINGS
    public static implicit operator CatchPerformanceAttributes(Game.Rulesets.Catch.Difficulty.CatchPerformanceAttributes attributes)
    {
        return new CatchPerformanceAttributes
        {
            Total = attributes.Total
        };
    }
#endif
}