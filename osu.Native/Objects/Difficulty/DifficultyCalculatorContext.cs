using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Difficulty;

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
    public Ruleset Ruleset { get; } = ruleset;

    public FlatWorkingBeatmap Beatmap { get; } = beatmap;

    public TCalculator Calculator { get; } = calculator;
}