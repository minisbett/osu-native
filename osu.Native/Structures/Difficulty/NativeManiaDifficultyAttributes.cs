using osu.Game.Rulesets.Mania.Difficulty;

namespace osu.Native.Structures.Difficulty;

/// <summary>
/// Represents the Catch difficulty attributes (<see cref="ManiaDifficultyAttributes"/>) on the native layer.
/// </summary>
internal struct NativeManiaDifficultyAttributes(ManiaDifficultyAttributes attributes)
{
  public double StarRating = attributes.StarRating;
  public int MaxCombo = attributes.MaxCombo;
}
