namespace osu.Native.Bindings;

public abstract class DifficultyCalculator<TDiffAttr, TPerfAttr, TScore>
    where TDiffAttr : IDifficultyAttributes
    where TPerfAttr : IPerformanceAttributes 
    where TScore : IScore
{
    protected readonly int _beatmapContextId;

    public DifficultyCalculator(FileInfo file)
    {
        OsuNative.Beatmap_CreateFromFile(file.FullName, out _beatmapContextId);
    }

    public DifficultyCalculator(string text)
    {
        OsuNative.Beatmap_CreateFromText(text, out _beatmapContextId);
    }

    ~DifficultyCalculator()
    {
        OsuNative.Beatmap_Destroy(_beatmapContextId);
    }

    public abstract TDiffAttr CalculateDifficulty(uint mods);

    public abstract TPerfAttr CalculatePerformance(TDiffAttr diffAttributes, uint mods, TScore score);
}

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

public class CatchDifficultyCalculator : DifficultyCalculator<CatchDifficultyAttributes, CatchPerformanceAttributes, CatchScore>
{
    public CatchDifficultyCalculator(FileInfo file) : base(file) { }

    public CatchDifficultyCalculator(string text) : base(text) { }

    public override CatchDifficultyAttributes CalculateDifficulty(uint mods)
    {
        OsuNative.Difficulty_ComputeCatch(_beatmapContextId, mods, out CatchDifficultyAttributes attributes);
        return attributes;
    }

    public override CatchPerformanceAttributes CalculatePerformance(CatchDifficultyAttributes diffAttributes, uint mods, CatchScore score)
    {
        int result = OsuNative.Performance_ComputeCatch(_beatmapContextId, diffAttributes, mods, score, out CatchPerformanceAttributes attributes);
        return attributes;
    }
}

public class ManiaDifficultyCalculator : DifficultyCalculator<ManiaDifficultyAttributes, ManiaPerformanceAttributes, ManiaScore>
{
    public ManiaDifficultyCalculator(FileInfo file) : base(file) { }

    public ManiaDifficultyCalculator(string text) : base(text) { }

    public override ManiaDifficultyAttributes CalculateDifficulty(uint mods)
    {
        OsuNative.Difficulty_ComputeMania(_beatmapContextId, mods, out ManiaDifficultyAttributes attributes);
        return attributes;
    }

    public override ManiaPerformanceAttributes CalculatePerformance(ManiaDifficultyAttributes diffAttributes, uint mods, ManiaScore score)
    {
        int result = OsuNative.Performance_ComputeMania(_beatmapContextId, diffAttributes, mods, score, out ManiaPerformanceAttributes attributes);
        return attributes;
    }
}

