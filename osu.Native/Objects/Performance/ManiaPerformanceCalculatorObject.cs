using osu.Game.Rulesets;
using osu.Game.Rulesets.Mania.Difficulty;
using osu.Game.Scoring;
using osu.Native.Compiler;
using osu.Native.Structures;
using osu.Native.Structures.Difficulty;
using osu.Native.Structures.Performance;

namespace osu.Native.Objects.Performance;

/// <summary>
/// Represents the performance calculator for the Mania ruleset (<see cref="ManiaPerformanceCalculator"/>).
/// </summary>
internal unsafe partial class ManiaPerformanceCalculatorObject : IOsuNativeObject<ManiaPerformanceCalculator>
{
  /// <summary>
  /// Creates an instance of a <see cref="ManiaPerformanceCalculator"/>.
  /// </summary>
  /// <param name="nativeManiaPerformanceCalculatorPtr">A pointer to write the resulting native difficulty calculator object to.</param>
  [OsuNativeFunction]
  public static ErrorCode Create(NativeManiaPerformanceCalculator* nativeManiaPerformanceCalculatorPtr)
  {
    ManiaPerformanceCalculator calculator = new();

    *nativeManiaPerformanceCalculatorPtr = new NativeManiaPerformanceCalculator { Handle = ManagedObjectStore.Store(calculator) };

    return ErrorCode.Success;
  }

  /// <summary>
  /// Calculates the performance attributes of the specified score, assuming the specified difficulty attributes.
  /// </summary>
  /// <param name="calcHandle">The handle of the performance calculator.</param>
  /// <param name="nativeScoreInfo">The native score to calculate the performance of.</param>
  /// <param name="nativeDifficultyAttributes">The difficulty attributes to calculate the performance with.</param>
  /// <param name="nativeAttributesPtr">A pointer to write the resulting performance attributes to.</param>
  [OsuNativeFunction]
  public static ErrorCode Calculate(ManagedObjectHandle<ManiaPerformanceCalculator> calcHandle, NativeScoreInfo nativeScoreInfo,
                                    NativeManiaDifficultyAttributes nativeDifficultyAttributes, NativeManiaPerformanceAttributes* nativeAttributesPtr)
  {
    ManiaPerformanceCalculator calculator = calcHandle.Resolve();

    ScoreInfo scoreInfo = nativeScoreInfo.ToScoreInfo();

    ManiaDifficultyAttributes difficultyAttributes = nativeDifficultyAttributes.ToManaged();
    ManiaPerformanceAttributes attributes = (ManiaPerformanceAttributes)calculator.Calculate(scoreInfo, difficultyAttributes);
    *nativeAttributesPtr = new NativeManiaPerformanceAttributes(attributes);

    return ErrorCode.Success;
  }
}
