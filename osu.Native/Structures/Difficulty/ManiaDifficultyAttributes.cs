// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.InteropServices;

#if IS_BINDINGS
namespace osu.Native.Bindings.Structures.Difficulty;
#else
namespace osu.Native.Structures.Difficulty;
#endif

/// <summary>
/// Represents the calculated difficulty attributes for an osu!mania beatmap.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ManiaDifficultyAttributes
{
    public double StarRating;
    public int MaxCombo;
    public double GreatHitWindow;

#if !IS_BINDINGS
    public static implicit operator ManiaDifficultyAttributes(Game.Rulesets.Mania.Difficulty.ManiaDifficultyAttributes attributes)
    {
        return new ManiaDifficultyAttributes
        {
            StarRating = attributes.StarRating,
            MaxCombo = attributes.MaxCombo,
            GreatHitWindow = attributes.GreatHitWindow
        };
    }

    public static implicit operator Game.Rulesets.Mania.Difficulty.ManiaDifficultyAttributes(ManiaDifficultyAttributes attributes)
    {
        return new ManiaDifficultyAttributes
        {
            StarRating = attributes.StarRating,
            MaxCombo = attributes.MaxCombo,
            GreatHitWindow= attributes.GreatHitWindow
        };
    }
#endif
}
