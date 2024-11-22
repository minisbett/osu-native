// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.InteropServices;

namespace osu.Native.Bindings.Models.Osu;

[StructLayout(LayoutKind.Sequential)]
public readonly struct OsuDifficultyAttributes
{
    public readonly double StarRating;
    public readonly int MaxCombo;
    public readonly double AimDifficulty;
    public readonly double SpeedDifficulty;
    public readonly double SpeedNoteCount;
    public readonly double? FlashlightDifficulty;
    public readonly double SliderFactor;
    public readonly double AimDifficultStrainCount;
    public readonly double SpeedDifficultStrainCount;
    public readonly double ApproachRate;
    public readonly double OverallDifficulty;
    public readonly double DrainRate;
    public readonly int HitCircleCount;
    public readonly int SliderCount;
    public readonly int SpinnerCount;
}
