using osu.Game.Beatmaps;
using osu.Game.Online.API;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Mania.Difficulty;
using osu.Native.Compiler;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Objects;

internal unsafe partial class ManiaDifficultyCalculatorObject : IOsuNativeObject<ManiaDifficultyCalculator>
{
  [OsuNativeFunction]
  public static ErrorCode Create(ManagedObjectHandle<Ruleset> rulesetHandle, ManagedObjectHandle<FlatWorkingBeatmap> beatmapHandle,
                                 NativeManiaDifficultyCalculator* nativeManiaDifficultyCalculatorPtr)
  {
    Ruleset ruleset = rulesetHandle.Resolve();
    FlatWorkingBeatmap beatmap = beatmapHandle.Resolve();

    ManiaDifficultyCalculator calculator = new(ruleset.RulesetInfo, beatmap);
    ManagedObjectHandle<ManiaDifficultyCalculator> handle = ManagedObjectRegistry<ManiaDifficultyCalculator>.Register(calculator);

    *nativeManiaDifficultyCalculatorPtr = new NativeManiaDifficultyCalculator()
    {
      Handle = handle
    };

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode Calculate(ManagedObjectHandle<ManiaDifficultyCalculator> calcHandle, NativeManiaDifficultyAttributes* attributes)
  {
    Calculate(calcHandle.Resolve(), [], attributes);

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode CalculateMods(ManagedObjectHandle<ManiaDifficultyCalculator> calcHandle, ManagedObjectHandle<Ruleset> rulesetHandle,
                                        ManagedObjectHandle<List<APIMod>> modsHandle, NativeManiaDifficultyAttributes* attributes)
  {
    Ruleset ruleset = rulesetHandle.Resolve();
    Mod[] mods = [.. modsHandle.Resolve().Select(x => x.ToMod(ruleset))];

    Calculate(calcHandle.Resolve(), mods, attributes);

    return ErrorCode.Success;
  }

  private static void Calculate(ManiaDifficultyCalculator calculator, Mod[] mods, NativeManiaDifficultyAttributes* attributes)
  {
    ManiaDifficultyAttributes result = (ManiaDifficultyAttributes)calculator.Calculate(mods);
    *attributes = new NativeManiaDifficultyAttributes(result);
  }
}
