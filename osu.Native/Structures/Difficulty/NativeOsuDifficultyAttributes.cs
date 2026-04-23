using osu.Game.Rulesets.Osu.Difficulty;

namespace osu.Native.Structures.Difficulty;

/// <summary>
/// Represents the Catch difficulty attributes (<see cref="OsuDifficultyAttributes"/>) on the native layer.
/// </summary>
public readonly struct NativeOsuDifficultyAttributes(OsuDifficultyAttributes attributes)
{
    public readonly double StarRating = attributes.StarRating;
    public readonly int MaxCombo = attributes.MaxCombo;
    public readonly double AimDifficulty = attributes.AimDifficulty;
    public readonly double AimDifficultSliderCount = attributes.AimDifficultSliderCount;
    public readonly double SpeedDifficulty = attributes.SpeedDifficulty;
    public readonly double SpeedNoteCount = attributes.SpeedNoteCount;
    public readonly double FlashlightDifficulty = attributes.FlashlightDifficulty;
    public readonly double SliderFactor = attributes.SliderFactor;
    public readonly double AimTopWeightedSliderFactor = attributes.AimTopWeightedSliderFactor;
    public readonly double SpeedTopWeightedSliderFactor = attributes.SpeedTopWeightedSliderFactor;
    public readonly double AimDifficultStrainCount = attributes.AimDifficultStrainCount;
    public readonly double SpeedDifficultStrainCount = attributes.SpeedDifficultStrainCount;
    public readonly double NestedScorePerObject = attributes.NestedScorePerObject;
    public readonly double LegacyScoreBaseMultiplier = attributes.LegacyScoreBaseMultiplier;
    public readonly double MaximumLegacyComboScore = attributes.MaximumLegacyComboScore;
    public readonly double DrainRate = attributes.DrainRate;
    public readonly int HitCircleCount = attributes.HitCircleCount;
    public readonly int SliderCount = attributes.SliderCount;
    public readonly int SpinnerCount = attributes.SpinnerCount;

    /// <summary>
    /// Converts the native difficulty attributes to a managed <see cref="OsuDifficultyAttributes"/> instance.
    /// </summary>
    public readonly OsuDifficultyAttributes ToManaged()
    {
        return new()
        {
            StarRating = StarRating,
            MaxCombo = MaxCombo,
            AimDifficulty = AimDifficulty,
            AimDifficultSliderCount = AimDifficultSliderCount,
            SpeedDifficulty = SpeedDifficulty,
            SpeedNoteCount = SpeedNoteCount,
            FlashlightDifficulty = FlashlightDifficulty,
            SliderFactor = SliderFactor,
            AimTopWeightedSliderFactor = AimTopWeightedSliderFactor,
            SpeedTopWeightedSliderFactor = SpeedTopWeightedSliderFactor,
            AimDifficultStrainCount = AimDifficultStrainCount,
            SpeedDifficultStrainCount = SpeedDifficultStrainCount,
            NestedScorePerObject = NestedScorePerObject,
            LegacyScoreBaseMultiplier = LegacyScoreBaseMultiplier,
            MaximumLegacyComboScore = MaximumLegacyComboScore,
            DrainRate = DrainRate,
            HitCircleCount = HitCircleCount,
            SliderCount = SliderCount,
            SpinnerCount = SpinnerCount
        };
    }
}
