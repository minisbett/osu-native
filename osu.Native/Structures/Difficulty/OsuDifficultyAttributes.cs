﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.InteropServices;

#if IS_BINDINGS
namespace osu.Native.Bindings.Structures.Difficulty;
#else
using System.Linq;
using osu.Game.Rulesets.Mods;
namespace osu.Native.Structures.Difficulty;
#endif

/// <summary>
/// Represents the calculated difficulty attributes for an osu!standard beatmap.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct OsuDifficultyAttributes
{
    public double StarRating;
    public int MaxCombo;
    public double AimDifficulty;
    public double SpeedDifficulty;
    public double SpeedNoteCount;
    public double? FlashlightDifficulty;
    public double SliderFactor;
    public double AimDifficultStrainCount;
    public double SpeedDifficultStrainCount;
    public double ApproachRate;
    public double OverallDifficulty;
    public double DrainRate;
    public int HitCircleCount;
    public int SliderCount;
    public int SpinnerCount;

#if !IS_BINDINGS
    public static implicit operator OsuDifficultyAttributes(Game.Rulesets.Osu.Difficulty.OsuDifficultyAttributes attributes)
    {
        double? flashlightDifficulty = attributes.Mods.Any(x => x is ModFlashlight) ? attributes.FlashlightDifficulty : null;

        return new OsuDifficultyAttributes
        {
            StarRating = attributes.StarRating,
            MaxCombo = attributes.MaxCombo,
            AimDifficulty = attributes.AimDifficulty,
            SpeedDifficulty = attributes.SpeedDifficulty,
            SpeedNoteCount = attributes.SpeedNoteCount,
            FlashlightDifficulty = flashlightDifficulty,
            SliderFactor = attributes.SliderFactor,
            AimDifficultStrainCount = attributes.AimDifficultStrainCount,
            SpeedDifficultStrainCount = attributes.SpeedDifficultStrainCount,
            ApproachRate = attributes.ApproachRate,
            OverallDifficulty = attributes.OverallDifficulty,
            DrainRate = attributes.DrainRate,
            HitCircleCount = attributes.HitCircleCount,
            SliderCount = attributes.SliderCount,
            SpinnerCount = attributes.SpinnerCount
        };
    }

    public static implicit operator Game.Rulesets.Osu.Difficulty.OsuDifficultyAttributes(OsuDifficultyAttributes attributes)
    {
        return new Game.Rulesets.Osu.Difficulty.OsuDifficultyAttributes
        {
            StarRating = attributes.StarRating,
            MaxCombo = attributes.MaxCombo,
            AimDifficulty = attributes.AimDifficulty,
            SpeedDifficulty = attributes.SpeedDifficulty,
            SpeedNoteCount = attributes.SpeedNoteCount,
            FlashlightDifficulty = attributes.FlashlightDifficulty ?? 0,
            SliderFactor = attributes.SliderFactor,
            AimDifficultStrainCount = attributes.AimDifficultStrainCount,
            SpeedDifficultStrainCount = attributes.SpeedDifficultStrainCount,
            ApproachRate = attributes.ApproachRate,
            OverallDifficulty = attributes.OverallDifficulty,
            DrainRate = attributes.DrainRate,
            HitCircleCount = attributes.HitCircleCount,
            SliderCount = attributes.SliderCount,
            SpinnerCount = attributes.SpinnerCount
        };
    }
#endif
}
