// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.InteropServices;

#if IS_BINDINGS
namespace osu.Native.Bindings.Structures.Difficulty;
#else
namespace osu.Native.Structures.Difficulty;
#endif

[StructLayout(LayoutKind.Sequential)]
public struct CatchDifficultyAttributes
{
    public double StarRating;
    public int MaxCombo;
    public double ApproachRate;

#if !IS_BINDINGS
    public static implicit operator CatchDifficultyAttributes(Game.Rulesets.Catch.Difficulty.CatchDifficultyAttributes attributes)
    {
        return new CatchDifficultyAttributes
        {
            StarRating = attributes.StarRating,
            MaxCombo = attributes.MaxCombo,
            ApproachRate = attributes.ApproachRate
        };
    }

    public static implicit operator Game.Rulesets.Catch.Difficulty.CatchDifficultyAttributes(CatchDifficultyAttributes attributes)
    {
        return new CatchDifficultyAttributes
        {
            StarRating = attributes.StarRating,
            MaxCombo = attributes.MaxCombo,
            ApproachRate = attributes.ApproachRate
        };
    }
#endif
}