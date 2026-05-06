using osu.Game.Rulesets.Taiko.Difficulty;

namespace osu.Native.Structures.Difficulty;

/// <summary>
/// Represents the Catch difficulty attributes (<see cref="TaikoDifficultyAttributes"/>) on the native layer.
/// </summary>
public readonly struct NativeTaikoDifficultyAttributes(TaikoDifficultyAttributes attributes)
{
    public readonly double StarRating = attributes.StarRating;
    public readonly int MaxCombo = attributes.MaxCombo;
    public readonly double MechanicalDifficulty = attributes.MechanicalDifficulty;
    public readonly double RhythmDifficulty = attributes.RhythmDifficulty;
    public readonly double ReadingDifficulty = attributes.ReadingDifficulty;
    public readonly double ColourDifficulty = attributes.ColourDifficulty;
    public readonly double StaminaDifficulty = attributes.StaminaDifficulty;
    public readonly double MonoStaminaFactor = attributes.MonoStaminaFactor;
    public readonly double ConsistencyFactor = attributes.ConsistencyFactor;
    public readonly double StaminaTopStrains = attributes.StaminaTopStrains;

    /// <summary>
    /// Converts the native difficulty attributes to a managed <see cref="TaikoDifficultyAttributes"/> instance.
    /// </summary>
    public readonly TaikoDifficultyAttributes ToManaged()
    {
        return new()
        {
            StarRating = StarRating,
            MaxCombo = MaxCombo,
            MechanicalDifficulty = MechanicalDifficulty,
            RhythmDifficulty = RhythmDifficulty,
            ReadingDifficulty = ReadingDifficulty,
            ColourDifficulty = ColourDifficulty,
            StaminaDifficulty = StaminaDifficulty,
            MonoStaminaFactor = MonoStaminaFactor,
            ConsistencyFactor = ConsistencyFactor,
            StaminaTopStrains = StaminaTopStrains
        };
    }
}
