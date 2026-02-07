using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Test;

public class OsuDifficultyBenchmark
{
    private uint _rulesetHandle;
    private uint _calculatorHandle;
    private NativeBeatmap _beatmap;
    private uint _modsHandle;
    private NativeOsuDifficultyAttributes _attributes;

    [GlobalSetup]
    public void Setup()
    {
        Native.Beatmap_CreateFromFile(@"C:\Users\mini\Desktop\w.osu", out _beatmap);
        Native.Ruleset_CreateFromId(0, out var ruleset);
        _rulesetHandle = ruleset.Handle;
        Native.OsuDifficultyCalculator_Create(ruleset.Handle, _beatmap.Handle, out uint diffCalcHandle);
        Native.OsuPerformanceCalculator_Create(out _calculatorHandle);
        Native.OsuDifficultyCalculator_Calculate(diffCalcHandle, 0, out _attributes);
        Native.ModsCollection_Create(out _modsHandle);
    }

    [Benchmark]
    public void CalculateDifficulty()
    {
       _ =  Native.OsuPerformanceCalculator_Calculate(_calculatorHandle, new()
        {
            BeatmapHandle = _beatmap.Handle,
            ModsHandle = _modsHandle,
            RulesetHandle = _rulesetHandle,
            Accuracy = 1,
            CountGreat = 6361,
            MaxCombo = 6368,
        }, _attributes, out _);
    }

    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<OsuDifficultyBenchmark>();
    }
}

