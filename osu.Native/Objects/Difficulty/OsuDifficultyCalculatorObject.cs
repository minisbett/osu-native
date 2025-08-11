using osu.Game.Beatmaps;
using osu.Game.Online.API;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu.Difficulty;
using osu.Native.Compiler;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Objects;

internal unsafe partial class OsuDifficultyCalculatorObject : IOsuNativeObject<OsuDifficultyCalculator>
{
  [OsuNativeFunction]
  public static ErrorCode Create(ManagedObjectHandle<Ruleset> rulesetHandle, ManagedObjectHandle<FlatWorkingBeatmap> beatmapHandle,
                                 NativeOsuDifficultyCalculator* nativeOsuDifficultyCalculatorPtr)
  {
    Ruleset ruleset = rulesetHandle.Resolve();
    FlatWorkingBeatmap beatmap = beatmapHandle.Resolve();

    OsuDifficultyCalculator calculator = new(ruleset.RulesetInfo, beatmap);
    ManagedObjectHandle<OsuDifficultyCalculator> handle = ManagedObjectRegistry<OsuDifficultyCalculator>.Register(calculator);

    *nativeOsuDifficultyCalculatorPtr = new NativeOsuDifficultyCalculator()
    {
      Handle = handle
    };

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode Calculate(ManagedObjectHandle<OsuDifficultyCalculator> calcHandle, NativeOsuDifficultyAttributes* nativeAttributesPtr)
  {
    Calculate(calcHandle.Resolve(), [], nativeAttributesPtr);

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode CalculateMods(ManagedObjectHandle<OsuDifficultyCalculator> calcHandle, ManagedObjectHandle<Ruleset> rulesetHandle,
                                        ManagedObjectHandle<List<APIMod>> modsHandle, NativeOsuDifficultyAttributes* nativeAttributesPtr)
  {
    Ruleset ruleset = rulesetHandle.Resolve();
    Mod[] mods = [.. modsHandle.Resolve().Select(x => x.ToMod(ruleset))];

    Calculate(calcHandle.Resolve(), mods, nativeAttributesPtr);

    return ErrorCode.Success;
  }

  private static void Calculate(OsuDifficultyCalculator calculator, Mod[] mods, NativeOsuDifficultyAttributes* nativeAttributesPtr)
  {
    OsuDifficultyAttributes attributes = (OsuDifficultyAttributes)calculator.Calculate(mods);
    *nativeAttributesPtr = new NativeOsuDifficultyAttributes(attributes);
  }
}
