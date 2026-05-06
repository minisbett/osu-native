using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Osu.Difficulty;

namespace osu.Native.Structures.Difficulty;

public readonly struct NativeTimedOsuDifficultyAttributes(TimedDifficultyAttributes timedAttributes)
{
    public readonly double Time = timedAttributes.Time;
    public readonly NativeOsuDifficultyAttributes Attributes = new((OsuDifficultyAttributes)timedAttributes.Attributes);
}
