using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Test;

public class OsuDifficultyBenchmark
{
    private int _rulesetHandle;
    private int _calculatorHandle;
    private List<int> _modCollectionHandles = [];

    [GlobalSetup]
    public void Setup()
    {
        Native.Beatmap_CreateFromFile(@"C:\Users\mini\Desktop\w.osu", out var beatmap);
        Native.Ruleset_CreateFromId(0, out var ruleset);
        _rulesetHandle = ruleset.Handle;
        Native.OsuDifficultyCalculator_Create(ruleset.Handle, beatmap.Handle, out _calculatorHandle);

        foreach (string combination in new string[] { "HR", "FL", "DT", "HT", "HRDT", "HRHT", "DTFL", "HRDTFL", "HRHTFL" })
        {
            List<int> modHandles = [];
            foreach (string acronym in combination.Chunk(2).Select(x => new string(x)))
            {
                Native.Mod_Create(acronym, out var modHandle);
                modHandles.Add(modHandle);
            }

            Native.ModsCollection_Create(out var modCollectionHandle);
            foreach(int modHandle in modHandles)
            {
                Native.ModsCollection_Add(modCollectionHandle, modHandle);
            }

            _modCollectionHandles.Add(modCollectionHandle);
        }
    }

    [Benchmark]
    public void CalculateDifficulty()
    {
        Native.OsuDifficultyCalculator_Calculate(_calculatorHandle, out _);

        foreach (int modCollectionHandle in _modCollectionHandles)
        {
            Native.OsuDifficultyCalculator_CalculateMods(_calculatorHandle, _rulesetHandle, modCollectionHandle, out _);
        }
    }

    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<OsuDifficultyBenchmark>();
    }
}

