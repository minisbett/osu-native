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

    [Test]
    public void Create_ExpectedRuleset_Success()
    {
        NativeOsuDifficultyCalculator nativeDifficultyCalculator;
        ErrorCode errorCode = OsuDifficultyCalculatorObject.Create(
            _nativeRuleset.Handle, _nativeBeatmap.Handle, &nativeDifficultyCalculator);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
    }

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

    [TestCaseSource(nameof(CalculateTestCases))]
    public void Calculate_Success(string beatmapFilename, string? mods, NativeOsuDifficultyAttributes expectedAttributes)
    {
        NativeBeatmap nativeBeatmap = TestUtils.CreateBeatmap(beatmapFilename);
        NativeModsCollection nativeModsCollection = TestUtils.CreateNativeModsCollection(mods);

        NativeOsuDifficultyCalculator nativeDifficultyCalculator;
        OsuDifficultyCalculatorObject.Create(_nativeRuleset.Handle, nativeBeatmap.Handle, &nativeDifficultyCalculator);

        NativeOsuDifficultyAttributes nativeAttributes;
        ErrorCode errorCode = OsuDifficultyCalculatorObject.Calculate(
            nativeDifficultyCalculator.Handle, nativeModsCollection.Handle, &nativeAttributes);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
        Assert.That(nativeAttributes, Is.EqualTo(expectedAttributes));
    }

    [TestCaseSource(nameof(CalculateTimedTestCases))]
    public void CalculateTimed_Success(string beatmapFilename, string? mods, int attributesIndex, NativeTimedOsuDifficultyAttributes expectedAttributes)
    {
        NativeBeatmap nativeBeatmap = TestUtils.CreateBeatmap(beatmapFilename);
        NativeModsCollection nativeModsCollection = TestUtils.CreateNativeModsCollection(mods);

        NativeOsuDifficultyCalculator nativeDifficultyCalculator;
        OsuDifficultyCalculatorObject.Create(_nativeRuleset.Handle, nativeBeatmap.Handle, &nativeDifficultyCalculator);

        int size = 0;
        OsuDifficultyCalculatorObject.CalculateTimed(nativeDifficultyCalculator.Handle, nativeModsCollection.Handle, null, &size);
        NativeTimedOsuDifficultyAttributes[] nativeAttributes = new NativeTimedOsuDifficultyAttributes[size];
        ErrorCode errorCode;
        fixed (NativeTimedOsuDifficultyAttributes* ptr = nativeAttributes)
            errorCode = OsuDifficultyCalculatorObject.CalculateTimed(nativeDifficultyCalculator.Handle, nativeModsCollection.Handle, ptr, &size);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
        Assert.That(nativeAttributes[attributesIndex], Is.EqualTo(expectedAttributes));
    }

    private static IEnumerable<TestCaseData> CalculateTestCases()
    {
        yield return new(
            "beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu",
            null,
            new NativeOsuDifficultyAttributes(new()
            {
                StarRating = 2.6269498953955925,
                MaxCombo = 314,
                AimDifficulty = 1.3985304840104509,
                AimDifficultSliderCount = 11.074653391431971,
                SpeedDifficulty = 1.1298267572899297,
                SpeedNoteCount = 83.85579111772137,
                FlashlightDifficulty = 0,
                SliderFactor = 0.9733515644468542,
                AimTopWeightedSliderFactor = 0.16627711236296208,
                SpeedTopWeightedSliderFactor = 0.251355083543094,
                AimDifficultStrainCount = 36.39277304261741,
                SpeedDifficultStrainCount = 36.58417925576565,
                NestedScorePerObject = 115.4639175257732,
                LegacyScoreBaseMultiplier = 4,
                MaximumLegacyComboScore = 1416576,
                DrainRate = 6,
                HitCircleCount = 160,
                SliderCount = 30,
                SpinnerCount = 4
            })
        );

        yield return new(
            "beatmaps/osu/Cardboard Box - The Limit Does Not Exist (Omekyu) [lim x - U+221E].osu",
            "HRHD",
            new NativeOsuDifficultyAttributes(new()
            {
                StarRating = 9.474923684013236,
                MaxCombo = 1547,
                AimDifficulty = 4.512804471827263,
                AimDifficultSliderCount = 218.4431214276953,
                SpeedDifficulty = 4.722779656977584,
                SpeedNoteCount = 454.0703585369717,
                FlashlightDifficulty = 0,
                SliderFactor = 0.9913857987909955,
                AimTopWeightedSliderFactor = 0.2032120579053057,
                SpeedTopWeightedSliderFactor = 0.11079764662813427,
                AimDifficultStrainCount = 197.47716332452504,
                SpeedDifficultStrainCount = 231.23686025160228,
                NestedScorePerObject = 23.523809523809526,
                LegacyScoreBaseMultiplier = 5,
                MaximumLegacyComboScore = 42080544,
                DrainRate = 5.599999904632568,
                HitCircleCount = 723,
                SliderCount = 327,
                SpinnerCount = 0
            })
        );

        yield return new(
            "beatmaps/osu/Bridgit Mendler, Adam Hicks, Naomi Scott and Hayley Kiyoko - Determinate (Nightcore & Cut Ver.) (My Angel Ram) [Fearless Rockstar].osu",
            "DT",
            new NativeOsuDifficultyAttributes(new()
            {
                StarRating = 8.325912502004124,
                MaxCombo = 267,
                AimDifficulty = 4.722356007610924,
                AimDifficultSliderCount = 34.000149522102646,
                SpeedDifficulty = 2.951886559818673,
                SpeedNoteCount = 149.5487196306236,
                FlashlightDifficulty = 0,
                SliderFactor = 0.9934874916205908,
                AimTopWeightedSliderFactor = 0.4331784078164127,
                SpeedTopWeightedSliderFactor = 0.4359688770016273,
                AimDifficultStrainCount = 64.24755507518815,
                SpeedDifficultStrainCount = 72.65345279407778,
                NestedScorePerObject = 20.353535353535353,
                LegacyScoreBaseMultiplier = 4,
                MaximumLegacyComboScore = 1293168,
                DrainRate = 5,
                HitCircleCount = 132,
                SliderCount = 66,
                SpinnerCount = 0
            })
        );

        yield return new(
            "beatmaps/osu/kradness&Reol - Remote Control (Taeyang) [Max Control!].osu",
            "EZFL",
            new NativeOsuDifficultyAttributes(new()
            {
                StarRating = 6.217650101667546,
                MaxCombo = 1774,
                AimDifficulty = 2.9251267451559255,
                AimDifficultSliderCount = 123.46283726466135,
                SpeedDifficulty = 2.107421220228791,
                SpeedNoteCount = 365.58004278300075,
                FlashlightDifficulty = 1.894058240433308,
                SliderFactor = 0.9978743821004423,
                AimTopWeightedSliderFactor = 0.2718079929528663,
                SpeedTopWeightedSliderFactor = 0.3013197261499631,
                AimDifficultStrainCount = 89.87964386694276,
                SpeedDifficultStrainCount = 115.449209584869,
                NestedScorePerObject = 47.928388746803066,
                LegacyScoreBaseMultiplier = 3,
                MaximumLegacyComboScore = 61107960,
                DrainRate = 3.25,
                HitCircleCount = 693,
                SliderCount = 479,
                SpinnerCount = 1
            })
        );
    }

    private static IEnumerable<TestCaseData> CalculateTimedTestCases()
    {
        yield return new(
            "beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu",
            null,
            97,
            new NativeTimedOsuDifficultyAttributes(new(82368, new OsuDifficultyAttributes()
            {
                StarRating = 2.322986708074882,
                MaxCombo = 146,
                AimDifficulty = 1.2240760089684632,
                AimDifficultSliderCount = 5.927815799050609,
                SpeedDifficulty = 1.0192639296286623,
                SpeedNoteCount = 35.9953966003706,
                FlashlightDifficulty = 0,
                SliderFactor = 0.9722669093150396,
                AimTopWeightedSliderFactor = 0.177818687951171,
                SpeedTopWeightedSliderFactor = 0.1761405813205129,
                AimDifficultStrainCount = 22.642471620306925,
                SpeedDifficultStrainCount = 22.445119472710314,
                NestedScorePerObject = 87.65306122448979,
                LegacyScoreBaseMultiplier = 4,
                MaximumLegacyComboScore = 327600,
                DrainRate = 6,
                HitCircleCount = 81,
                SliderCount = 15,
                SpinnerCount = 2
            }))
        );

        yield return new(
            "beatmaps/osu/Cardboard Box - The Limit Does Not Exist (Omekyu) [lim x - U+221E].osu",
            "HRHD",
            525,
            new NativeTimedOsuDifficultyAttributes(new(68628, new OsuDifficultyAttributes()
            {
                StarRating = 8.26719418585876,
                MaxCombo = 857,
                AimDifficulty = 4.016530753310711,
                AimDifficultSliderCount = 164.7655871057952,
                SpeedDifficulty = 4.046544832926176,
                SpeedNoteCount = 179.37206569210772,
                FlashlightDifficulty = 0,
                SliderFactor = 0.9810044854489924,
                AimTopWeightedSliderFactor = 1.046032170471003,
                SpeedTopWeightedSliderFactor = 0.37765406787645023,
                AimDifficultStrainCount = 145.59280443204628,
                SpeedDifficultStrainCount = 80.01448285147679,
                NestedScorePerObject = 32.28136882129277,
                LegacyScoreBaseMultiplier = 5,
                MaximumLegacyComboScore = 11302512,
                DrainRate = 5.599999904632568,
                HitCircleCount = 291,
                SliderCount = 235,
                SpinnerCount = 0
            }))
        );

        yield return new(
            "beatmaps/osu/Bridgit Mendler, Adam Hicks, Naomi Scott and Hayley Kiyoko - Determinate (Nightcore & Cut Ver.) (My Angel Ram) [Fearless Rockstar].osu",
            "DT",
            99,
            new NativeTimedOsuDifficultyAttributes(new(52643, new OsuDifficultyAttributes()
            {
                StarRating = 7.4834248180501595,
                MaxCombo = 141,
                AimDifficulty = 4.23398552909992,
                AimDifficultSliderCount = 16.813469572637004,
                SpeedDifficulty = 2.6830682368098024,
                SpeedNoteCount = 69.00674647936822,
                FlashlightDifficulty = 0,
                SliderFactor = 0.9887893456375428,
                AimTopWeightedSliderFactor = 0.485601108852943,
                SpeedTopWeightedSliderFactor = 0.5531040265470156,
                AimDifficultStrainCount = 41.49055787527263,
                SpeedDifficultStrainCount = 44.1020381896742,
                NestedScorePerObject = 23.5,
                LegacyScoreBaseMultiplier = 4,
                MaximumLegacyComboScore = 343536,
                DrainRate = 5,
                HitCircleCount = 62,
                SliderCount = 38,
                SpinnerCount = 0
            }))
        );

        yield return new(
            "beatmaps/osu/kradness&Reol - Remote Control (Taeyang) [Max Control!].osu",
            "EZFL",
            586,
            new NativeTimedOsuDifficultyAttributes(new(147131.81818181818, new OsuDifficultyAttributes()
            {
                StarRating = 5.3352111227460375,
                MaxCombo = 833,
                AimDifficulty = 2.5880861302750824,
                AimDifficultSliderCount = 66.23259026381031,
                SpeedDifficulty = 2.0446132016066256,
                SpeedNoteCount = 194.574748613241,
                FlashlightDifficulty = 1.2747775350768742,
                SliderFactor = 0.9942995663295898,
                AimTopWeightedSliderFactor = 0.46214609351160063,
                SpeedTopWeightedSliderFactor = 0.2911920597504217,
                AimDifficultStrainCount = 61.61880558361183,
                SpeedDifficultStrainCount = 79.57322283233718,
                NestedScorePerObject = 23.867120954003408,
                LegacyScoreBaseMultiplier = 3,
                MaximumLegacyComboScore = 14666460,
                DrainRate = 3.25,
                HitCircleCount = 364,
                SliderCount = 223,
                SpinnerCount = 0
            }))
        );
    }
}
