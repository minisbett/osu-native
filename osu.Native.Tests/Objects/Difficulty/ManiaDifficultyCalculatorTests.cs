using osu.Native.Objects;
using osu.Native.Objects.Difficulty;
using osu.Native.Structures;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Tests.Objects.Difficulty;

[TestFixture]
internal unsafe class ManiaDifficultyCalculatorTests
{
    private NativeRuleset _nativeRuleset;
    private NativeBeatmap _nativeBeatmap;

    [SetUp]
    public void Setup()
    {
        fixed (NativeRuleset* ptr = &_nativeRuleset)
            RulesetObject.CreateFromId(3, ptr);

        _nativeBeatmap = TestUtils.CreateBeatmap("beatmaps/mania/xi - FREEDOM DiVE (razlteh) [Blocko's 7K Black Another].osu");
    }

    [Test]
    public void Create_ExpectedRuleset_Success()
    {
        NativeManiaDifficultyCalculator nativeDifficultyCalculator;
        ErrorCode errorCode = ManiaDifficultyCalculatorObject.Create(
            _nativeRuleset.Handle, _nativeBeatmap.Handle, &nativeDifficultyCalculator);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
    }

    [Test]
    public void Create_UnexpectedRuleset_Errors()
    {
        NativeRuleset nativeRuleset;
        RulesetObject.CreateFromId(1, &nativeRuleset);

        NativeManiaDifficultyCalculator nativeDifficultyCalculator;
        ErrorCode errorCode = ManiaDifficultyCalculatorObject.Create(
            nativeRuleset.Handle, _nativeBeatmap.Handle, &nativeDifficultyCalculator);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.UnexpectedRuleset));
    }

    [TestCaseSource(nameof(GetTestCases))]
    public void Calculate_Success(string beatmapFilename, string? mods, NativeManiaDifficultyAttributes expectedAttributes)
    {
        NativeBeatmap nativeBeatmap = TestUtils.CreateBeatmap(beatmapFilename);
        NativeModsCollection nativeModsCollection = TestUtils.CreateNativeModsCollection(mods);

        NativeManiaDifficultyCalculator nativeDifficultyCalculator;
        ManiaDifficultyCalculatorObject.Create(_nativeRuleset.Handle, nativeBeatmap.Handle, &nativeDifficultyCalculator);

        NativeManiaDifficultyAttributes nativeAttributes;
        ErrorCode errorCode = ManiaDifficultyCalculatorObject.Calculate(
            nativeDifficultyCalculator.Handle, nativeModsCollection.Handle, &nativeAttributes);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
        Assert.That(nativeAttributes, Is.EqualTo(expectedAttributes));
    }

    private static IEnumerable<TestCaseData> GetTestCases()
    {
        yield return new(
           "beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu",
           null,
           new NativeManiaDifficultyAttributes(new()
           {
               StarRating = 1.9374325665272143,
               MaxCombo = 761
           })
       );

        yield return new(
            "beatmaps/mania/xi - FREEDOM DiVE (razlteh) [Blocko's 7K Black Another].osu",
            "HDHR",
            new NativeManiaDifficultyAttributes(new()
            {
                StarRating = 8.343351334085787,
                MaxCombo = 7496
            })
        );

        yield return new(
            "beatmaps/mania/MYTH & ROID - STYX HELIX (Tsukuyomi) [victorica's Hard].osu",
            "DTFI",
            new NativeManiaDifficultyAttributes(new()
            {
                StarRating = 4.102620621879934,
                MaxCombo = 1087
            })
        );

        yield return new(
            "beatmaps/mania/Soleily - Renatus (ExPew) [Another].osu",
            "IN",
            new NativeManiaDifficultyAttributes(new()
            {
                StarRating = 6.998209417646642,
                MaxCombo = 14842
            })
        );
    }
}
