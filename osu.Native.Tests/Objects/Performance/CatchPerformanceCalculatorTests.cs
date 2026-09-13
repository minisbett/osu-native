using osu.Native.Objects;
using osu.Native.Objects.Difficulty;
using osu.Native.Objects.Performance;
using osu.Native.Structures;
using osu.Native.Structures.Difficulty;
using osu.Native.Structures.Performance;

namespace osu.Native.Tests.Objects.Performance;

internal unsafe class CatchPerformanceCalculatorTests
{
    private NativeRuleset _nativeRuleset;
    private NativeBeatmap _nativeBeatmap;
    private NativeCatchPerformanceCalculator _nativePerformanceCalculator;

    [SetUp]
    public void Setup()
    {
        fixed (NativeRuleset* ptr = &_nativeRuleset)
            RulesetObject.CreateFromId(2, ptr);

        _nativeBeatmap = TestUtils.CreateBeatmap("beatmaps/catch/Lite Show Magic (t+pazolite vs C-Show) - Crack Traxxxx (Fatfan Kolek) [Spec's Hi-Speed Overdose].osu");

        fixed (NativeCatchPerformanceCalculator* ptr = &_nativePerformanceCalculator)
            CatchPerformanceCalculatorObject.Create(ptr);
    }

    /// <summary>
    /// Creates a performance calculator and expects Success to return.
    /// </summary>
    [Test]
    public void Create_Success()
    {
        NativeCatchPerformanceCalculator nativePerformanceCalculator;
        ErrorCode errorCode = CatchPerformanceCalculatorObject.Create(&nativePerformanceCalculator);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
    }

    /// <summary>
    /// Creates a performance calculator, performs performance calculation for the specified score and expects the attributes to match the provided ones.
    /// </summary>
    [TestCaseSource(nameof(GetTestCases))]
    public void Calculate_Success(string beatmapFilename, string? mods, NativeScoreInfo scoreInfo, NativeCatchPerformanceAttributes expectedAttributes)
    {
        NativeBeatmap nativeBeatmap = TestUtils.CreateBeatmap(beatmapFilename);
        NativeModsCollection nativeModsCollection = TestUtils.CreateNativeModsCollection(mods);

        scoreInfo.RulesetHandle = _nativeRuleset.Handle;
        scoreInfo.BeatmapHandle = nativeBeatmap.Handle;
        scoreInfo.ModsHandle = nativeModsCollection.Handle;

        NativeCatchDifficultyCalculator nativeDifficultyCalculator;
        CatchDifficultyCalculatorObject.Create(_nativeRuleset.Handle, nativeBeatmap.Handle, &nativeDifficultyCalculator);

        NativeCatchDifficultyAttributes nativeDifficultyAttributes;
        CatchDifficultyCalculatorObject.Calculate(nativeDifficultyCalculator.Handle, nativeModsCollection.Handle, &nativeDifficultyAttributes);

        NativeCatchPerformanceAttributes nativeAttributes;
        ErrorCode errorCode = CatchPerformanceCalculatorObject.Calculate(
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
                MaxCombo = 310,
                Accuracy = 1,
                LegacyTotalScore = null,
                CountMiss = 0,
                CountMeh = 0,
                CountOk = 0,
                CountGood = 0,
                CountGreat = 235,
                CountPerfect = 0,
                CountSmallTickMiss = 0,
                CountSmallTickHit = 360,
                CountLargeTickMiss = 0,
                CountLargeTickHit = 75,
                CountSliderTailHit = 0
            },
            new NativeCatchPerformanceAttributes(new()
            {
                Total = 16.62329862571729
            })
        );

        yield return new(
            "beatmaps/catch/Lite Show Magic (t+pazolite vs C-Show) - Crack Traxxxx (Fatfan Kolek) [Spec's Hi-Speed Overdose].osu",
            "DTFL",
            new NativeScoreInfo
            {
                MaxCombo = 519,
                Accuracy = 0.8962892483349191,
                LegacyTotalScore = null,
                CountMiss = 17,
                CountMeh = 0,
                CountOk = 0,
                CountGood = 0,
                CountGreat = 892,
                CountPerfect = 0,
                CountSmallTickMiss = 92,
                CountSmallTickHit = 15,
                CountLargeTickMiss = 0,
                CountLargeTickHit = 35,
                CountSliderTailHit = 0
            },
            new NativeCatchPerformanceAttributes(new()
            {
                Total = 305.6862748958652
            })
        );

        yield return new(
            "beatmaps/catch/Hanatan - Airman ga Taosenai (SOUND HOLIC Ver.) (Natsu) [Zero's Overdose].osu",
            "FFEZ",
            new NativeScoreInfo
            {
                MaxCombo = 860,
                Accuracy = 0.9738775510204082,
                LegacyTotalScore = null,
                CountMiss = 2,
                CountMeh = 0,
                CountOk = 0,
                CountGood = 0,
                CountGreat = 981,
                CountPerfect = 0,
                CountSmallTickMiss = 30,
                CountSmallTickHit = 212,
                CountLargeTickMiss = 0,
                CountLargeTickHit = 0,
                CountSliderTailHit = 0
            },
            new NativeCatchPerformanceAttributes(new()
            {
                Total = 210.8154386487999
            })
        );

        yield return new(
            "beatmaps/catch/Icon For Hire - Make a Move (Speed Up Ver.) (Sotarks) [Ascendance's Overdose].osu",
            "MF",
            new NativeScoreInfo
            {
                MaxCombo = 204,
                Accuracy = 0.7258064516129032,
                LegacyTotalScore = null,
                CountMiss = 3,
                CountMeh = 0,
                CountOk = 0,
                CountGood = 0,
                CountGreat = 236,
                CountPerfect = 0,
                CountSmallTickMiss = 99,
                CountSmallTickHit = 11,
                CountLargeTickMiss = 0,
                CountLargeTickHit = 23,
                CountSliderTailHit = 0
            },
            new NativeCatchPerformanceAttributes(new()
            {
                Total = 31.724115302973036
            })
        );
    }
}
