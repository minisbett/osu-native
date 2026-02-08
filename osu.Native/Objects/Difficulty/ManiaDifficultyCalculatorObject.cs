using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Mania.Difficulty;
using osu.Native.Compiler;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Objects.Difficulty;

/// <summary>
/// Represents a <see cref="ManiaDifficultyCalculator"/>.
/// </summary>
internal unsafe partial class ManiaDifficultyCalculatorObject : IOsuNativeObject<DifficultyCalculatorContext<ManiaDifficultyCalculator>>
{
    /// <summary>
    /// Creates an instance of a <see cref="ManiaDifficultyCalculator"/> for the specified ruleset and beatmap.
    /// </summary>
    /// <param name="rulesetHandle">The handle of the ruleset passed into the difficulty calculator.</param>
    /// <param name="beatmapHandle">The handle of the beatmap the difficulty calculator targets.</param>
    /// <param name="nativeManiaDifficultyCalculatorPtr">A pointer to write the resulting native difficulty calculator object to.</param>
    [OsuNativeFunction]
    public static ErrorCode Create(RulesetHandle rulesetHandle, BeatmapHandle beatmapHandle,
                                   NativeManiaDifficultyCalculator* nativeManiaDifficultyCalculatorPtr)
    {
        Ruleset ruleset = rulesetHandle.Resolve();
        FlatWorkingBeatmap beatmap = beatmapHandle.Resolve();

        if (ruleset is not ManiaRuleset)
            return ErrorCode.UnexpectedRuleset;

        ManiaDifficultyCalculator calculator = (ManiaDifficultyCalculator)ruleset.CreateDifficultyCalculator(beatmap);
        DifficultyCalculatorContext<ManiaDifficultyCalculator> context = new(ruleset, beatmap, calculator);

        *nativeManiaDifficultyCalculatorPtr = new() { Handle = ManagedObjectStore.Store(context) };

        return ErrorCode.Success;
    }

    /// <summary>
    /// Calculates the difficulty attributes of the beatmap targetted by the specified difficulty calculator.
    /// </summary>
    /// <param name="calcHandle">The handle of the difficulty calculator.</param>
    /// <param name="modsHandle">The handle of the mods collection to consider. A null-handle equals to an empty mods collection.</param>
    /// <param name="nativeAttributesPtr">A pointer to write the resulting difficulty attributes to.</param>
    [OsuNativeFunction]
    public static ErrorCode Calculate(ManiaDifficultyCalculatorHandle calcHandle, ModsCollectionHandle modsHandle,
                                      NativeManiaDifficultyAttributes* nativeAttributesPtr)
    {
        DifficultyCalculatorContext<ManiaDifficultyCalculator> context = calcHandle.Resolve();
        Mod[] mods = modsHandle.IsNull ? [] : [.. modsHandle.Resolve().Select(x => x.ToMod(context.Ruleset))];

        ManiaDifficultyAttributes attributes = (ManiaDifficultyAttributes)context.Calculator.Calculate(mods);
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
    public static ErrorCode CalculateTimed(ManiaDifficultyCalculatorHandle calcHandle, ModsCollectionHandle modsHandle,
                                           NativeTimedManiaDifficultyAttributes* nativeTimedAttributesBuffer, int* bufferSize)
    {
        DifficultyCalculatorContext<ManiaDifficultyCalculator> context = calcHandle.Resolve();
        Mod[] mods = modsHandle.IsNull ? [] : [.. modsHandle.Resolve().Select(x => x.ToMod(context.Ruleset))];

        if (nativeTimedAttributesBuffer is null)
        {
            *bufferSize = context.Beatmap.GetPlayableBeatmap(context.Ruleset.RulesetInfo, mods).HitObjects.Count;
            return ErrorCode.BufferSizeQuery;
        }

        List<TimedDifficultyAttributes> attributes = context.Calculator.CalculateTimed(mods);
        NativeTimedManiaDifficultyAttributes[] nativeAttributes = [.. attributes.Select(x => new NativeTimedManiaDifficultyAttributes(x))];

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
    [OsuNativeEnumerator<NativeTimedManiaDifficultyAttributes>]
    public static ErrorCode CalculateTimedLazy(ManiaDifficultyCalculatorHandle calcHandle, ModsCollectionHandle modsHandle,
                                               NativeManiaTimedDifficultyAttributesEnumeratorHandle* timedAttributesEnumeratorHandle)
    {
        DifficultyCalculatorContext<ManiaDifficultyCalculator> context = calcHandle.Resolve();
        Mod[] mods = modsHandle.IsNull ? [] : [.. modsHandle.Resolve().Select(x => x.ToMod(context.Ruleset))];

        IEnumerator<NativeTimedManiaDifficultyAttributes> enumerator = LazyDifficultyCalculationHelper.CalculateTimedLazy(context.Calculator, mods)
            .Select(x => new NativeTimedManiaDifficultyAttributes(x))
            .GetEnumerator();

        enumerator.MoveNext();

        *timedAttributesEnumeratorHandle = ManagedObjectStore.Store(enumerator);

        return ErrorCode.Success;
    }
}
