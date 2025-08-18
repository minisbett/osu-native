using osu.Game.Beatmaps;
using osu.Game.Online.API;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Mania.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Native.Compiler;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Objects;

/// <summary>
/// Represents the difficulty calculator for the Mania ruleset (<see cref="ManiaDifficultyCalculator"/>).
/// </summary>
internal unsafe partial class ManiaDifficultyCalculatorObject : IOsuNativeObject<ManiaDifficultyCalculator>
{
  /// <summary>
  /// Creates an instance of a <see cref="ManiaDifficultyCalculator"/> for the specified ruleset and beatmap.
  /// </summary>
  /// <param name="rulesetHandle">The handle of the ruleset passed into the difficulty calculator.</param>
  /// <param name="beatmapHandle">The handle of the beatmap the difficulty calculator targets.</param>
  /// <param name="nativeManiaDifficultyCalculatorPtr">A pointer to write the resulting native difficulty calculator object to.</param>
  [OsuNativeFunction]
  public static ErrorCode Create(ManagedObjectHandle<Ruleset> rulesetHandle, ManagedObjectHandle<FlatWorkingBeatmap> beatmapHandle,
                                 NativeManiaDifficultyCalculator* nativeManiaDifficultyCalculatorPtr)
  {
    Ruleset ruleset = rulesetHandle.Resolve();
    FlatWorkingBeatmap beatmap = beatmapHandle.Resolve();

    if (ruleset is not ManiaRuleset)
      return ErrorCode.UnexpectedRuleset;

    ManiaDifficultyCalculator calculator = (ManiaDifficultyCalculator)ruleset.CreateDifficultyCalculator(beatmap);

    *nativeManiaDifficultyCalculatorPtr = new NativeManiaDifficultyCalculator { Handle = ManagedObjectRegistry.Register(calculator) };

    return ErrorCode.Success;
  }

  /// <summary>
  /// Calculates the difficulty attributes of the beatmap targetted by the specified difficulty calculator.
  /// </summary>
  /// <param name="calcHandle">The handle of the difficulty calculator.</param>
  /// <param name="nativeAttributesPtr">A pointer to write the resulting difficulty attributes to.</param>
  [OsuNativeFunction]
  public static ErrorCode Calculate(ManagedObjectHandle<ManiaDifficultyCalculator> calcHandle, NativeManiaDifficultyAttributes* nativeAttributesPtr)
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
  public static ErrorCode CalculateMods(ManagedObjectHandle<ManiaDifficultyCalculator> calcHandle, ManagedObjectHandle<Ruleset> rulesetHandle,
                                        ManagedObjectHandle<List<APIMod>> modsHandle, NativeManiaDifficultyAttributes* nativeAttributesPtr)
  {
    Ruleset ruleset = rulesetHandle.Resolve();

    if (ruleset is not ManiaRuleset)
      return ErrorCode.UnexpectedRuleset;

    Mod[] mods = [.. modsHandle.Resolve().Select(x => x.ToMod(ruleset))];
    Calculate(calcHandle.Resolve(), mods, nativeAttributesPtr);

    return ErrorCode.Success;
  }

  private static void Calculate(ManiaDifficultyCalculator calculator, Mod[] mods, NativeManiaDifficultyAttributes* nativeAttributesPtr)
  {
    ManiaDifficultyAttributes attributes = (ManiaDifficultyAttributes)calculator.Calculate(mods);
    *nativeAttributesPtr = new NativeManiaDifficultyAttributes(attributes);
  }
}
