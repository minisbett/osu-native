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
