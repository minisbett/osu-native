using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Taiko.Difficulty;

namespace osu.Native.Structures.Difficulty;

public readonly struct NativeTimedTaikoDifficultyAttributes(TimedDifficultyAttributes timedAttributes)
{
    public readonly double Time = timedAttributes.Time;
    public readonly NativeTaikoDifficultyAttributes Attributes = new((TaikoDifficultyAttributes)timedAttributes.Attributes);
}
