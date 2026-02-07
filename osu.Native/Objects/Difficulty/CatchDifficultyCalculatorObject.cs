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

    /// <summary>
    /// Calculates the difficulty attributes of the beatmap targetted by the specified difficulty calculator.
    /// </summary>
    /// <param name="calcHandle">The handle of the difficulty calculator.</param>
    /// <param name="modsHandle">The handle of the mods collection to consider. A null-handle equals to an empty mods collection.</param>
    /// <param name="nativeAttributesPtr">A pointer to write the resulting difficulty attributes to.</param>
    [OsuNativeFunction]
    public static ErrorCode Calculate(CatchDifficultyCalculatorHandle calcHandle, ModsCollectionHandle modsHandle,
                                      NativeCatchDifficultyAttributes* nativeAttributesPtr)
    {
        DifficultyCalculatorContext<CatchDifficultyCalculator> context = calcHandle.Resolve();
        Mod[] mods = modsHandle.IsNull ? [] : [.. modsHandle.Resolve().Select(x => x.ToMod(context.Ruleset))];

        CatchDifficultyAttributes attributes = (CatchDifficultyAttributes)context.Calculator.Calculate(mods);
        *nativeAttributesPtr = new(attributes);

        return ErrorCode.Success;
    }

    /// <summary>
    /// Calculates the timed (per-object) difficulty attributes of the beatmap targetted by the specified calculator.
    /// </summary>
    /// <param name="calcHandle">The handle of the difficulty calculator.</param>
    /// <param name="modsHandle">The handle of the mods collection to consider. A null-handle equals to an empty mods collection.</param>
    /// <param name="nativeTimedAttributesBuffer">A pointer to write the resulting timed difficulty attributes to.</param>
    /// <param name="bufferSize">The size of the provided buffer.</param>
    [OsuNativeFunction]
    public static ErrorCode CalculateTimed(CatchDifficultyCalculatorHandle calcHandle, ModsCollectionHandle modsHandle,
                                           NativeTimedCatchDifficultyAttributes* nativeTimedAttributesBuffer, int* bufferSize)
    {
        DifficultyCalculatorContext<CatchDifficultyCalculator> context = calcHandle.Resolve();
        Mod[] mods = modsHandle.IsNull ? [] : [.. modsHandle.Resolve().Select(x => x.ToMod(context.Ruleset))];

        if (nativeTimedAttributesBuffer is null)
        {
            *bufferSize = context.Beatmap.GetPlayableBeatmap(context.Ruleset.RulesetInfo, mods).HitObjects.Count;
            return ErrorCode.BufferSizeQuery;
        }

        List<TimedDifficultyAttributes> attributes = context.Calculator.CalculateTimed(mods);
        NativeTimedCatchDifficultyAttributes[] nativeAttributes = [.. attributes.Select(x => new NativeTimedCatchDifficultyAttributes(x))];

        BufferHelper.Write(nativeAttributes, nativeTimedAttributesBuffer, bufferSize);
        return ErrorCode.Success;
    }

    /// <summary>
    /// Calculates the timed (per-object) difficulty attributes of the beatmap targetted by the specified calculator.
    /// This function returns an enumerator allowing to lazily perform calculation of difficulty attributes.
    /// </summary>
    /// <param name="calcHandle">The handle of the difficulty calculator.</param>
    /// <param name="modsHandle">The handle of the mods collection to consider. A null-handle equals to an empty mods collection.</param>
    /// <param name="timedAttributesEnumeratorHandle">The handle for the enumerator.</param>
    [OsuNativeFunction]
    [OsuNativeEnumerator<NativeTimedCatchDifficultyAttributes>]
    public static ErrorCode CalculateTimedLazy(CatchDifficultyCalculatorHandle calcHandle, ModsCollectionHandle modsHandle,
                                               NativeCatchTimedDifficultyAttributesEnumeratorHandle* timedAttributesEnumeratorHandle)
    {
        DifficultyCalculatorContext<CatchDifficultyCalculator> context = calcHandle.Resolve();
        Mod[] mods = modsHandle.IsNull ? [] : [.. modsHandle.Resolve().Select(x => x.ToMod(context.Ruleset))];

        IEnumerator<NativeTimedCatchDifficultyAttributes> enumerator = context.Calculator.CalculateTimed(mods)
            .Select(x => new NativeTimedCatchDifficultyAttributes(x)).GetEnumerator();

        enumerator.MoveNext();

        *timedAttributesEnumeratorHandle = ManagedObjectStore.Store(enumerator);

        return ErrorCode.Success;
    }
}
