using osu.Game.Rulesets.Catch.Difficulty;
using osu.Game.Scoring;
using osu.Native.Compiler;
using osu.Native.Structures;
using osu.Native.Structures.Difficulty;
using osu.Native.Structures.Performance;

namespace osu.Native.Objects.Performance;

/// <summary>
/// Represents the performance calculator for the Catch ruleset (<see cref="CatchPerformanceCalculator"/>).
/// </summary>
internal unsafe partial class CatchPerformanceCalculatorObject : IOsuNativeObject<CatchPerformanceCalculator>
{
    /// <summary>
    /// Creates an instance of a <see cref="CatchPerformanceCalculator"/>.
    /// </summary>
    /// <param name="nativeCatchPerformanceCalculatorPtr">A pointer to write the resulting native difficulty calculator object to.</param>
    [OsuNativeFunction]
    public static ErrorCode Create(NativeCatchPerformanceCalculator* nativeCatchPerformanceCalculatorPtr)
    {
        CatchPerformanceCalculator calculator = new();

        *nativeCatchPerformanceCalculatorPtr = new NativeCatchPerformanceCalculator { Handle = ManagedObjectStore.Store(calculator) };

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
    public static ErrorCode Calculate(ManagedObjectHandle<CatchPerformanceCalculator> calcHandle, NativeScoreInfo nativeScoreInfo,
                                      NativeCatchDifficultyAttributes nativeDifficultyAttributes, NativeCatchPerformanceAttributes* nativeAttributesPtr)
    {
        CatchPerformanceCalculator calculator = calcHandle.Resolve();

        ScoreInfo scoreInfo = nativeScoreInfo.ToScoreInfo();

        CatchDifficultyAttributes difficultyAttributes = nativeDifficultyAttributes.ToManaged();
        CatchPerformanceAttributes attributes = (CatchPerformanceAttributes)calculator.Calculate(scoreInfo, difficultyAttributes);
        *nativeAttributesPtr = new NativeCatchPerformanceAttributes(attributes);

        return ErrorCode.Success;
    }
}
