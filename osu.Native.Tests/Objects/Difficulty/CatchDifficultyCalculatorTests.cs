using osu.Game.Rulesets.Catch.Difficulty;
using osu.Native.Objects;
using osu.Native.Objects.Difficulty;
using osu.Native.Structures;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Tests.Objects.Difficulty;

[TestFixture]
internal unsafe class CatchDifficultyCalculatorTests
{
    private NativeRuleset _nativeRuleset;
    private NativeBeatmap _nativeBeatmap;

    [SetUp]
    public void Setup()
    {
        fixed (NativeRuleset* ptr = &_nativeRuleset)
            RulesetObject.CreateFromId(2, ptr);

        _nativeBeatmap = TestUtils.CreateBeatmap("beatmaps/catch/Lite Show Magic (t+pazolite vs C-Show) - Crack Traxxxx (Fatfan Kolek) [Spec's Hi-Speed Overdose].osu");
    }

    [Test]
    public void Create_ExpectedRuleset_Success()
    {
        NativeCatchDifficultyCalculator nativeDifficultyCalculator;
        ErrorCode errorCode = CatchDifficultyCalculatorObject.Create(
            _nativeRuleset.Handle, _nativeBeatmap.Handle, &nativeDifficultyCalculator);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
    }

    [Test]
    public void Create_UnexpectedRuleset_Errors()
    {
        NativeRuleset nativeRuleset;
        RulesetObject.CreateFromId(0, &nativeRuleset);

        NativeCatchDifficultyCalculator nativeDifficultyCalculator;
        ErrorCode errorCode = CatchDifficultyCalculatorObject.Create(
            nativeRuleset.Handle, _nativeBeatmap.Handle, &nativeDifficultyCalculator);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.UnexpectedRuleset));
    }

    [TestCaseSource(nameof(CalculateTestCases))]
    public void Calculate_Success(string beatmapFilename, string? mods, NativeCatchDifficultyAttributes expectedAttributes)
    {
        NativeBeatmap nativeBeatmap = TestUtils.CreateBeatmap(beatmapFilename);
        NativeModsCollection nativeModsCollection = TestUtils.CreateNativeModsCollection(mods);

        NativeCatchDifficultyCalculator nativeDifficultyCalculator;
        CatchDifficultyCalculatorObject.Create(_nativeRuleset.Handle, nativeBeatmap.Handle, &nativeDifficultyCalculator);

        NativeCatchDifficultyAttributes nativeAttributes;
        ErrorCode errorCode = CatchDifficultyCalculatorObject.Calculate(
            nativeDifficultyCalculator.Handle, nativeModsCollection.Handle, &nativeAttributes);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
        Assert.That(nativeAttributes, Is.EqualTo(expectedAttributes));
    }

    [TestCaseSource(nameof(CalculateTimedTestCases))]
    public void CalculateTimed_Success(string beatmapFilename, string? mods, int attributesIndex, NativeTimedCatchDifficultyAttributes expectedAttributes)
    {
        NativeBeatmap nativeBeatmap = TestUtils.CreateBeatmap(beatmapFilename);
        NativeModsCollection nativeModsCollection = TestUtils.CreateNativeModsCollection(mods);

        NativeCatchDifficultyCalculator nativeDifficultyCalculator;
        CatchDifficultyCalculatorObject.Create(_nativeRuleset.Handle, nativeBeatmap.Handle, &nativeDifficultyCalculator);

        int size = 0;
        CatchDifficultyCalculatorObject.CalculateTimed(nativeDifficultyCalculator.Handle, nativeModsCollection.Handle, null, &size);
        NativeTimedCatchDifficultyAttributes[] nativeAttributes = new NativeTimedCatchDifficultyAttributes[size];
        ErrorCode errorCode;
        fixed (NativeTimedCatchDifficultyAttributes* ptr = nativeAttributes)
            errorCode = CatchDifficultyCalculatorObject.CalculateTimed(nativeDifficultyCalculator.Handle, nativeModsCollection.Handle, ptr, &size);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
        Assert.That(nativeAttributes[attributesIndex], Is.EqualTo(expectedAttributes));
    }

    private static IEnumerable<TestCaseData> CalculateTestCases()
    {
        yield return new(
            "beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu",
            null,
            new NativeCatchDifficultyAttributes(new()
            {
                StarRating = 1.3280836378862895,
                MaxCombo = 310
            })
        );

        yield return new(
            "beatmaps/catch/Lite Show Magic (t+pazolite vs C-Show) - Crack Traxxxx (Fatfan Kolek) [Spec's Hi-Speed Overdose].osu",
            null,
            new NativeCatchDifficultyAttributes(new()
            {
                StarRating = 5.536300019585655,
                MaxCombo = 944
            })
        );

        yield return new(
            "beatmaps/catch/Hanatan - Airman ga Taosenai (SOUND HOLIC Ver.) (Natsu) [Zero's Overdose].osu",
            "DTFF",
            new NativeCatchDifficultyAttributes(new()
            {
                StarRating = 7.4250086916273315,
                MaxCombo = 983
            })
        );

        yield return new(
            "beatmaps/catch/Icon For Hire - Make a Move (Speed Up Ver.) (Sotarks) [Ascendance's Overdose].osu",
            "MFFL",
            new NativeCatchDifficultyAttributes(new()
            {
                StarRating = 4.609845192696199,
                MaxCombo = 262
            })
        );
    }

    private static IEnumerable<TestCaseData> CalculateTimedTestCases()
    {
        yield return new(
            "beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu",
            null,
            97,
            new NativeTimedCatchDifficultyAttributes(new(82368, new CatchDifficultyAttributes()
            {
                StarRating = 1.143108905419039,
                MaxCombo = 144
            }))
        );

        yield return new(
            "beatmaps/catch/Lite Show Magic (t+pazolite vs C-Show) - Crack Traxxxx (Fatfan Kolek) [Spec's Hi-Speed Overdose].osu",
            null,
            281,
            new NativeTimedCatchDifficultyAttributes(new(61291, new CatchDifficultyAttributes()
            {
                StarRating = 5.112422608001735,
                MaxCombo = 466
            }))
        );

        yield return new(
            "beatmaps/catch/Hanatan - Airman ga Taosenai (SOUND HOLIC Ver.) (Natsu) [Zero's Overdose].osu",
            "DTFF",
            378,
            new NativeTimedCatchDifficultyAttributes(new(101370, new CatchDifficultyAttributes()
            {
                StarRating = 7.017780409236279,
                MaxCombo = 492
            }))
        );

        yield return new(
            "beatmaps/catch/Icon For Hire - Make a Move (Speed Up Ver.) (Sotarks) [Ascendance's Overdose].osu",
            "MFFL",
            70,
            new NativeTimedCatchDifficultyAttributes(new(29316, new CatchDifficultyAttributes()
            {
                StarRating = 3.8205977060505543,
                MaxCombo = 125
            }))
        );
    }
}
