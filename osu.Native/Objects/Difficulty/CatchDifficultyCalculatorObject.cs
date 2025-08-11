using osu.Game.Beatmaps;
using osu.Game.Online.API;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Catch.Difficulty;
using osu.Native.Compiler;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Objects;

internal unsafe partial class CatchDifficultyCalculatorObject : IOsuNativeObject<CatchDifficultyCalculator>
{
  [OsuNativeFunction]
  public static ErrorCode Create(ManagedObjectHandle<Ruleset> rulesetHandle, ManagedObjectHandle<FlatWorkingBeatmap> beatmapHandle,
                                 NativeCatchDifficultyCalculator* nativeCatchDifficultyCalculatorPtr)
  {
    Ruleset ruleset = rulesetHandle.Resolve();
    FlatWorkingBeatmap beatmap = beatmapHandle.Resolve();

    CatchDifficultyCalculator calculator = new(ruleset.RulesetInfo, beatmap);
    ManagedObjectHandle<CatchDifficultyCalculator> handle = ManagedObjectRegistry<CatchDifficultyCalculator>.Register(calculator);

    *nativeCatchDifficultyCalculatorPtr = new NativeCatchDifficultyCalculator()
    {
      Handle = handle
    };

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode Calculate(ManagedObjectHandle<CatchDifficultyCalculator> calcHandle, NativeCatchDifficultyAttributes* nativeAttributesPr)
  {
    Calculate(calcHandle.Resolve(), [], nativeAttributesPr);

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode CalculateMods(ManagedObjectHandle<CatchDifficultyCalculator> calcHandle, ManagedObjectHandle<Ruleset> rulesetHandle,
                                        ManagedObjectHandle<List<APIMod>> modsHandle, NativeCatchDifficultyAttributes* nativeAttributesPr)
  {
    Ruleset ruleset = rulesetHandle.Resolve();
    Mod[] mods = [.. modsHandle.Resolve().Select(x => x.ToMod(ruleset))];

    Calculate(calcHandle.Resolve(), mods, nativeAttributesPr);

    return ErrorCode.Success;
  }

  private static void Calculate(CatchDifficultyCalculator calculator, Mod[] mods, NativeCatchDifficultyAttributes* nativeAttributesPr)
  {
    CatchDifficultyAttributes attributes = (CatchDifficultyAttributes)calculator.Calculate(mods);
    *nativeAttributesPr = new NativeCatchDifficultyAttributes(attributes);
  }
}
