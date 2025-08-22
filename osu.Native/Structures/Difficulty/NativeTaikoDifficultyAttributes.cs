using osu.Game.Rulesets.Catch.Difficulty;
using osu.Game.Rulesets.Taiko.Difficulty;

namespace osu.Native.Structures.Difficulty;

/// <summary>
/// Represents the Catch difficulty attributes (<see cref="TaikoDifficultyAttributes"/>) on the native layer.
/// </summary>
internal struct NativeTaikoDifficultyAttributes(TaikoDifficultyAttributes attributes)
{
  public double StarRating = attributes.StarRating;
  public int MaxCombo = attributes.MaxCombo;
  public double RhythmDifficulty = attributes.RhythmDifficulty;
  public double ReadingDifficulty = attributes.ReadingDifficulty;
  public double ColourDifficulty = attributes.ColourDifficulty;
  public double StaminaDifficulty = attributes.StaminaDifficulty;
  public double MonoStaminaFactor = attributes.MonoStaminaFactor;
  public double RhythmTopStrains = attributes.RhythmTopStrains;
  public double ColourTopStrains = attributes.ColourTopStrains;
  public double StaminaTopStrains = attributes.StaminaTopStrains;

  /// <summary>
  /// Converts the native difficulty attributes to a managed <see cref="TaikoDifficultyAttributes"/> instance.
  /// </summary>
  public TaikoDifficultyAttributes ToManaged()
  {
    return new TaikoDifficultyAttributes()
    {
      StarRating = StarRating,
      MaxCombo = MaxCombo,
      RhythmDifficulty = RhythmDifficulty,
      ReadingDifficulty = ReadingDifficulty,
      ColourDifficulty = ColourDifficulty,
      StaminaDifficulty = StaminaDifficulty,
      MonoStaminaFactor = MonoStaminaFactor,
      RhythmTopStrains = RhythmTopStrains,
      ColourTopStrains = ColourTopStrains,
      StaminaTopStrains = StaminaTopStrains
    };
  }
}
