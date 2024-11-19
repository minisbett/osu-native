
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
