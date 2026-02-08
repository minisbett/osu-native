using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Taiko;
using osu.Game.Rulesets.Taiko.Difficulty;
using osu.Native.Compiler;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Objects.Difficulty;

/// <summary>
/// Represents a <see cref="TaikoDifficultyCalculator"/>.
/// </summary>
internal unsafe partial class TaikoDifficultyCalculatorObject : IOsuNativeObject<DifficultyCalculatorContext<TaikoDifficultyCalculator>>
{
    /// <summary>
    /// Creates an instance of a <see cref="TaikoDifficultyCalculator"/> for the specified ruleset and beatmap.
    /// </summary>
    /// <param name="rulesetHandle">The handle of the ruleset passed into the difficulty calculator.</param>
    /// <param name="beatmapHandle">The handle of the beatmap the difficulty calculator targets.</param>
    /// <param name="nativeTaikoDifficultyCalculatorPtr">A pointer to write the resulting native difficulty calculator object to.</param>
    [OsuNativeFunction]
    public static ErrorCode Create(RulesetHandle rulesetHandle, BeatmapHandle beatmapHandle,
                                   NativeTaikoDifficultyCalculator* nativeTaikoDifficultyCalculatorPtr)
    {
        Ruleset ruleset = rulesetHandle.Resolve();
        FlatWorkingBeatmap beatmap = beatmapHandle.Resolve();

        if (ruleset is not TaikoRuleset)
            return ErrorCode.UnexpectedRuleset;

        TaikoDifficultyCalculator calculator = (TaikoDifficultyCalculator)ruleset.CreateDifficultyCalculator(beatmap);
        DifficultyCalculatorContext<TaikoDifficultyCalculator> context = new(ruleset, beatmap, calculator);

        *nativeTaikoDifficultyCalculatorPtr = new() { Handle = ManagedObjectStore.Store(context) };

        return ErrorCode.Success;
    }

    /// <summary>
    /// Calculates the difficulty attributes of the beatmap targetted by the specified difficulty calculator.
    /// </summary>
    /// <param name="calcHandle">The handle of the difficulty calculator.</param>
    /// <param name="modsHandle">The handle of the mods collection to consider. A null-handle equals to an empty mods collection.</param>
    /// <param name="nativeAttributesPtr">A pointer to write the resulting difficulty attributes to.</param>
    [OsuNativeFunction]
    public static ErrorCode Calculate(TaikoDifficultyCalculatorHandle calcHandle, ModsCollectionHandle modsHandle,
                                      NativeTaikoDifficultyAttributes* nativeAttributesPtr)
    {
        DifficultyCalculatorContext<TaikoDifficultyCalculator> context = calcHandle.Resolve();
        Mod[] mods = modsHandle.IsNull ? [] : [.. modsHandle.Resolve().Select(x => x.ToMod(context.Ruleset))];

        TaikoDifficultyAttributes attributes = (TaikoDifficultyAttributes)context.Calculator.Calculate(mods);
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
    public static ErrorCode CalculateTimed(TaikoDifficultyCalculatorHandle calcHandle, ModsCollectionHandle modsHandle,
                                           NativeTimedTaikoDifficultyAttributes* nativeTimedAttributesBuffer, int* bufferSize)
    {
        DifficultyCalculatorContext<TaikoDifficultyCalculator> context = calcHandle.Resolve();
        Mod[] mods = modsHandle.IsNull ? [] : [.. modsHandle.Resolve().Select(x => x.ToMod(context.Ruleset))];

        if (nativeTimedAttributesBuffer is null)
        {
            *bufferSize = context.Beatmap.GetPlayableBeatmap(context.Ruleset.RulesetInfo, mods).HitObjects.Count;
            return ErrorCode.BufferSizeQuery;
        }

        List<TimedDifficultyAttributes> attributes = context.Calculator.CalculateTimed(mods);
        NativeTimedTaikoDifficultyAttributes[] nativeAttributes = [.. attributes.Select(x => new NativeTimedTaikoDifficultyAttributes(x))];

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
    [OsuNativeEnumerator<NativeTimedTaikoDifficultyAttributes>]
    public static ErrorCode CalculateTimedLazy(TaikoDifficultyCalculatorHandle calcHandle, ModsCollectionHandle modsHandle,
                                               NativeTaikoTimedDifficultyAttributesEnumeratorHandle* timedAttributesEnumeratorHandle)
    {
        DifficultyCalculatorContext<TaikoDifficultyCalculator> context = calcHandle.Resolve();
        Mod[] mods = modsHandle.IsNull ? [] : [.. modsHandle.Resolve().Select(x => x.ToMod(context.Ruleset))];

        IEnumerator<NativeTimedTaikoDifficultyAttributes> enumerator = DifficultyCalculatorHelper.CalculateTimedLazy(context.Calculator, mods)
            .Select(x => new NativeTimedTaikoDifficultyAttributes(x))
            .GetEnumerator();

        enumerator.MoveNext();

        *timedAttributesEnumeratorHandle = ManagedObjectStore.Store(enumerator);

        return ErrorCode.Success;
    }
}
