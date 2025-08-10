using osu.Game.Rulesets.Taiko.Difficulty;

namespace osu.Native.Structures.Difficulty;

internal struct NativeTaikoDifficultyAttributes(TaikoDifficultyAttributes attributes)
{
  public double StarRating = attributes.StarRating;
  public double MaxCombo = attributes.MaxCombo;
  public double RhythmDifficulty = attributes.RhythmDifficulty;
  public double ReadingDifficulty = attributes.ReadingDifficulty;
  public double ColourDifficulty = attributes.ColourDifficulty;
  public double StaminaDifficulty = attributes.StaminaDifficulty;
  public double MonoStaminaFactor = attributes.MonoStaminaFactor;
  public double RhythmTopStrains = attributes.RhythmTopStrains;
  public double ColourTopStrains = attributes.ColourTopStrains;
  public double StaminaTopStrains = attributes.StaminaTopStrains;
}
