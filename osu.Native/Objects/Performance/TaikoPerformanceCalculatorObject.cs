using osu.Game.Rulesets;
using osu.Game.Rulesets.Taiko.Difficulty;
using osu.Game.Scoring;
using osu.Native.Compiler;
using osu.Native.Structures;
using osu.Native.Structures.Difficulty;
using osu.Native.Structures.Performance;

namespace osu.Native.Objects.Performance;

/// <summary>
/// Represents the performance calculator for the Taiko ruleset (<see cref="TaikoPerformanceCalculator"/>).
/// </summary>
internal unsafe partial class TaikoPerformanceCalculatorObject : IOsuNativeObject<TaikoPerformanceCalculator>
{
  /// <summary>
  /// Creates an instance of a <see cref="TaikoPerformanceCalculator"/>.
  /// </summary>
  /// <param name="nativeTaikoPerformanceCalculatorPtr">A pointer to write the resulting native difficulty calculator object to.</param>
  [OsuNativeFunction]
  public static ErrorCode Create(NativeTaikoPerformanceCalculator* nativeTaikoPerformanceCalculatorPtr)
  {
    TaikoPerformanceCalculator calculator = new();

    *nativeTaikoPerformanceCalculatorPtr = new NativeTaikoPerformanceCalculator { Handle = ManagedObjectRegistry.Register(calculator) };

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
  public static ErrorCode Calculate(ManagedObjectHandle<TaikoPerformanceCalculator> calcHandle, NativeScoreInfo nativeScoreInfo,
                                    NativeTaikoDifficultyAttributes nativeDifficultyAttributes, NativeTaikoPerformanceAttributes* nativeAttributesPtr)
  {
    TaikoPerformanceCalculator calculator = calcHandle.Resolve();

    ScoreInfo scoreInfo = nativeScoreInfo.ToScoreInfo();

    TaikoDifficultyAttributes difficultyAttributes = nativeDifficultyAttributes.ToManaged();
    TaikoPerformanceAttributes attributes = (TaikoPerformanceAttributes)calculator.Calculate(scoreInfo, difficultyAttributes);
    *nativeAttributesPtr = new NativeTaikoPerformanceAttributes(attributes);

    return ErrorCode.Success;
  }
}
