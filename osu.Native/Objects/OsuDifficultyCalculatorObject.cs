using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Osu.Difficulty;
using osu.Native.Compiler;
using osu.Native.Structures;

namespace osu.Native.Objects;

internal unsafe partial class OsuDifficultyCalculatorObject : IOsuNativeObject<OsuDifficultyCalculator>
{
  [OsuNativeFunction]
  public static ErrorCode Create(NativeRuleset nativeRuleset, NativeBeatmap nativeBeatmap, NativeOsuDifficultyCalculator* nativeOsuDifficultyCalculatorPtr)
  {
    Ruleset ruleset = nativeRuleset.Resolve();
    FlatWorkingBeatmap beatmap = nativeBeatmap.Resolve();
    OsuDifficultyCalculator calculator = new(ruleset.RulesetInfo, beatmap);

    int objectId = ObjectContainer<OsuDifficultyCalculator>.Add(calculator);
    
    *nativeOsuDifficultyCalculatorPtr = new NativeOsuDifficultyCalculator()
    {
      ObjectId = objectId
    };

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode Calculate(NativeOsuDifficultyCalculator nativeOsuDifficultyCalculator, NativeOsuDifficultyAttributes* attributes)
  {
    OsuDifficultyCalculator calculator = nativeOsuDifficultyCalculator.Resolve();

    OsuDifficultyAttributes result = (OsuDifficultyAttributes)calculator.Calculate();
    *attributes = new NativeOsuDifficultyAttributes(result);

    return ErrorCode.Success;
  }
}
