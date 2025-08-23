using osu.Game.Rulesets.Catch.Difficulty;
using osu.Game.Rulesets.Mania.Difficulty;

namespace osu.Native.Structures.Difficulty;

/// <summary>
/// Represents the Catch difficulty attributes (<see cref="ManiaDifficultyAttributes"/>) on the native layer.
/// </summary>
internal struct NativeManiaDifficultyAttributes(ManiaDifficultyAttributes attributes)
{
  public double StarRating = attributes.StarRating;
  public int MaxCombo = attributes.MaxCombo;

  /// <summary>
  /// Converts the native difficulty attributes to a managed <see cref="ManiaDifficultyAttributes"/> instance.
  /// </summary>
  public ManiaDifficultyAttributes ToManaged()
  {
    return new ManiaDifficultyAttributes()
    {
      StarRating = StarRating,
      MaxCombo = MaxCombo
    };
  }
}
