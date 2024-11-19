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

    public abstract TDiffAttr CalculateDifficulty();

    public abstract TPerfAttr CalculatePerformance(TDiffAttr diffAttributes, TScore score);
}

public class OsuDifficultyCalculator : DifficultyCalculator<OsuDifficultyAttributes, OsuPerformanceAttributes, OsuScore>
{
    public OsuDifficultyCalculator(FileInfo file) : base(file) { }

    public OsuDifficultyCalculator(string text) : base(text) { }

    public override OsuDifficultyAttributes CalculateDifficulty()
    {
        OsuNative.Difficulty_ComputeOsu(_beatmapContextId, out OsuDifficultyAttributes attributes);
        return attributes;
    }

    public override unsafe OsuPerformanceAttributes CalculatePerformance(OsuDifficultyAttributes diffAttributes, OsuScore score)
    {
        OsuNative.Performance_ComputeOsu(_beatmapContextId, diffAttributes, score, out OsuPerformanceAttributes attributes);
        return attributes;
    }
}

public class TaikoDifficultyCalculator : DifficultyCalculator<TaikoDifficultyAttributes, TaikoPerformanceAttributes, TaikoScore>
{
    public TaikoDifficultyCalculator(FileInfo file) : base(file) { }

    public TaikoDifficultyCalculator(string text) : base(text) { }

    public override TaikoDifficultyAttributes CalculateDifficulty()
    {
        OsuNative.Difficulty_ComputeTaiko(_beatmapContextId, out TaikoDifficultyAttributes attributes);
        return attributes;
    }

    public override TaikoPerformanceAttributes CalculatePerformance(TaikoDifficultyAttributes diffAttributes, TaikoScore score)
    {
        OsuNative.Performance_ComputeTaiko(_beatmapContextId, diffAttributes, score, out TaikoPerformanceAttributes attributes);
        return attributes;
    }
}

public class CatchDifficultyCalculator : DifficultyCalculator<CatchDifficultyAttributes, CatchPerformanceAttributes, CatchScore>
{
    public CatchDifficultyCalculator(FileInfo file) : base(file) { }

    public CatchDifficultyCalculator(string text) : base(text) { }

    public override CatchDifficultyAttributes CalculateDifficulty()
    {
        OsuNative.Difficulty_ComputeCatch(_beatmapContextId, out CatchDifficultyAttributes attributes);
        return attributes;
    }

    public override CatchPerformanceAttributes CalculatePerformance(CatchDifficultyAttributes diffAttributes, CatchScore score)
    {
        OsuNative.Performance_ComputeCatch(_beatmapContextId, diffAttributes, score, out CatchPerformanceAttributes attributes);
        return attributes;
    }
}

public class ManiaDifficultyCalculator : DifficultyCalculator<ManiaDifficultyAttributes, ManiaPerformanceAttributes, ManiaScore>
{
    public ManiaDifficultyCalculator(FileInfo file) : base(file) { }

    public ManiaDifficultyCalculator(string text) : base(text) { }

    public override ManiaDifficultyAttributes CalculateDifficulty()
    {
        OsuNative.Difficulty_ComputeMania(_beatmapContextId, out ManiaDifficultyAttributes attributes);
        return attributes;
    }

    public override ManiaPerformanceAttributes CalculatePerformance(ManiaDifficultyAttributes diffAttributes, ManiaScore score)
    {
        OsuNative.Performance_ComputeMania(_beatmapContextId, diffAttributes, score, out ManiaPerformanceAttributes attributes);
        return attributes;
    }
}

