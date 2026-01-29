using osu.Game.Rulesets.Osu.Difficulty;

namespace osu.Native.Structures.Difficulty;

public struct NativeTimedOsuDifficultyAttributes(double time, OsuDifficultyAttributes attributes)
{
    public double Time = time;
    public NativeOsuDifficultyAttributes Attributes = new(attributes);
}
