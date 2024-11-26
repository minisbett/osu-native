// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.InteropServices;

namespace osu.Native.Bindings.Structures.Performance;

[StructLayout(LayoutKind.Sequential)]
public readonly struct CatchPerformanceAttributes
{
    public readonly double Total;
}