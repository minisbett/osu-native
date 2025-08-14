using osu.Game.Rulesets.Mania.Difficulty;

namespace osu.Native.Structures.Difficulty;

internal struct NativeManiaDifficultyAttributes(ManiaDifficultyAttributes attributes)
{
  public double StarRating = attributes.StarRating;
  public int MaxCombo = attributes.MaxCombo;
}
