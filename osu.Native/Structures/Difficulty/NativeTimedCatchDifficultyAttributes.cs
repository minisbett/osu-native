using osu.Game.Rulesets.Catch.Difficulty;
using osu.Game.Rulesets.Difficulty;

namespace osu.Native.Structures.Difficulty;

public readonly struct NativeTimedCatchDifficultyAttributes(TimedDifficultyAttributes timedAttributes)
{
    public readonly double Time = timedAttributes.Time;
    public readonly NativeCatchDifficultyAttributes Attributes = new((CatchDifficultyAttributes)timedAttributes.Attributes);
}
