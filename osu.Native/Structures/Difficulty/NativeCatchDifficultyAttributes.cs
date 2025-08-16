using osu.Game.Rulesets.Catch.Difficulty;

namespace osu.Native.Structures.Difficulty;

/// <summary>
/// Represents the Catch difficulty attributes (<see cref="CatchDifficultyAttributes"/>) on the native layer.
/// </summary>
internal struct NativeCatchDifficultyAttributes(CatchDifficultyAttributes attributes)
{
  public double StarRating = attributes.StarRating;
  public int MaxCombo = attributes.MaxCombo;
}
