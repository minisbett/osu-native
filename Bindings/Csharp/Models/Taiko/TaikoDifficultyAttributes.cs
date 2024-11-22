// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.InteropServices;

namespace osu.Native.Bindings.Models.Taiko;

[StructLayout(LayoutKind.Sequential)]
public readonly struct TaikoDifficultyAttributes
{
    public readonly double StarRating;
    public readonly int MaxCombo;
    public readonly double StaminaDifficulty;
    public readonly double MonoStaminaFactor;
    public readonly double RhythmDifficulty;
    public readonly double ColourDifficulty;
    public readonly double PeakDifficulty;
    public readonly double GreatHitWindow;
    public readonly double OkHitWindow;
}
