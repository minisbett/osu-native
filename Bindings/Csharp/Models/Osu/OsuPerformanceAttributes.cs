// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.InteropServices;

namespace osu.Native.Bindings.Models.Osu;

[StructLayout(LayoutKind.Sequential)]
public readonly struct OsuPerformanceAttributes
{
    public readonly double Total;
    public readonly double Aim;
    public readonly double Speed;
    public readonly double Accuracy;
    public readonly double Flashlight;
    public readonly double EffectiveMissCount;
}