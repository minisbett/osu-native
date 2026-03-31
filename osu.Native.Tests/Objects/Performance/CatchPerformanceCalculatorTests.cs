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

    [Test]
    public void Create_Success()
    {
        NativeOsuPerformanceCalculator nativePerformanceCalculator;
        ErrorCode errorCode = OsuPerformanceCalculatorObject.Create(&nativePerformanceCalculator);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
    }

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
        Assert.That(nativeAttributes, Is.EqualTo(expectedAttributes));
    }

    private static IEnumerable<TestCaseData> GetTestCases()
    {
        yield return new(
            "beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu",
            null,
            new NativeScoreInfo()
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
                Total = 18.924552117524012
            })
        );

        yield return new(
            "beatmaps/catch/Lite Show Magic (t+pazolite vs C-Show) - Crack Traxxxx (Fatfan Kolek) [Spec's Hi-Speed Overdose].osu",
            "DTFL",
            new NativeScoreInfo()
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
                Total = 306.481176897223
            })
        );

        yield return new(
            "beatmaps/catch/Hanatan - Airman ga Taosenai (SOUND HOLIC Ver.) (Natsu) [Zero's Overdose].osu",
            "FFEZ",
            new NativeScoreInfo()
            {
                MaxCombo = 924,
                Accuracy = 1,
                LegacyTotalScore = null,
                CountMiss = 2,
                CountMeh = 0,
                CountOk = 0,
                CountGood = 0,
                CountGreat = 979,
                CountPerfect = 0,
                CountSmallTickMiss = -2,
                CountSmallTickHit = 244,
                CountLargeTickMiss = 0,
                CountLargeTickHit = 2,
                CountSliderTailHit = 0
            },
            new NativeCatchPerformanceAttributes(new()
            {
                Total = 250.24123058175263
            })
        );

        yield return new(
            "beatmaps/catch/Icon For Hire - Make a Move (Speed Up Ver.) (Sotarks) [Ascendance's Overdose].osu",
            "MF",
            new NativeScoreInfo()
            {
                MaxCombo = 204,
                Accuracy = 0.7258064516129032,
                LegacyTotalScore = null,
                CountMiss = 3,
                CountMeh = 0,
                CountOk = 0,
                CountGood = 0,
                CountGreat = 242,
                CountPerfect = 0,
                CountSmallTickMiss = 99,
                CountSmallTickHit = 11,
                CountLargeTickMiss = 0,
                CountLargeTickHit = 17,
                CountSliderTailHit = 0
            },
            new NativeCatchPerformanceAttributes(new()
            {
                Total = 31.727829984154145
            })
        );
    }
}
