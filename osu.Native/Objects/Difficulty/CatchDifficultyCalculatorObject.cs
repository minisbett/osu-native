using osu.Game.Beatmaps;
using osu.Game.Online.API;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Catch.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Native.Compiler;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Objects;

/// <summary>
/// Represents the difficulty calculator for the Catch ruleset (<see cref="CatchDifficultyCalculator"/>).
/// </summary>
internal unsafe partial class CatchDifficultyCalculatorObject : IOsuNativeObject<CatchDifficultyCalculator>
{
  /// <summary>
  /// Creates an instance of a <see cref="CatchDifficultyCalculator"/> for the specified ruleset and beatmap.
  /// </summary>
  /// <param name="rulesetHandle">The handle of the ruleset passed into the difficulty calculator.</param>
  /// <param name="beatmapHandle">The handle of the beatmap the difficulty calculator targets.</param>
  /// <param name="nativeCatchDifficultyCalculatorPtr">A pointer to write the resulting native difficulty calculator object to.</param>
  [OsuNativeFunction]
  public static ErrorCode Create(ManagedObjectHandle<Ruleset> rulesetHandle, ManagedObjectHandle<FlatWorkingBeatmap> beatmapHandle,
                                 NativeCatchDifficultyCalculator* nativeCatchDifficultyCalculatorPtr)
  {
    Ruleset ruleset = rulesetHandle.Resolve();
    FlatWorkingBeatmap beatmap = beatmapHandle.Resolve();

    if (ruleset is not CatchRuleset)
      return ErrorCode.UnexpectedRuleset;

    CatchDifficultyCalculator calculator = new(ruleset.RulesetInfo, beatmap);
    ManagedObjectHandle<CatchDifficultyCalculator> handle = ManagedObjectRegistry<CatchDifficultyCalculator>.Register(calculator);

    *nativeCatchDifficultyCalculatorPtr = new NativeCatchDifficultyCalculator()
    {
      Handle = handle
    };

    return ErrorCode.Success;
  }

  /// <summary>
  /// Calculates the difficulty attributes of the beatmap targetted by the specified difficulty calculator.
  /// </summary>
  /// <param name="calcHandle">The handle of the difficulty calculator.</param>
  /// <param name="nativeAttributesPtr">A pointer to write the resulting difficulty attributes to.</param>
  [OsuNativeFunction]
  public static ErrorCode Calculate(ManagedObjectHandle<CatchDifficultyCalculator> calcHandle, NativeCatchDifficultyAttributes* nativeAttributesPtr)
  {
    Calculate(calcHandle.Resolve(), [], nativeAttributesPtr);

    return ErrorCode.Success;
  }


  /// <summary>
  /// Calculates the difficulty attributes, including the specified mods, of the beatmap targetted by the specified difficulty calculator.
  /// </summary>
  /// <param name="calcHandle">The handle of the difficulty calculator.</param>
  /// <param name="rulesetHandle">The handle of the ruleset use to instantiate the mods (must match the ruleset of the calculator).</param>
  /// <param name="modsHandle">The handle of the mods collection to consider.</param>
  /// <param name="nativeAttributesPtr">A pointer to write the resulting difficulty attributes to.</param>
  [OsuNativeFunction]
  public static ErrorCode CalculateMods(ManagedObjectHandle<CatchDifficultyCalculator> calcHandle, ManagedObjectHandle<Ruleset> rulesetHandle,
                                        ManagedObjectHandle<List<APIMod>> modsHandle, NativeCatchDifficultyAttributes* nativeAttributesPtr)
  {
    Ruleset ruleset = rulesetHandle.Resolve();

    if (ruleset is not CatchRuleset)
      return ErrorCode.UnexpectedRuleset;

    Mod[] mods = [.. modsHandle.Resolve().Select(x => x.ToMod(ruleset))];

    Calculate(calcHandle.Resolve(), mods, nativeAttributesPtr);

    return ErrorCode.Success;
  }

  private static void Calculate(CatchDifficultyCalculator calculator, Mod[] mods, NativeCatchDifficultyAttributes* nativeAttributesPr)
  {
    CatchDifficultyAttributes attributes = (CatchDifficultyAttributes)calculator.Calculate(mods);
    *nativeAttributesPr = new NativeCatchDifficultyAttributes(attributes);
  }
}
