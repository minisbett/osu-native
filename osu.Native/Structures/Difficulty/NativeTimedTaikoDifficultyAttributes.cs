using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Taiko.Difficulty;

namespace osu.Native.Structures.Difficulty;

public struct NativeTimedTaikoDifficultyAttributes(TimedDifficultyAttributes timedAttributes)
{
    public double Time = timedAttributes.Time;
    public NativeTaikoDifficultyAttributes Attributes = new((TaikoDifficultyAttributes)timedAttributes.Attributes);
}
