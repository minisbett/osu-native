using osu.Game.Rulesets.Taiko.Difficulty;

namespace osu.Native.Structures.Difficulty;

/// <summary>
/// Represents the Catch difficulty attributes (<see cref="TaikoDifficultyAttributes"/>) on the native layer.
/// </summary>
public struct NativeTaikoDifficultyAttributes(TaikoDifficultyAttributes attributes)
{
    public double StarRating = attributes.StarRating;
    public int MaxCombo = attributes.MaxCombo;
    public double MechanicalDifficulty = attributes.MechanicalDifficulty;
    public double RhythmDifficulty = attributes.RhythmDifficulty;
    public double ReadingDifficulty = attributes.ReadingDifficulty;
    public double ColourDifficulty = attributes.ColourDifficulty;
    public double StaminaDifficulty = attributes.StaminaDifficulty;
    public double MonoStaminaFactor = attributes.MonoStaminaFactor;
    public double ConsistencyFactor = attributes.ConsistencyFactor;
    public double StaminaTopStrains = attributes.StaminaTopStrains;

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
