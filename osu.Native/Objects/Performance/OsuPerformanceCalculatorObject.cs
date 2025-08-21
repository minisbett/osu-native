using osu.Game.Rulesets;
using osu.Game.Rulesets.Osu.Difficulty;
using osu.Game.Scoring;
using osu.Native.Compiler;
using osu.Native.Structures;
using osu.Native.Structures.Difficulty;
using osu.Native.Structures.Performance;

namespace osu.Native.Objects.Performance;

/// <summary>
/// Represents the performance calculator for the Osu ruleset (<see cref="OsuPerformanceCalculator"/>).
/// </summary>
internal unsafe partial class OsuPerformanceCalculatorObject : IOsuNativeObject<OsuPerformanceCalculator>
{
  /// <summary>
  /// Creates an instance of a <see cref="OsuPerformanceCalculator"/>.
  /// </summary>
  /// <param name="nativeOsuPerformanceCalculatorPtr">A pointer to write the resulting native difficulty calculator object to.</param>
  [OsuNativeFunction]
  public static ErrorCode Create(NativeOsuPerformanceCalculator* nativeOsuPerformanceCalculatorPtr)
  {
    OsuPerformanceCalculator calculator = new();

    *nativeOsuPerformanceCalculatorPtr = new NativeOsuPerformanceCalculator { Handle = ManagedObjectRegistry.Register(calculator) };

    return ErrorCode.Success;
  }

  /// <summary>
  /// Calculates the performance attributes of the specified score, assuming the specified difficulty attributes.
  /// </summary>
  /// <param name="calcHandle">The handle of the performance calculator.</param>
  /// <param name="rulesetHandle">The handle of the ruleset use to instantiate the mods.</param>
  /// <param name="nativeScore">The native score to calculate the performance of.</param>
  /// <param name="nativeDifficultyAttributes">The difficulty attributes to calculate the performance with.</param>
  /// <param name="nativeAttributesPtr">A pointer to write the resulting performance attributes to.</param>
  [OsuNativeFunction]
  public static ErrorCode Calculate(ManagedObjectHandle<OsuPerformanceCalculator> calcHandle, ManagedObjectHandle<Ruleset> rulesetHandle,
                                    NativeScore nativeScore, NativeOsuDifficultyAttributes nativeDifficultyAttributes,
                                    NativeOsuPerformanceAttributes* nativeAttributesPtr)
  {
    OsuPerformanceCalculator calculator = calcHandle.Resolve();
    Ruleset ruleset = rulesetHandle.Resolve();

    ScoreInfo scoreInfo = nativeScore.ToScoreInfo(ruleset);

    OsuDifficultyAttributes difficultyAttributes = nativeDifficultyAttributes.ToManaged();
    OsuPerformanceAttributes attributes = (OsuPerformanceAttributes)calculator.Calculate(scoreInfo, difficultyAttributes);
    *nativeAttributesPtr = new NativeOsuPerformanceAttributes(attributes);

    return ErrorCode.Success;
  }
}
