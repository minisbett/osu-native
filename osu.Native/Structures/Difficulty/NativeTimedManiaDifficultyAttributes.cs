using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mania.Difficulty;

namespace osu.Native.Structures.Difficulty;

public struct NativeTimedManiaDifficultyAttributes(TimedDifficultyAttributes timedAttributes)
{
    public double Time = timedAttributes.Time;
    public NativeManiaDifficultyAttributes Attributes = new((ManiaDifficultyAttributes)timedAttributes.Attributes);
}
