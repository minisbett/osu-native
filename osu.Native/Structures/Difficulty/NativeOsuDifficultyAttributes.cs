using osu.Game.Rulesets.Osu.Difficulty;

namespace osu.Native.Structures.Difficulty;

/// <summary>
/// Represents the Catch difficulty attributes (<see cref="OsuDifficultyAttributes"/>) on the native layer.
/// </summary>
internal struct NativeOsuDifficultyAttributes(OsuDifficultyAttributes attributes)
{
  public double StarRating = attributes.StarRating;
  public int MaxCombo = attributes.MaxCombo;
  public double AimDifficulty = attributes.AimDifficulty;
  public double AimDifficultSliderCount = attributes.AimDifficultSliderCount;
  public double SpeedDifficulty = attributes.SpeedDifficulty;
  public double SpeedNoteCount = attributes.SpeedNoteCount;
  public double FlashlightDifficulty = attributes.FlashlightDifficulty;
  public double SliderFactor = attributes.SliderFactor;
  public double AimDifficultStrainCount = attributes.AimDifficultStrainCount;
  public double SpeedDifficultStrainCount = attributes.SpeedDifficultStrainCount;
  public double DrainRate = attributes.DrainRate;
  public int HitCircleCount = attributes.HitCircleCount;
  public int SliderCount = attributes.SliderCount;
  public int SpinnerCount = attributes.SpinnerCount;

  public OsuDifficultyAttributes ToManaged()
  {
    return new OsuDifficultyAttributes()
    {
      StarRating = StarRating,
      MaxCombo = MaxCombo,
      AimDifficulty = AimDifficulty,
      AimDifficultSliderCount = AimDifficultSliderCount,
      SpeedDifficulty = SpeedDifficulty,
      SpeedNoteCount = SpeedNoteCount,
      FlashlightDifficulty = FlashlightDifficulty,
      SliderFactor = SliderFactor,
      AimDifficultStrainCount = AimDifficultStrainCount,
      SpeedDifficultStrainCount = SpeedDifficultStrainCount,
      DrainRate = DrainRate,
      HitCircleCount = HitCircleCount,
      SliderCount = SliderCount,
      SpinnerCount = SpinnerCount
    };
  }
}
