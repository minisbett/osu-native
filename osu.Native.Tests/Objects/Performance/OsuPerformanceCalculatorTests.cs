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
    private NativeBeatmap _nativeBeatmap;
    private NativeOsuPerformanceCalculator _nativePerformanceCalculator;

    [SetUp]
    public void Setup()
    {
        fixed (NativeRuleset* ptr = &_nativeRuleset)
            RulesetObject.CreateFromId(0, ptr);

        _nativeBeatmap = TestUtils.CreateBeatmap("beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu");

        fixed (NativeOsuPerformanceCalculator* ptr = &_nativePerformanceCalculator)
            OsuPerformanceCalculatorObject.Create(ptr);
    }

    [Test]
    public void Create_Success()
    {
        NativeOsuPerformanceCalculator nativePerformanceCalculator;
        ErrorCode errorCode = OsuPerformanceCalculatorObject.Create(&nativePerformanceCalculator);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
    }

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
        Assert.That(nativeAttributes, Is.EqualTo(expectedAttributes));
    }

    private static IEnumerable<TestCaseData> GetTestCases()
    {
        yield return new(
            "beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu",
            null,
            new NativeScoreInfo()
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
                Total = 37.854264515735835,
                Aim = 9.76830276704173,
                Speed = 5.004194451118395,
                Accuracy = 21.343561429982746,
                Flashlight = 0,
                EffectiveMissCount = 0,
                SpeedDeviation = 22.570196065338518,
                ComboBasedEstimatedMissCount = 0,
                ScoreBasedEstimatedMissCount = null,
                AimEstimatedSliderBreaks = 0,
                SpeedEstimatedSliderBreaks = 0
            })
        );

        yield return new(
            "beatmaps/osu/Cardboard Box - The Limit Does Not Exist (Omekyu) [lim x - U+221E].osu",
            "EZFLCL",
            new NativeScoreInfo()
            {
                MaxCombo = 549,
                Accuracy = 0.9617460317460318,
                LegacyTotalScore = null,
                CountMiss = 9,
                CountMeh = 3,
                CountOk = 43,
                CountGood = 0,
                CountGreat = 995,
                CountPerfect = 0,
                CountSmallTickMiss = 0,
                CountSmallTickHit = 0,
                CountLargeTickMiss = 0,
                CountLargeTickHit = 0,
                CountSliderTailHit = 0
            },
            new NativeOsuPerformanceAttributes(new()
            {
                Total = 299.1931642341429,
                Aim = 136.97888271541498,
                Speed = 99.73731592800607,
                Accuracy = 5.052688839946082,
                Flashlight = 47.62348841677981,
                EffectiveMissCount = 9,
                SpeedDeviation = 32.71447444011484,
                ComboBasedEstimatedMissCount = 2.758287795992714,
                ScoreBasedEstimatedMissCount = null,
                AimEstimatedSliderBreaks = 2.5157433007489667,
                SpeedEstimatedSliderBreaks = 0.3808861172140199
            })
        );

        yield return new(
            "beatmaps/osu/Bridgit Mendler, Adam Hicks, Naomi Scott and Hayley Kiyoko - Determinate (Nightcore & Cut Ver.) (My Angel Ram) [Fearless Rockstar].osu",
            "HDHR",
            new NativeScoreInfo()
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
                Total = 37.50386007312924,
                Aim = 32.10649691490753,
                Speed = 1.2100994307988802,
                Accuracy = 7.352105227014095E-08,
                Flashlight = 0,
                EffectiveMissCount = 25,
                SpeedDeviation = 50.18055630641438,
                ComboBasedEstimatedMissCount = 7.216216216216216,
                ScoreBasedEstimatedMissCount = null,
                AimEstimatedSliderBreaks = 0,
                SpeedEstimatedSliderBreaks = 0
            })
        );

        yield return new(
            "beatmaps/osu/kradness&Reol - Remote Control (Taeyang) [Max Control!].osu",
            "DT",
            new NativeScoreInfo()
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
                Total = 620.2994603384528,
                Aim = 316.59188215083833,
                Speed = 136.57334656099067,
                Accuracy = 143.1726562251678,
                Flashlight = 0,
                EffectiveMissCount = 4,
                SpeedDeviation = 10.459943261364726,
                ComboBasedEstimatedMissCount = 4,
                ScoreBasedEstimatedMissCount = null,
                AimEstimatedSliderBreaks = 0,
                SpeedEstimatedSliderBreaks = 0
            })
        );
    }
}
