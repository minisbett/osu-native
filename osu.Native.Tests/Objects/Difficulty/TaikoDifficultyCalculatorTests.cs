using osu.Native.Objects;
using osu.Native.Objects.Difficulty;
using osu.Native.Structures;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Tests.Objects.Difficulty;

[TestFixture]
internal unsafe class TaikoDifficultyCalculatorTests
{
    private NativeRuleset _nativeRuleset;
    private NativeBeatmap _nativeBeatmap;

    [SetUp]
    public void Setup()
    {
        fixed (NativeRuleset* ptr = &_nativeRuleset)
            RulesetObject.CreateFromId(1, ptr);

        _nativeBeatmap = TestUtils.CreateBeatmap("beatmaps/taiko/Nanamori-chu  Goraku-bu - Happy Time wa Owaranai (eiri-) [Oni].osu");
    }

    [Test]
    public void Create_ExpectedRuleset_Success()
    {
        NativeTaikoDifficultyCalculator nativeDifficultyCalculator;
        ErrorCode errorCode = TaikoDifficultyCalculatorObject.Create(
            _nativeRuleset.Handle, _nativeBeatmap.Handle, &nativeDifficultyCalculator);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
    }

    [Test]
    public void Create_UnexpectedRuleset_Errors()
    {
        NativeRuleset nativeRuleset;
        RulesetObject.CreateFromId(3, &nativeRuleset);

        NativeTaikoDifficultyCalculator nativeDifficultyCalculator;
        ErrorCode errorCode = TaikoDifficultyCalculatorObject.Create(
            nativeRuleset.Handle, _nativeBeatmap.Handle, &nativeDifficultyCalculator);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.UnexpectedRuleset));
    }

    [TestCaseSource(nameof(GetTestCases))]
    public void Calculate_Success(string beatmapFilename, string? mods, NativeTaikoDifficultyAttributes expectedAttributes)
    {
        NativeBeatmap nativeBeatmap = TestUtils.CreateBeatmap(beatmapFilename);
        NativeModsCollection nativeModsCollection = TestUtils.CreateNativeModsCollection(mods);

        NativeTaikoDifficultyCalculator nativeDifficultyCalculator;
        TaikoDifficultyCalculatorObject.Create(_nativeRuleset.Handle, nativeBeatmap.Handle, &nativeDifficultyCalculator);

        NativeTaikoDifficultyAttributes nativeAttributes;
        ErrorCode errorCode = TaikoDifficultyCalculatorObject.Calculate(
            nativeDifficultyCalculator.Handle, nativeModsCollection.Handle, &nativeAttributes);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
        Assert.That(nativeAttributes, Is.EqualTo(expectedAttributes));
    }

    private static IEnumerable<TestCaseData> GetTestCases()
    {
        yield return new(
            "beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu",
            null,
            new NativeTaikoDifficultyAttributes(new()
            {
                StarRating = 1.259462252424754,
                MaxCombo = 208,
                MechanicalDifficulty = 1.1948490180889453,
                RhythmDifficulty = 0.06461323414087576,
                ReadingDifficulty = 1.9493318819594794E-10,
                ColourDifficulty = 0.2844333630538491,
                StaminaDifficulty = 0.9104156550350961,
                MonoStaminaFactor = 0.6825722747762478,
                ConsistencyFactor = 0.6537250059396112,
                StaminaTopStrains = 53.323583838377736
            })
        );

        yield return new(
            "beatmaps/taiko/Nanamori-chu  Goraku-bu - Happy Time wa Owaranai (eiri-) [Oni].osu",
            null,
            new NativeTaikoDifficultyAttributes(new()
            {
                StarRating = 4.130892918957952,
                MaxCombo = 774,
                MechanicalDifficulty = 3.1640976153391107,
                RhythmDifficulty = 0.9667938417334558,
                ReadingDifficulty = 1.4618853857505597E-06,
                ColourDifficulty = 0.9990222042585569,
                StaminaDifficulty = 2.165075411080554,
                MonoStaminaFactor = 3.153914838605005E-08,
                ConsistencyFactor = 0.8121443501643035,
                StaminaTopStrains = 321.9080028367804
            })
        );

        yield return new(
            "beatmaps/taiko/AliA - Kakurenbo (Santi199) [From Here].osu",
            "HDDT",
            new NativeTaikoDifficultyAttributes(new()
            {
                StarRating = 6.943511873960143,
                MaxCombo = 1942,
                MechanicalDifficulty = 4.8936150001660215,
                RhythmDifficulty = 1.343464224059641,
                ReadingDifficulty = 0.7064326497344819,
                ColourDifficulty = 1.3780350267674881,
                StaminaDifficulty = 3.5155799733985336,
                MonoStaminaFactor = 2.5868877078195933E-08,
                ConsistencyFactor = 0.7427894855621978,
                StaminaTopStrains = 465.8521322338041
            })
        );

        yield return new(
            "beatmaps/taiko/The Quick Brown Fox - The Big Black (Blue Dragon) [Ono's Taiko Oni].osu",
            "FLSR",
            new NativeTaikoDifficultyAttributes(new()
            {
                StarRating = 5.202212137303396,
                MaxCombo = 947,
                MechanicalDifficulty = 4.218862417547102,
                RhythmDifficulty = 0.983349131269575,
                ReadingDifficulty = 5.884867184406646E-07,
                ColourDifficulty = 1.3184081938427739,
                StaminaDifficulty = 2.9004542237043287,
                MonoStaminaFactor = 0.00033412449222804705,
                ConsistencyFactor = 0.7365465128799001,
                StaminaTopStrains = 304.45808068546506
            })
        );
    }
}
