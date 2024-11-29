// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.InteropServices;

#if IS_BINDINGS
namespace osu.Native.Bindings.Structures.Difficulty;
#else
namespace osu.Native.Structures.Difficulty;
#endif

/// <summary>
/// Represents the calculated difficulty attributes for an osu!taiko beatmap.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct TaikoDifficultyAttributes
{
    public double StarRating;
    public int MaxCombo;
    public double StaminaDifficulty;
    public double MonoStaminaFactor;
    public double RhythmDifficulty;
    public double ColourDifficulty;
    public double PeakDifficulty;
    public double GreatHitWindow;
    public double OkHitWindow;

#if !IS_BINDINGS
    public static implicit operator TaikoDifficultyAttributes(Game.Rulesets.Taiko.Difficulty.TaikoDifficultyAttributes attributes)
    {
        return new TaikoDifficultyAttributes
        {
            StarRating = attributes.StarRating,
            MaxCombo = attributes.MaxCombo,
            StaminaDifficulty = attributes.StaminaDifficulty,
            MonoStaminaFactor = attributes.MonoStaminaFactor,
            RhythmDifficulty = attributes.RhythmDifficulty,
            ColourDifficulty = attributes.ColourDifficulty,
            PeakDifficulty = attributes.PeakDifficulty,
            GreatHitWindow = attributes.GreatHitWindow,
            OkHitWindow = attributes.OkHitWindow
        };
    }

    public static implicit operator Game.Rulesets.Taiko.Difficulty.TaikoDifficultyAttributes(TaikoDifficultyAttributes attributes)
    {
        return new TaikoDifficultyAttributes
        {
            StarRating = attributes.StarRating,
            MaxCombo = attributes.MaxCombo,
            StaminaDifficulty = attributes.StaminaDifficulty,
            MonoStaminaFactor = attributes.MonoStaminaFactor,
            RhythmDifficulty = attributes.RhythmDifficulty,
            ColourDifficulty = attributes.ColourDifficulty,
            PeakDifficulty = attributes.PeakDifficulty,
            GreatHitWindow = attributes.GreatHitWindow,
            OkHitWindow = attributes.OkHitWindow
        };
    }
#endif
}
