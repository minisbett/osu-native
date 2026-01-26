using osu.Game.Rulesets.Taiko.Difficulty;

namespace osu.Native.Structures.Difficulty;

public struct NativeTimedTaikoDifficultyAttributes(double time, TaikoDifficultyAttributes attributes)
{
    public double Time = time;
    public NativeTaikoDifficultyAttributes Attributes = new(attributes);
}
