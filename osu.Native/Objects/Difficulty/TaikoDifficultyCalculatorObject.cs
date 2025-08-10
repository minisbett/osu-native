using osu.Game.Beatmaps;
using osu.Game.Online.API;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Taiko.Difficulty;
using osu.Native.Compiler;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Objects;

internal unsafe partial class TaikoDifficultyCalculatorObject : IOsuNativeObject<TaikoDifficultyCalculator>
{
  [OsuNativeFunction]
  public static ErrorCode Create(ManagedObjectHandle<Ruleset> rulesetHandle, ManagedObjectHandle<FlatWorkingBeatmap> beatmapHandle,
                                 NativeTaikoDifficultyCalculator* nativeTaikoDifficultyCalculatorPtr)
  {
    Ruleset ruleset = rulesetHandle.Resolve();
    FlatWorkingBeatmap beatmap = beatmapHandle.Resolve();

    TaikoDifficultyCalculator calculator = new(ruleset.RulesetInfo, beatmap);
    ManagedObjectHandle<TaikoDifficultyCalculator> handle = ManagedObjectRegistry<TaikoDifficultyCalculator>.Register(calculator);

    *nativeTaikoDifficultyCalculatorPtr = new NativeTaikoDifficultyCalculator()
    {
      Handle = handle
    };

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode Calculate(ManagedObjectHandle<TaikoDifficultyCalculator> calcHandle, NativeTaikoDifficultyAttributes* attributes)
  {
    Calculate(calcHandle.Resolve(), [], attributes);

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode CalculateMods(ManagedObjectHandle<TaikoDifficultyCalculator> calcHandle, ManagedObjectHandle<Ruleset> rulesetHandle,
                                        ManagedObjectHandle<List<APIMod>> modsHandle, NativeTaikoDifficultyAttributes* attributes)
  {
    Ruleset ruleset = rulesetHandle.Resolve();
    Mod[] mods = [.. modsHandle.Resolve().Select(x => x.ToMod(ruleset))];

    Calculate(calcHandle.Resolve(), mods, attributes);

    return ErrorCode.Success;
  }

  private static void Calculate(TaikoDifficultyCalculator calculator, Mod[] mods, NativeTaikoDifficultyAttributes* attributes)
  {
    TaikoDifficultyAttributes result = (TaikoDifficultyAttributes)calculator.Calculate(mods);
    *attributes = new NativeTaikoDifficultyAttributes(result);
  }
}
