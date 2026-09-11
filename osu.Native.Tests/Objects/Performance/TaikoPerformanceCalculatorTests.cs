using osu.Native.Objects;
using osu.Native.Objects.Difficulty;
using osu.Native.Objects.Performance;
using osu.Native.Structures;
using osu.Native.Structures.Difficulty;
using osu.Native.Structures.Performance;

namespace osu.Native.Tests.Objects.Performance;

internal unsafe class TaikoPerformanceCalculatorTests
{
    private NativeRuleset _nativeRuleset;
    private NativeBeatmap _nativeBeatmap;
    private NativeTaikoPerformanceCalculator _nativePerformanceCalculator;

    [SetUp]
    public void Setup()
    {
        fixed (NativeRuleset* ptr = &_nativeRuleset)
            RulesetObject.CreateFromId(1, ptr);

        _nativeBeatmap = TestUtils.CreateBeatmap("beatmaps/taiko/Nanamori-chu  Goraku-bu - Happy Time wa Owaranai (eiri-) [Oni].osu");

        fixed (NativeTaikoPerformanceCalculator* ptr = &_nativePerformanceCalculator)
            TaikoPerformanceCalculatorObject.Create(ptr);
    }

    /// <summary>
    /// Creates a performance calculator and expects Success to return.
    /// </summary>
    [Test]
    public void Create_Success()
    {
        NativeOsuPerformanceCalculator nativePerformanceCalculator;
        ErrorCode errorCode = OsuPerformanceCalculatorObject.Create(&nativePerformanceCalculator);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
    }

    /// <summary>
    /// Creates a performance calculator, performs performance calculation for the specified score and expects the attributes to match the provided ones.
    /// </summary>
    [TestCaseSource(nameof(GetTestCases))]
    public void Calculate_Success(string beatmapFilename, string? mods, NativeScoreInfo scoreInfo, NativeTaikoPerformanceAttributes expectedAttributes)
    {
        NativeBeatmap nativeBeatmap = TestUtils.CreateBeatmap(beatmapFilename);
        NativeModsCollection nativeModsCollection = TestUtils.CreateNativeModsCollection(mods);

        scoreInfo.RulesetHandle = _nativeRuleset.Handle;
        scoreInfo.BeatmapHandle = nativeBeatmap.Handle;
        scoreInfo.ModsHandle = nativeModsCollection.Handle;

        NativeTaikoDifficultyCalculator nativeDifficultyCalculator;
        TaikoDifficultyCalculatorObject.Create(_nativeRuleset.Handle, nativeBeatmap.Handle, &nativeDifficultyCalculator);

        NativeTaikoDifficultyAttributes nativeDifficultyAttributes;
        TaikoDifficultyCalculatorObject.Calculate(nativeDifficultyCalculator.Handle, nativeModsCollection.Handle, &nativeDifficultyAttributes);

        NativeTaikoPerformanceAttributes nativeAttributes;
        ErrorCode errorCode = TaikoPerformanceCalculatorObject.Calculate(
            _nativePerformanceCalculator.Handle, scoreInfo, nativeDifficultyAttributes, &nativeAttributes);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
        TestUtils.AssertEqualAttributes(nativeAttributes, expectedAttributes);
    }

    private static IEnumerable<TestCaseData> GetTestCases()
    {
        yield return new(
            "beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu",
            null,
            new NativeScoreInfo
            {
                MaxCombo = 208,
                Accuracy = 1,
                LegacyTotalScore = null,
                CountMiss = 0,
                CountMeh = 0,
                CountOk = 0,
                CountGood = 0,
                CountGreat = 208,
                CountPerfect = 0,
                CountSmallTickMiss = 0,
                CountSmallTickHit = 0,
                CountLargeTickMiss = 0,
                CountLargeTickHit = 0,
                CountSliderTailHit = 0
            },
            new NativeTaikoPerformanceAttributes(new()
            {
                Difficulty = 2.149233415959476,
                Accuracy = 102.35456278369898,
                EstimatedUnstableRate = 140.90673313955597,
                Total = 104.50379619965845
            })
        );

        yield return new(
            "beatmaps/taiko/Nanamori-chu  Goraku-bu - Happy Time wa Owaranai (eiri-) [Oni].osu",
            "DT",
            new NativeScoreInfo
            {
                MaxCombo = 697,
                Accuracy = 0.9870801033591732,
                LegacyTotalScore = null,
                CountMiss = 4,
                CountMeh = 0,
                CountOk = 12,
                CountGood = 0,
                CountGreat = 758,
                CountPerfect = 0,
                CountSmallTickMiss = 0,
                CountSmallTickHit = 0,
                CountLargeTickMiss = 0,
                CountLargeTickHit = 0,
                CountSliderTailHit = 0
            },
            new NativeTaikoPerformanceAttributes(new()
            {
                Difficulty = 221.22921471940643,
                Accuracy = 178.14966960828508,
                EstimatedUnstableRate = 100.34576232908552,
                Total = 399.3788843276915
            })
        );

        yield return new(
            "beatmaps/taiko/AliA - Kakurenbo (Santi199) [From Here].osu",
            "HDEZ",
            new NativeScoreInfo
            {
                MaxCombo = 214,
                Accuracy = 0.9642121524201854,
                LegacyTotalScore = null,
                CountMiss = 37,
                CountMeh = 0,
                CountOk = 65,
                CountGood = 0,
                CountGreat = 1840,
                CountPerfect = 0,
                CountSmallTickMiss = 0,
                CountSmallTickHit = 0,
                CountLargeTickMiss = 0,
                CountLargeTickHit = 0,
                CountSliderTailHit = 0
            },
            new NativeTaikoPerformanceAttributes(new()
            {
                Difficulty = 98.57056524265009,
                Accuracy = 47.381572665252705,
                EstimatedUnstableRate = 220.0276001395029,
                Total = 145.9521379079028
            })
        );

        yield return new(
            "beatmaps/taiko/The Quick Brown Fox - The Big Black (Blue Dragon) [Ono's Taiko Oni].osu",
            "FL",
            new NativeScoreInfo
            {
                MaxCombo = 758,
                Accuracy = 0.9883843717001056,
                LegacyTotalScore = null,
                CountMiss = 11,
                CountMeh = 0,
                CountOk = 0,
                CountGood = 0,
                CountGreat = 936,
                CountPerfect = 0,
                CountSmallTickMiss = 0,
                CountSmallTickHit = 0,
                CountLargeTickMiss = 0,
                CountLargeTickHit = 0,
                CountSliderTailHit = 0
            },
            new NativeTaikoPerformanceAttributes(new()
            {
                Difficulty = 156.57682643609704,
                Accuracy = 111.16338835281029,
                EstimatedUnstableRate = 138.503757680939,
                Total = 267.74021478890734
            })
        );
    }
}
