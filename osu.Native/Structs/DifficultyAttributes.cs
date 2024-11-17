// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.InteropServices;

namespace osu.Native.Structs;

[StructLayout(LayoutKind.Sequential)]
public struct NativeOsuDifficultyAttributes
{
    public double StarRating;
    public int MaxCombo;
    public double AimDifficulty;
    public double SpeedDifficulty;
    public double SpeedNoteCount;
    public double FlashlightDifficulty;
    public double SliderFactor;
    public double AimDifficultStrainCount;
    public double SpeedDifficultStrainCount;
    public double ApproachRate;
    public double OverallDifficulty;
    public double DrainRate;
    public int HitCircleCount;
    public int SliderCount;
    public int SpinnerCount;
}

[StructLayout(LayoutKind.Sequential)]
public struct NativeTaikoDifficultyAttributes
{
    public double StarRating;
    public int MaxCombo;
    public double StaminaDifficulty;
    public double RhythmDifficulty;
    public double ColourDifficulty;
    public double PeakDifficulty;
    public double GreatHitWindow;
    public double OkHitWindow;
}

[StructLayout(LayoutKind.Sequential)]
public struct NativeCatchDifficultyAttributes
{
    public double StarRating;
    public int MaxCombo;
    public double ApproachRate;
}


[StructLayout(LayoutKind.Sequential)]
public struct NativeManiaDifficultyAttributes
{
    public double StarRating;
    public int MaxCombo;
    public double GreatHitWindow;
}