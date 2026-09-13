using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mania.Difficulty;

namespace osu.Native.Structures.Difficulty;

public readonly struct NativeTimedManiaDifficultyAttributes(TimedDifficultyAttributes timedAttributes)
{
    public readonly double Time = timedAttributes.Time;
    public readonly NativeManiaDifficultyAttributes Attributes = new((ManiaDifficultyAttributes)timedAttributes.Attributes);
}
