using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Online.API;

namespace osu.Native.Objects.Difficulty;

/// <summary>
/// A wrapper around a <see cref="DifficultyCalculator"/>, providing more contextual information (eg. the ruleset and beatmap).
/// </summary>
public class DifficultyCalculatorContext<TCalculator>(Ruleset ruleset, FlatWorkingBeatmap beatmap, TCalculator calculator)
    where TCalculator : DifficultyCalculator
{
    // Note: Re-using the ruleset used to create a calculator for eg. instantiating mods violates the osu!(lazer) OOP logic,
    //       as mods passed into a calculator are free to be instantiated using any other instance of Ruleset.
    //       However, we are doing this for convenience, as it avoids having to pass rulesets whenever mods are to be instantiated.
    /// <summary>
    /// The ruleset used to create the difficulty calculator. This ruleset instance is re-used to instantiate mods,
    /// since osu-native's <see cref="ModObject"/> represents a ruleset-agnostic <see cref="APIMod"/> per design.
    /// </summary>
    public Ruleset Ruleset { get; } = ruleset;

    /// <summary>
    /// The beatmap used to create the difficulty calculator. This is not exposed via the calculator itself, hence why it is provided here.
    /// </summary>
    public FlatWorkingBeatmap Beatmap { get; } = beatmap;

    /// <summary>
    /// The difficulty calculator this context is wrapping.
    /// </summary>
    public TCalculator Calculator { get; } = calculator;
}