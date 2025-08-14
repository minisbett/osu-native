using osu.Game.Rulesets.Catch.Difficulty;

namespace osu.Native.Structures.Difficulty;

internal struct NativeCatchDifficultyAttributes(CatchDifficultyAttributes attributes)
{
  public double StarRating = attributes.StarRating;
  public int MaxCombo = attributes.MaxCombo;
}
