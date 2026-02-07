using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Osu.Difficulty;

namespace osu.Native.Structures.Difficulty;

public struct NativeTimedOsuDifficultyAttributes(TimedDifficultyAttributes timedAttributes)
{
    public double Time = timedAttributes.Time;
    public NativeOsuDifficultyAttributes Attributes = new((OsuDifficultyAttributes)timedAttributes.Attributes);
}
