using osu.Game.Rulesets.Catch.Difficulty;
using osu.Game.Rulesets.Mania.Difficulty;
using osu.Game.Rulesets.Osu.Difficulty;
using osu.Game.Rulesets.Taiko.Difficulty;

namespace osu.Native.Structs;

public static class StructHelper
{
    #region osu!

    public static NativeOsuDifficultyAttributes DifficultyAttributesToStruct(OsuDifficultyAttributes attributes)
    {
        return new NativeOsuDifficultyAttributes
        {
            StarRating = attributes.StarRating,
            MaxCombo = attributes.MaxCombo,
            AimDifficulty = attributes.AimDifficulty,
            SpeedDifficulty = attributes.SpeedDifficulty,
            SpeedNoteCount = attributes.SpeedNoteCount,
            FlashlightDifficulty = attributes.FlashlightDifficulty,
            SliderFactor = attributes.SliderFactor,
            AimDifficultStrainCount = attributes.AimDifficultStrainCount,
            SpeedDifficultStrainCount = attributes.SpeedDifficultStrainCount,
            ApproachRate = attributes.ApproachRate,
            OverallDifficulty = attributes.OverallDifficulty,
            DrainRate = attributes.DrainRate,
            HitCircleCount = attributes.HitCircleCount,
            SliderCount = attributes.SliderCount,
            SpinnerCount = attributes.SpinnerCount
        };
    }
    
    public static NativeOsuPerformanceAttributes PerformanceAttributesToStruct(OsuPerformanceAttributes attributes)
    {
        return new NativeOsuPerformanceAttributes
        {
            Total = attributes.Total,
            Aim = attributes.Aim,
            Speed = attributes.Speed,
            Accuracy = attributes.Accuracy,
            Flashlight = attributes.Flashlight,
            EffectiveMissCount = attributes.EffectiveMissCount
        };
    }

    #endregion

    #region Taiko

    public static NativeTaikoDifficultyAttributes DifficultyAttributesToStruct(TaikoDifficultyAttributes attributes)
    {
        return new NativeTaikoDifficultyAttributes
        {
            StarRating = attributes.StarRating,
            MaxCombo = attributes.MaxCombo,
            StaminaDifficulty = attributes.StaminaDifficulty,
            RhythmDifficulty = attributes.RhythmDifficulty,
            ColourDifficulty = attributes.ColourDifficulty,
            PeakDifficulty = attributes.PeakDifficulty,
            GreatHitWindow = attributes.GreatHitWindow,
            OkHitWindow = attributes.OkHitWindow
        };
    }

    public static NativeTaikoPerformanceAttributes PerformanceAttributesToStruct(TaikoPerformanceAttributes attributes)
    {
        return new NativeTaikoPerformanceAttributes
        {
            Total = attributes.Total,
            Difficulty = attributes.Difficulty,
            Accuracy = attributes.Accuracy,
            EffectiveMissCount = attributes.EffectiveMissCount,
            EstimatedUnstableRate = attributes.EstimatedUnstableRate
        };
    }

    #endregion

    #region Catch

    public static NativeCatchDifficultyAttributes DifficultyAttributesToStruct(CatchDifficultyAttributes attributes)
    {
        return new NativeCatchDifficultyAttributes
        {
            StarRating = attributes.StarRating,
            MaxCombo = attributes.MaxCombo,
            ApproachRate = attributes.ApproachRate
        };
    }

    public static NativeCatchPerformanceAttributes PerformanceAttributesToStruct(CatchPerformanceAttributes attributes)
    {
        return new NativeCatchPerformanceAttributes
        {
            Total = attributes.Total
        };
    }

    #endregion

    #region Mania

    public static NativeManiaDifficultyAttributes DifficultyAttributesToStruct(ManiaDifficultyAttributes attributes)
    {
        return new NativeManiaDifficultyAttributes
        {
            StarRating = attributes.StarRating,
            MaxCombo = attributes.MaxCombo,
            GreatHitWindow = attributes.GreatHitWindow
        };
    }

    public static NativeManiaPerformanceAttributes PerformanceAttributesToStruct(ManiaPerformanceAttributes attributes)
    {
        return new NativeManiaPerformanceAttributes
        {
            Total = attributes.Total,
            Difficulty = attributes.Difficulty
        };
    }

    #endregion
}
