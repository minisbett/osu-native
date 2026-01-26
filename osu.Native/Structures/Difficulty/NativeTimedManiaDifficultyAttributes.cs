using osu.Game.Rulesets.Mania.Difficulty;

namespace osu.Native.Structures.Difficulty;

public struct NativeTimedManiaDifficultyAttributes(double time, ManiaDifficultyAttributes attributes)
{
    public double Time = time;
    public NativeManiaDifficultyAttributes Attributes = new(attributes);
}
