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

    private static void Calculate(ManiaDifficultyCalculator calculator, Mod[] mods, NativeManiaDifficultyAttributes* nativeAttributesPtr)
    {
        ManiaDifficultyAttributes attributes = (ManiaDifficultyAttributes)calculator.Calculate(mods);
        *nativeAttributesPtr = new(attributes);
    }

    /// <summary>
    /// Calculates the difficulty attributes of the beatmap targetted by the specified difficulty calculator.
    /// </summary>
    /// <param name="calcHandle">The handle of the difficulty calculator.</param>
    /// <param name="nativeAttributesPtr">A pointer to write the resulting difficulty attributes to.</param>
    [OsuNativeFunction]
    public static ErrorCode Calculate(ManiaDifficultyCalculatorHandle calcHandle, NativeManiaDifficultyAttributes* nativeAttributesPtr)
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
    public static ErrorCode CalculateMods(ManiaDifficultyCalculatorHandle calcHandle, ModsCollectionHandle modsHandle,
                                          NativeManiaDifficultyAttributes* nativeAttributesPtr)
    {
        DifficultyCalculatorContext<ManiaDifficultyCalculator> context = calcHandle.Resolve();
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
    public static ErrorCode CalculateTimed(ManiaDifficultyCalculatorHandle calcHandle, NativeTimedManiaDifficultyAttributes* nativeTimedAttributesBuffer,
                                           int* bufferSize)
    {
        DifficultyCalculatorContext<ManiaDifficultyCalculator> context = calcHandle.Resolve();

        if (nativeTimedAttributesBuffer is null)
        {
            *bufferSize = context.Beatmap.GetPlayableBeatmap(context.Ruleset.RulesetInfo).HitObjects.Count;
            return ErrorCode.BufferSizeQuery;
        }

        List<TimedDifficultyAttributes> attributes = context.Calculator.CalculateTimed();
        NativeTimedManiaDifficultyAttributes[] nativeAttributes = [..attributes.Select(
            x => new NativeTimedManiaDifficultyAttributes(x.Time, (ManiaDifficultyAttributes)x.Attributes))];

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
    public static ErrorCode CalculateModsTimed(ManiaDifficultyCalculatorHandle calcHandle, ModsCollectionHandle modsHandle,
                                               NativeTimedManiaDifficultyAttributes* nativeTimedAttributesBuffer, int* bufferSize)
    {
        DifficultyCalculatorContext<ManiaDifficultyCalculator> context = calcHandle.Resolve();

        Mod[] mods = [.. modsHandle.Resolve().Select(x => x.ToMod(context.Ruleset))];

        if (nativeTimedAttributesBuffer is null)
        {
            *bufferSize = context.Beatmap.GetPlayableBeatmap(context.Ruleset.RulesetInfo).HitObjects.Count;
            return ErrorCode.BufferSizeQuery;
        }

        List<TimedDifficultyAttributes> attributes = context.Calculator.CalculateTimed(mods);
        NativeTimedManiaDifficultyAttributes[] nativeAttributes = [..attributes.Select(
            x => new NativeTimedManiaDifficultyAttributes(x.Time, (ManiaDifficultyAttributes)x.Attributes))];

        BufferHelper.Write(nativeAttributes, nativeTimedAttributesBuffer, bufferSize);
        return ErrorCode.Success;
    }
}
