using osu.Game.Rulesets.Catch.Difficulty;
using osu.Game.Rulesets.Difficulty;

namespace osu.Native.Structures.Difficulty;

public struct NativeTimedCatchDifficultyAttributes(TimedDifficultyAttributes timedAttributes)
{
    public double Time = timedAttributes.Time;
    public NativeCatchDifficultyAttributes Attributes = new((CatchDifficultyAttributes)timedAttributes.Attributes);
}
