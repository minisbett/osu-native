using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Taiko;
using osu.Game.Rulesets.Taiko.Difficulty;
using osu.Native.Compiler;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Objects;

/// <summary>
/// Represents the difficulty calculator for the Taiko ruleset (<see cref="TaikoDifficultyCalculator"/>).
/// </summary>
internal unsafe partial class TaikoDifficultyCalculatorObject : IOsuNativeObject<TaikoDifficultyCalculator>
{
    /// <summary>
    /// Creates an instance of a <see cref="TaikoDifficultyCalculator"/> for the specified ruleset and beatmap.
    /// </summary>
    /// <param name="rulesetHandle">The handle of the ruleset passed into the difficulty calculator.</param>
    /// <param name="beatmapHandle">The handle of the beatmap the difficulty calculator targets.</param>
    /// <param name="nativeTaikoDifficultyCalculatorPtr">A pointer to write the resulting native difficulty calculator object to.</param>
    [OsuNativeFunction]
    public static ErrorCode Create(ManagedObjectHandle<Ruleset> rulesetHandle, ManagedObjectHandle<FlatWorkingBeatmap> beatmapHandle,
                                   NativeTaikoDifficultyCalculator* nativeTaikoDifficultyCalculatorPtr)
    {
        Ruleset ruleset = rulesetHandle.Resolve();
        FlatWorkingBeatmap beatmap = beatmapHandle.Resolve();

        if (ruleset is not TaikoRuleset)
            return ErrorCode.UnexpectedRuleset;

        TaikoDifficultyCalculator calculator = (TaikoDifficultyCalculator)ruleset.CreateDifficultyCalculator(beatmap);

        *nativeTaikoDifficultyCalculatorPtr = new NativeTaikoDifficultyCalculator { Handle = ManagedObjectStore.Store(calculator) };

        return ErrorCode.Success;
    }

    /// <summary>
    /// Calculates the difficulty attributes of the beatmap targetted by the specified difficulty calculator.
    /// </summary>
    /// <param name="calcHandle">The handle of the difficulty calculator.</param>
    /// <param name="nativeAttributesPtr">A pointer to write the resulting difficulty attributes to.</param>
    [OsuNativeFunction]
    public static ErrorCode Calculate(ManagedObjectHandle<TaikoDifficultyCalculator> calcHandle, NativeTaikoDifficultyAttributes* nativeAttributesPtr)
    {
        Calculate(calcHandle.Resolve(), [], nativeAttributesPtr);

        return ErrorCode.Success;
    }

    /// <summary>
    /// Calculates the difficulty attributes, including the specified mods, of the beatmap targetted by the specified difficulty calculator.
    /// </summary>
    /// <param name="calcHandle">The handle of the difficulty calculator.</param>
    /// <param name="rulesetHandle">The handle of the ruleset used to instantiate the mods.</param>
    /// <param name="modsHandle">The handle of the mods collection to consider.</param>
    /// <param name="nativeAttributesPtr">A pointer to write the resulting difficulty attributes to.</param>
    [OsuNativeFunction]
    public static ErrorCode CalculateMods(ManagedObjectHandle<TaikoDifficultyCalculator> calcHandle, ManagedObjectHandle<Ruleset> rulesetHandle,
                                          ManagedObjectHandle<ModsCollection> modsHandle, NativeTaikoDifficultyAttributes* nativeAttributesPtr)
    {
        Ruleset ruleset = rulesetHandle.Resolve();

        if (ruleset is not TaikoRuleset)
            return ErrorCode.UnexpectedRuleset;

        Mod[] mods = [.. modsHandle.Resolve().Select(x => x.ToMod(ruleset))];
        Calculate(calcHandle.Resolve(), mods, nativeAttributesPtr);

        return ErrorCode.Success;
    }

    private static void Calculate(TaikoDifficultyCalculator calculator, Mod[] mods, NativeTaikoDifficultyAttributes* nativeAttributesPtr)
    {
        TaikoDifficultyAttributes attributes = (TaikoDifficultyAttributes)calculator.Calculate(mods);
        *nativeAttributesPtr = new NativeTaikoDifficultyAttributes(attributes);
    }

    /// <summary>
    /// Calculates the timed (per-object) difficulty attributes of the beatmap targetted by the specified calculator.
    /// </summary>
    /// <param name="calcHandle">The handle of the difficulty calculator.</param>
    /// <param name="nativeTimedAttributesBuffer">A pointer to write the resulting timed difficulty attributes to.</param>
    /// <param name="bufferSize">The size of the provided buffer.</param>
    [OsuNativeFunction]
    public static ErrorCode CalculateTimed(ManagedObjectHandle<TaikoDifficultyCalculator> calcHandle,
                                           NativeTimedTaikoDifficultyAttributes* nativeTimedAttributesBuffer, int* bufferSize)
    {
        TaikoDifficultyCalculator calculator = calcHandle.Resolve();

        if (nativeTimedAttributesBuffer is null)
        {
            *bufferSize = calculator.CalculateTimed().Count;
            return ErrorCode.BufferSizeQuery;
        }

        List<TimedDifficultyAttributes> attributes = calculator.CalculateTimed();
        NativeTimedTaikoDifficultyAttributes[] nativeAttributes = [..attributes.Select(
            x => new NativeTimedTaikoDifficultyAttributes(x.Time, (TaikoDifficultyAttributes)x.Attributes))];

        BufferHelper.Write(nativeAttributes, nativeTimedAttributesBuffer, bufferSize);
        return ErrorCode.Success;
    }

    /// <summary>
    /// Calculates the timed (per-object) difficulty attributes, including the specified mods, of the beatmap targetted by the specified calculator.
    /// </summary>
    /// <param name="calcHandle">The handle of the difficulty calculator.</param>
    /// <param name="rulesetHandle">The handle of the ruleset used to instantiate the mods.</param>
    /// <param name="modsHandle">The handle of the mods collection to consider.</param>
    /// <param name="nativeTimedAttributesBuffer">A pointer to write the resulting timed difficulty attributes to.</param>
    /// <param name="bufferSize">The size of the provided buffer.</param>
    [OsuNativeFunction]
    public static ErrorCode CalculateModsTimed(ManagedObjectHandle<TaikoDifficultyCalculator> calcHandle, ManagedObjectHandle<Ruleset> rulesetHandle,
                                               ManagedObjectHandle<ModsCollection> modsHandle, NativeTimedTaikoDifficultyAttributes* nativeTimedAttributesBuffer,
                                               int* bufferSize)
    {
        TaikoDifficultyCalculator calculator = calcHandle.Resolve();
        Ruleset ruleset = rulesetHandle.Resolve();

        if (nativeTimedAttributesBuffer is null)
        {
            *bufferSize = calculator.CalculateTimed().Count;
            return ErrorCode.BufferSizeQuery;
        }

        if (ruleset is not OsuRuleset)
            return ErrorCode.UnexpectedRuleset;

        Mod[] mods = [.. modsHandle.Resolve().Select(x => x.ToMod(ruleset))];

        List<TimedDifficultyAttributes> attributes = calculator.CalculateTimed(mods);
        NativeTimedTaikoDifficultyAttributes[] nativeAttributes = [..attributes.Select(
            x => new NativeTimedTaikoDifficultyAttributes(x.Time, (TaikoDifficultyAttributes)x.Attributes))];

        BufferHelper.Write(nativeAttributes, nativeTimedAttributesBuffer, bufferSize);
        return ErrorCode.Success;
    }
}
