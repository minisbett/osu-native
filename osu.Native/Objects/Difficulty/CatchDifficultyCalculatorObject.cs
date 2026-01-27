using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Catch.Difficulty;
using osu.Native.Compiler;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Objects.Difficulty;

/// <summary>
/// Represents a <see cref="CatchDifficultyCalculator"/>.
/// </summary>
internal unsafe partial class CatchDifficultyCalculatorObject : IOsuNativeObject<DifficultyCalculatorContext<CatchDifficultyCalculator>>
{
    /// <summary>
    /// Creates an instance of a <see cref="CatchDifficultyCalculator"/> for the specified ruleset and beatmap.
    /// </summary>
    /// <param name="rulesetHandle">The handle of the ruleset passed into the difficulty calculator.</param>
    /// <param name="beatmapHandle">The handle of the beatmap the difficulty calculator targets.</param>
    /// <param name="nativeCatchDifficultyCalculatorPtr">A pointer to write the resulting native difficulty calculator object to.</param>
    [OsuNativeFunction]
    public static ErrorCode Create(RulesetHandle rulesetHandle, BeatmapHandle beatmapHandle,
                                   NativeCatchDifficultyCalculator* nativeCatchDifficultyCalculatorPtr)
    {
        Ruleset ruleset = rulesetHandle.Resolve();
        FlatWorkingBeatmap beatmap = beatmapHandle.Resolve();

        if (ruleset is not CatchRuleset)
            return ErrorCode.UnexpectedRuleset;

        CatchDifficultyCalculator calculator = (CatchDifficultyCalculator)ruleset.CreateDifficultyCalculator(beatmap);
        DifficultyCalculatorContext<CatchDifficultyCalculator> context = new(ruleset, beatmap, calculator);

        *nativeCatchDifficultyCalculatorPtr = new() { Handle = ManagedObjectStore.Store(context) };

        return ErrorCode.Success;
    }

    private static void Calculate(CatchDifficultyCalculator calculator, Mod[] mods, NativeCatchDifficultyAttributes* nativeAttributesPtr)
    {
        CatchDifficultyAttributes attributes = (CatchDifficultyAttributes)calculator.Calculate(mods);
        *nativeAttributesPtr = new(attributes);
    }

    /// <summary>
    /// Calculates the difficulty attributes of the beatmap targetted by the specified difficulty calculator.
    /// </summary>
    /// <param name="calcHandle">The handle of the difficulty calculator.</param>
    /// <param name="nativeAttributesPtr">A pointer to write the resulting difficulty attributes to.</param>
    [OsuNativeFunction]
    public static ErrorCode Calculate(CatchDifficultyCalculatorHandle calcHandle, NativeCatchDifficultyAttributes* nativeAttributesPtr)
    {
        Calculate(calcHandle.Resolve().Calculator, [], nativeAttributesPtr);

        return ErrorCode.Success;
    }

    /// <summary>
    /// Calculates the difficulty attributes, including the specified mods, of the beatmap targetted by the specified difficulty calculator.
    /// </summary>
    /// <param name="calcHandle">The handle of the difficulty calculator.</param>
    /// <param name="modsHandle">The handle of the mods collection to consider.</param>
    /// <param name="nativeAttributesPtr">A pointer to write the resulting difficulty attributes to.</param>
    [OsuNativeFunction]
    public static ErrorCode CalculateMods(CatchDifficultyCalculatorHandle calcHandle, ModsCollectionHandle modsHandle,
                                          NativeCatchDifficultyAttributes* nativeAttributesPtr)
    {
        DifficultyCalculatorContext<CatchDifficultyCalculator> context = calcHandle.Resolve();
        Mod[] mods = [.. modsHandle.Resolve().Select(x => x.ToMod(context.Ruleset))];
        Calculate(context.Calculator, mods, nativeAttributesPtr);

        return ErrorCode.Success;
    }

    /// <summary>
    /// Calculates the timed (per-object) difficulty attributes of the beatmap targetted by the specified calculator.
    /// </summary>
    /// <param name="calcHandle">The handle of the difficulty calculator.</param>
    /// <param name="nativeTimedAttributesBuffer">A pointer to write the resulting timed difficulty attributes to.</param>
    /// <param name="bufferSize">The size of the provided buffer.</param>
    [OsuNativeFunction]
    public static ErrorCode CalculateTimed(CatchDifficultyCalculatorHandle calcHandle, NativeTimedCatchDifficultyAttributes* nativeTimedAttributesBuffer,
                                           int* bufferSize)
    {
        DifficultyCalculatorContext<CatchDifficultyCalculator> context = calcHandle.Resolve();

        if (nativeTimedAttributesBuffer is null)
        {
            *bufferSize = context.Beatmap.GetPlayableBeatmap(context.Ruleset.RulesetInfo).HitObjects.Count;
            return ErrorCode.BufferSizeQuery;
        }

        List<TimedDifficultyAttributes> attributes = context.Calculator.CalculateTimed();
        NativeTimedCatchDifficultyAttributes[] nativeAttributes = [..attributes.Select(
            x => new NativeTimedCatchDifficultyAttributes(x.Time, (CatchDifficultyAttributes)x.Attributes))];

        BufferHelper.Write(nativeAttributes, nativeTimedAttributesBuffer, bufferSize);
        return ErrorCode.Success;
    }

    /// <summary>
    /// Calculates the timed (per-object) difficulty attributes, including the specified mods, of the beatmap targetted by the specified calculator.
    /// </summary>
    /// <param name="calcHandle">The handle of the difficulty calculator.</param>
    /// <param name="modsHandle">The handle of the mods collection to consider.</param>
    /// <param name="nativeTimedAttributesBuffer">A pointer to write the resulting timed difficulty attributes to.</param>
    /// <param name="bufferSize">The size of the provided buffer.</param>
    [OsuNativeFunction]
    public static ErrorCode CalculateModsTimed(CatchDifficultyCalculatorHandle calcHandle, ModsCollectionHandle modsHandle,
                                               NativeTimedCatchDifficultyAttributes* nativeTimedAttributesBuffer, int* bufferSize)
    {
        DifficultyCalculatorContext<CatchDifficultyCalculator> context = calcHandle.Resolve();

        Mod[] mods = [.. modsHandle.Resolve().Select(x => x.ToMod(context.Ruleset))];

        if (nativeTimedAttributesBuffer is null)
        {
            *bufferSize = context.Beatmap.GetPlayableBeatmap(context.Ruleset.RulesetInfo).HitObjects.Count;
            return ErrorCode.BufferSizeQuery;
        }

        List<TimedDifficultyAttributes> attributes = context.Calculator.CalculateTimed(mods);
        NativeTimedCatchDifficultyAttributes[] nativeAttributes = [..attributes.Select(
            x => new NativeTimedCatchDifficultyAttributes(x.Time, (CatchDifficultyAttributes)x.Attributes))];

        BufferHelper.Write(nativeAttributes, nativeTimedAttributesBuffer, bufferSize);
        return ErrorCode.Success;
    }
}
