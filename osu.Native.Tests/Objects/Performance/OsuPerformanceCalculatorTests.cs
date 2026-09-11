using osu.Native.Objects;
using osu.Native.Objects.Difficulty;
using osu.Native.Objects.Performance;
using osu.Native.Structures;
using osu.Native.Structures.Difficulty;
using osu.Native.Structures.Performance;

namespace osu.Native.Tests.Objects.Performance;

internal unsafe class OsuPerformanceCalculatorTests
{
    private NativeRuleset _nativeRuleset;
    private NativeOsuPerformanceCalculator _nativePerformanceCalculator;

    [SetUp]
    public void Setup()
    {
        fixed (NativeRuleset* ptr = &_nativeRuleset)
            RulesetObject.CreateFromId(0, ptr);

        fixed (NativeOsuPerformanceCalculator* ptr = &_nativePerformanceCalculator)
            OsuPerformanceCalculatorObject.Create(ptr);
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
    public void Calculate_Success(string beatmapFilename, string? mods, NativeScoreInfo scoreInfo, NativeOsuPerformanceAttributes expectedAttributes)
    {
        NativeBeatmap nativeBeatmap = TestUtils.CreateBeatmap(beatmapFilename);
        NativeModsCollection nativeModsCollection = TestUtils.CreateNativeModsCollection(mods);

        scoreInfo.RulesetHandle = _nativeRuleset.Handle;
        scoreInfo.BeatmapHandle = nativeBeatmap.Handle;
        scoreInfo.ModsHandle = nativeModsCollection.Handle;

        NativeOsuDifficultyCalculator nativeDifficultyCalculator;
        OsuDifficultyCalculatorObject.Create(_nativeRuleset.Handle, nativeBeatmap.Handle, &nativeDifficultyCalculator);

        NativeOsuDifficultyAttributes nativeDifficultyAttributes;
        OsuDifficultyCalculatorObject.Calculate(nativeDifficultyCalculator.Handle, nativeModsCollection.Handle, &nativeDifficultyAttributes);

        NativeOsuPerformanceAttributes nativeAttributes;
        ErrorCode errorCode = OsuPerformanceCalculatorObject.Calculate(
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
                MaxCombo = 314,
                Accuracy = 1,
                LegacyTotalScore = null,
                CountMiss = 0,
                CountMeh = 0,
                CountOk = 0,
                CountGood = 0,
                CountGreat = 194,
                CountPerfect = 0,
                CountSmallTickMiss = 0,
                CountSmallTickHit = 0,
                CountLargeTickMiss = 0,
                CountLargeTickHit = 0,
                CountSliderTailHit = 30
            },
            new NativeOsuPerformanceAttributes(new()
            {
                Aim = 9.29946359268675,
                Speed = 2.9323792232775427,
                Accuracy = 21.343561429982746,
                Flashlight = 0,
                Reading = 0.7413655667665224,
                EffectiveMissCount = 0,
                SpeedDeviation = 23.74860356438467,
                ComboBasedEstimatedMissCount = 0,
                ScoreBasedEstimatedMissCount = null,
                AimEstimatedSliderBreaks = 0,
                SpeedEstimatedSliderBreaks = 0,
                Total = 35.363845024126974
            })
        );

        yield return new(
            "beatmaps/osu/Cardboard Box - The Limit Does Not Exist (Omekyu) [lim x - U+221E].osu",
            "EZFLCL",
            new NativeScoreInfo
            {
                MaxCombo = 1547,
                Accuracy = 1,
                LegacyTotalScore = null,
                CountMiss = 0,
                CountMeh = 0,
                CountOk = 0,
                CountGood = 0,
                CountGreat = 1050,
                CountPerfect = 0,
                CountSmallTickMiss = 0,
                CountSmallTickHit = 0,
                CountLargeTickMiss = 0,
                CountLargeTickHit = 0,
                CountSliderTailHit = 0
            },
            new NativeOsuPerformanceAttributes(new()
            {
                Aim = 211.784971998384,
                Speed = 210.44593126648613,
                Accuracy = 19.529377102872083,
                Flashlight = 143.53904092265503,
                Reading = 251.68379722774023,
                EffectiveMissCount = 0,
                SpeedDeviation = 19.432773175416827,
                ComboBasedEstimatedMissCount = 0,
                ScoreBasedEstimatedMissCount = null,
                AimEstimatedSliderBreaks = 0,
                SpeedEstimatedSliderBreaks = 0,
                Total = 765.091920982053
            })
        );

        yield return new(
            "beatmaps/osu/Bridgit Mendler, Adam Hicks, Naomi Scott and Hayley Kiyoko - Determinate (Nightcore & Cut Ver.) (My Angel Ram) [Fearless Rockstar].osu",
            "HDHR",
            new NativeScoreInfo
            {
                MaxCombo = 37,
                Accuracy = 0.4970456838161118,
                LegacyTotalScore = null,
                CountMiss = 25,
                CountMeh = 32,
                CountOk = 97,
                CountGood = 0,
                CountGreat = 44,
                CountPerfect = 0,
                CountSmallTickMiss = 0,
                CountSmallTickHit = 0,
                CountLargeTickMiss = 0,
                CountLargeTickHit = 0,
                CountSliderTailHit = 66
            },
            new NativeOsuPerformanceAttributes(new()
            {
                Aim = 36.57304050612379,
                Speed = 1.5551585446011267,
                Accuracy = 6.807504839827866E-08,
                Flashlight = 0,
                Reading = 0.680396622666621,
                EffectiveMissCount = 25,
                SpeedDeviation = 49.94571431733021,
                ComboBasedEstimatedMissCount = 7.216216216216216,
                ScoreBasedEstimatedMissCount = null,
                AimEstimatedSliderBreaks = 0,
                SpeedEstimatedSliderBreaks = 0,
                Total = 42.57843292093474
            })
        );

        yield return new(
            "beatmaps/osu/kradness&Reol - Remote Control (Taeyang) [Max Control!].osu",
            "DT",
            new NativeScoreInfo
            {
                MaxCombo = 1774,
                Accuracy = 0.9831543482838492,
                LegacyTotalScore = null,
                CountMiss = 4,
                CountMeh = 0,
                CountOk = 30,
                CountGood = 0,
                CountGreat = 1139,
                CountPerfect = 0,
                CountSmallTickMiss = 0,
                CountSmallTickHit = 0,
                CountLargeTickMiss = 0,
                CountLargeTickHit = 0,
                CountSliderTailHit = 479
            },
            new NativeOsuPerformanceAttributes(new()
            {
                Aim = 336.3926458790663,
                Speed = 129.20212118918835,
                Accuracy = 138.69939285506217,
                Flashlight = 0,
                Reading = 32.455047260983264,
                EffectiveMissCount = 4,
                SpeedDeviation = 11.235483126825962,
                ComboBasedEstimatedMissCount = 4,
                ScoreBasedEstimatedMissCount = null,
                AimEstimatedSliderBreaks = 0,
                SpeedEstimatedSliderBreaks = 0,
                Total = 643.7736537825398
            })
        );
    }
}
