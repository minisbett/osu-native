using osu.Game.Rulesets.Osu.Difficulty;
using osu.Native.Objects;
using osu.Native.Objects.Difficulty;
using osu.Native.Structures;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Tests.Objects.Difficulty;

[TestFixture]
internal unsafe class OsuDifficultyCalculatorTests
{
    private NativeRuleset _nativeRuleset;
    private NativeBeatmap _nativeBeatmap;

    [SetUp]
    public void Setup()
    {
        fixed (NativeRuleset* ptr = &_nativeRuleset)
            RulesetObject.CreateFromId(0, ptr);

        _nativeBeatmap = TestUtils.CreateBeatmap("beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu");
    }

    /// <summary>
    /// Creates a difficulty calculator while providing the correct ruleset and expects Success to return.
    /// </summary>
    [Test]
    public void Create_ExpectedRuleset_Success()
    {
        NativeOsuDifficultyCalculator nativeDifficultyCalculator;
        ErrorCode errorCode = OsuDifficultyCalculatorObject.Create(
            _nativeRuleset.Handle, _nativeBeatmap.Handle, &nativeDifficultyCalculator);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
    }

    /// <summary>
    /// Creates a difficulty calculator while providing an incorrect ruleset and expects UnexpectedRuleset to return.
    /// </summary>
    [Test]
    public void Create_UnexpectedRuleset_Errors()
    {
        NativeRuleset nativeRuleset;
        RulesetObject.CreateFromId(2, &nativeRuleset);

        NativeOsuDifficultyCalculator nativeDifficultyCalculator;
        ErrorCode errorCode = OsuDifficultyCalculatorObject.Create(
            nativeRuleset.Handle, _nativeBeatmap.Handle, &nativeDifficultyCalculator);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.UnexpectedRuleset));
    }

    /// <summary>
    /// Creates a difficulty calculator, performs difficulty calculation and expects the attributes to match the provided ones.
    /// </summary>
    [TestCaseSource(nameof(CalculateTestCases))]
    public void Calculate_Success(string beatmapFilename, string? mods,
        NativeOsuDifficultyAttributes expectedAttributes)
    {
        NativeBeatmap nativeBeatmap = TestUtils.CreateBeatmap(beatmapFilename);
        NativeModsCollection nativeModsCollection = TestUtils.CreateNativeModsCollection(mods);

        NativeOsuDifficultyCalculator nativeDifficultyCalculator;
        OsuDifficultyCalculatorObject.Create(_nativeRuleset.Handle, nativeBeatmap.Handle, &nativeDifficultyCalculator);

        NativeOsuDifficultyAttributes nativeAttributes;
        ErrorCode errorCode = OsuDifficultyCalculatorObject.Calculate(
            nativeDifficultyCalculator.Handle, nativeModsCollection.Handle, &nativeAttributes);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
        TestUtils.AssertEqualAttributes(nativeAttributes, expectedAttributes);
    }

    /// <summary>
    /// Creates a difficulty calculator, performs timed difficulty calculation and expects a sample of timed attributes to match the provided ones.
    /// </summary>
    [TestCaseSource(nameof(CalculateTimedTestCases))]
    public void CalculateTimed_Success(string beatmapFilename, string? mods, int attributesIndex,
        NativeTimedOsuDifficultyAttributes expectedAttributes)
    {
        NativeBeatmap nativeBeatmap = TestUtils.CreateBeatmap(beatmapFilename);
        NativeModsCollection nativeModsCollection = TestUtils.CreateNativeModsCollection(mods);

        NativeOsuDifficultyCalculator nativeDifficultyCalculator;
        OsuDifficultyCalculatorObject.Create(_nativeRuleset.Handle, nativeBeatmap.Handle, &nativeDifficultyCalculator);

        int size = 0;
        OsuDifficultyCalculatorObject.CalculateTimed(nativeDifficultyCalculator.Handle, nativeModsCollection.Handle,
            null, &size);
        NativeTimedOsuDifficultyAttributes[] nativeAttributes = new NativeTimedOsuDifficultyAttributes[size];
        ErrorCode errorCode;
        fixed (NativeTimedOsuDifficultyAttributes* ptr = nativeAttributes)
            errorCode = OsuDifficultyCalculatorObject.CalculateTimed(nativeDifficultyCalculator.Handle,
                nativeModsCollection.Handle, ptr, &size);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
        Assert.That(nativeAttributes[attributesIndex].Time, Is.EqualTo(expectedAttributes.Time));
        TestUtils.AssertEqualAttributes(nativeAttributes[attributesIndex].Attributes, expectedAttributes.Attributes);
    }

    private static IEnumerable<TestCaseData> CalculateTestCases()
    {
        yield return new(
            "beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu",
            null,
            new NativeOsuDifficultyAttributes(new()
            {
                AimDifficulty = 1.3319102435600392,
                AimDifficultSliderCount = 18.77651922891594,
                SpeedDifficulty = 0.9312869958154305,
                SpeedNoteCount = 61.943796403379736,
                FlashlightDifficulty = 0,
                ReadingDifficulty = 0.5701522028057534,
                SliderFactor = 0.9642145971427525,
                AimTopWeightedSliderFactor = 0.18349040771438463,
                SpeedTopWeightedSliderFactor = 0.2196595610616259,
                AimDifficultStrainCount = 45.073463929706655,
                SpeedDifficultStrainCount = 48.469095507345145,
                ReadingDifficultNoteCount = 17.023947414650102,
                NestedScorePerObject = 115.4639175257732,
                LegacyScoreBaseMultiplier = 4,
                MaximumLegacyComboScore = 1416576,
                HitCircleCount = 160,
                SliderCount = 30,
                SpinnerCount = 4,
                StarRating = 2.4143080739407754,
                MaxCombo = 314
            })
        );
        
        yield return new(
            "beatmaps/osu/Cardboard Box - The Limit Does Not Exist (Omekyu) [lim x - U+221E].osu",
            "HDHR",
            new NativeOsuDifficultyAttributes(new()
            {
                AimDifficulty = 4.553729946742288,
                AimDifficultSliderCount = 210.0078707552745,
                SpeedDifficulty = 4.5266813407302955,
                SpeedNoteCount = 530.0183391691842,
                FlashlightDifficulty = 0,
                ReadingDifficulty = 2.8890386964172152,
                SliderFactor = 0.9963619897589688,
                AimTopWeightedSliderFactor = 0.31129166957284715,
                SpeedTopWeightedSliderFactor = 0.1320047924555278,
                AimDifficultStrainCount = 199.0080055251571,
                SpeedDifficultStrainCount = 315.4064355619253,
                ReadingDifficultNoteCount = 189.68581599144488,
                NestedScorePerObject = 23.523809523809526,
                LegacyScoreBaseMultiplier = 4,
                MaximumLegacyComboScore = 42080544,
                HitCircleCount = 723,
                SliderCount = 327,
                SpinnerCount = 0,
                StarRating = 9.537230951709477,
                MaxCombo = 1547
            })
        );
        
        yield return new(
            "beatmaps/osu/Bridgit Mendler, Adam Hicks, Naomi Scott and Hayley Kiyoko - Determinate (Nightcore & Cut Ver.) (My Angel Ram) [Fearless Rockstar].osu",
            "DT",
            new NativeOsuDifficultyAttributes(new()
            {
                AimDifficulty = 4.735688687609512,
                AimDifficultSliderCount = 41.198607045569204,
                SpeedDifficulty = 2.1315402941725377,
                SpeedNoteCount = 153.78306481123056,
                FlashlightDifficulty = 0,
                ReadingDifficulty = 1.8789882754140017,
                SliderFactor = 0.9937119256067164,
                AimTopWeightedSliderFactor = 0.4151729207093055,
                SpeedTopWeightedSliderFactor = 0.4356696920113319,
                AimDifficultStrainCount = 75.33924680830303,
                SpeedDifficultStrainCount = 78.69503848695882,
                ReadingDifficultNoteCount = 78.41667360257166,
                NestedScorePerObject = 20.353535353535353,
                LegacyScoreBaseMultiplier = 4,
                MaximumLegacyComboScore = 1293168,
                HitCircleCount = 132,
                SliderCount = 66,
                SpinnerCount = 0,
                StarRating = 8.077646311431128,
                MaxCombo = 267
            })
        );
        
        yield return new(
            "beatmaps/osu/kradness&Reol - Remote Control (Taeyang) [Max Control!].osu",
            "EZFL",
            new NativeOsuDifficultyAttributes(new()
            {
                AimDifficulty = 2.8637097628595543,
                AimDifficultSliderCount = 177.68434514889722,
                SpeedDifficulty = 2.2183557330771393,
                SpeedNoteCount = 231.44396759959565,
                FlashlightDifficulty = 1.9450551279140778,
                ReadingDifficulty = 2.7551630936685974,
                SliderFactor = 0.992433196544301,
                AimTopWeightedSliderFactor = 0.3135031957247166,
                SpeedTopWeightedSliderFactor = 0.41036952639501184,
                AimDifficultStrainCount = 119.82401008400586,
                SpeedDifficultStrainCount = 126.82343461787978,
                ReadingDifficultNoteCount = 154.4483231519511,
                NestedScorePerObject = 47.928388746803066,
                LegacyScoreBaseMultiplier = 5,
                MaximumLegacyComboScore = 61107960,
                HitCircleCount = 693,
                SliderCount = 479,
                SpinnerCount = 1,
                StarRating = 6.790117334909455,
                MaxCombo = 1774
            })
        );
    }

    private static IEnumerable<TestCaseData> CalculateTimedTestCases()
    {
        yield return new(
            "beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu",
            null,
            97,
            new NativeTimedOsuDifficultyAttributes(new(82368, new OsuDifficultyAttributes
            {
                AimDifficulty = 1.1488773348671546,
                AimDifficultSliderCount = 10.099078007276143,
                SpeedDifficulty = 0.8284626569614423,
                SpeedNoteCount = 32.30864020459323,
                FlashlightDifficulty = 0,
                ReadingDifficulty = 0.21049634820772292,
                SliderFactor = 0.9768929335288736,
                AimTopWeightedSliderFactor = 0.15759787948943293,
                SpeedTopWeightedSliderFactor = 0.16704212677365066,
                AimDifficultStrainCount = 30.261249136202355,
                SpeedDifficultStrainCount = 23.0290428885223,
                ReadingDifficultNoteCount = 13.153422877873599,
                NestedScorePerObject = 87.65306122448979,
                LegacyScoreBaseMultiplier = 4,
                MaximumLegacyComboScore = 327600,
                HitCircleCount = 81,
                SliderCount = 15,
                SpinnerCount = 2,
                StarRating = 2.071282125726815,
                MaxCombo = 146
            }))
        );
        
        yield return new(
            "beatmaps/osu/Cardboard Box - The Limit Does Not Exist (Omekyu) [lim x - U+221E].osu",
            "HDHR",
            525,
            new NativeTimedOsuDifficultyAttributes(new(68628, new OsuDifficultyAttributes
            {
                AimDifficulty = 4.09891509320941,
                AimDifficultSliderCount = 186.6709313930493,
                SpeedDifficulty = 3.846729209043493,
                SpeedNoteCount = 164.597722048457,
                FlashlightDifficulty = 0,
                ReadingDifficulty = 2.3753056187523196,
                SliderFactor = 0.9836973810876553,
                AimTopWeightedSliderFactor = 1.1711612420240307,
                SpeedTopWeightedSliderFactor = 0.3567119193743762,
                AimDifficultStrainCount = 154.41296528728566,
                SpeedDifficultStrainCount = 90.13937034552298,
                ReadingDifficultNoteCount = 118.75958281954377,
                NestedScorePerObject = 32.28136882129277,
                LegacyScoreBaseMultiplier = 4,
                MaximumLegacyComboScore = 11302512,
                HitCircleCount = 291,
                SliderCount = 235,
                SpinnerCount = 0,
                StarRating = 8.306260175836313,
                MaxCombo = 857
            }))
        );
        
        yield return new(
            "beatmaps/osu/Bridgit Mendler, Adam Hicks, Naomi Scott and Hayley Kiyoko - Determinate (Nightcore & Cut Ver.) (My Angel Ram) [Fearless Rockstar].osu",
            "DT",
            99,
            new NativeTimedOsuDifficultyAttributes(new(52643, new OsuDifficultyAttributes
            {
                AimDifficulty = 4.180969729032288,
                AimDifficultSliderCount = 20.78158480592368,
                SpeedDifficulty = 1.8909068168829781,
                SpeedNoteCount = 74.00384399070217,
                FlashlightDifficulty = 0,
                ReadingDifficulty = 1.64604106934218,
                SliderFactor = 0.9942053463469414,
                AimTopWeightedSliderFactor = 0.47356146310069164,
                SpeedTopWeightedSliderFactor = 0.5709326868364574,
                AimDifficultStrainCount = 46.87294463831595,
                SpeedDifficultStrainCount = 39.033871031741505,
                ReadingDifficultNoteCount = 37.81131081322172,
                NestedScorePerObject = 23.5,
                LegacyScoreBaseMultiplier = 4,
                MaximumLegacyComboScore = 343536,
                HitCircleCount = 62,
                SliderCount = 38,
                SpinnerCount = 0,
                StarRating = 7.1313590311952275,
                MaxCombo = 141
            }))
        );
        
        yield return new(
            "beatmaps/osu/kradness&Reol - Remote Control (Taeyang) [Max Control!].osu",
            "EZFL",
            586,
            new NativeTimedOsuDifficultyAttributes(new(147131.81818181818, new OsuDifficultyAttributes
            {
                AimDifficulty = 2.5200848488862726,
                AimDifficultSliderCount = 98.7411528572954,
                SpeedDifficulty = 2.0546346027975875,
                SpeedNoteCount = 152.33384848472946,
                FlashlightDifficulty = 1.3124432285292662,
                ReadingDifficulty = 2.5247707031898483,
                SliderFactor = 0.9841227216454809,
                AimTopWeightedSliderFactor = 0.5165531114369557,
                SpeedTopWeightedSliderFactor = 0.32842016840750776,
                AimDifficultStrainCount = 87.52415198587617,
                SpeedDifficultStrainCount = 82.3641132237059,
                ReadingDifficultNoteCount = 87.08011648157293,
                NestedScorePerObject = 23.867120954003408,
                LegacyScoreBaseMultiplier = 5,
                MaximumLegacyComboScore = 14666460,
                HitCircleCount = 364,
                SliderCount = 223,
                SpinnerCount = 0,
                StarRating = 5.754955474073387,
                MaxCombo = 833
            }))
        );
    }
}