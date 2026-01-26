using osu.Game.Rulesets.Catch.Difficulty;

namespace osu.Native.Structures.Difficulty;

public struct NativeTimedCatchDifficultyAttributes(double time, CatchDifficultyAttributes attributes)
{
    public double Time = time;
    public NativeCatchDifficultyAttributes Attributes = new(attributes);
}
