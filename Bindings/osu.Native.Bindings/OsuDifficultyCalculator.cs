
using System.Runtime.InteropServices;

namespace osu.Native.Bindings;

public class OsuDifficultyCalculator : DifficultyCalculator<OsuDifficultyAttributes, OsuPerformanceAttributes, OsuScore>
{
    public OsuDifficultyCalculator(FileInfo file) : base(file) { }

    public OsuDifficultyCalculator(string text) : base(text) { }

    public override OsuDifficultyAttributes CalculateDifficulty(uint mods)
    {
        OsuNative.Difficulty_ComputeOsu(_beatmapContextId, mods, out OsuDifficultyAttributes attributes);
        return attributes;
    }

    public override OsuPerformanceAttributes CalculatePerformance(OsuDifficultyAttributes diffAttributes, uint mods, OsuScore score)
    {
        OsuNative.Performance_ComputeOsu(_beatmapContextId, diffAttributes, mods, score, out OsuPerformanceAttributes attributes);
        return attributes;
    }
}

public class TaikoDifficultyCalculator : DifficultyCalculator<TaikoDifficultyAttributes, TaikoPerformanceAttributes, TaikoScore>
{
    public TaikoDifficultyCalculator(FileInfo file) : base(file) { }

    public TaikoDifficultyCalculator(string text) : base(text) { }

    public override TaikoDifficultyAttributes CalculateDifficulty(uint mods)
    {
        OsuNative.Difficulty_ComputeTaiko(_beatmapContextId, mods, out TaikoDifficultyAttributes attributes);
        return attributes;
    }

    public override TaikoPerformanceAttributes CalculatePerformance(TaikoDifficultyAttributes diffAttributes, uint mods, TaikoScore score)
    {
        int result = OsuNative.Performance_ComputeTaiko(_beatmapContextId, diffAttributes, mods, score, out TaikoPerformanceAttributes attributes);
        return attributes;
    }
}
