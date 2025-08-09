using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Osu.Difficulty;
using osu.Native.Compiler;
using osu.Native.Structures;

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
  public static ErrorCode Calculate(ManagedObjectHandle<OsuDifficultyCalculator> calculatorHandle, NativeOsuDifficultyAttributes* attributes)
  {
    OsuDifficultyCalculator calculator = calculatorHandle.Resolve();

    OsuDifficultyAttributes result = (OsuDifficultyAttributes)calculator.Calculate();
    *attributes = new NativeOsuDifficultyAttributes(result);

    return ErrorCode.Success;
  }
}
