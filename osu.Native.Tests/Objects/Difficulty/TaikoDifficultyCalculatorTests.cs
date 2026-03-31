using osu.Game.Rulesets.Osu.Difficulty;
using osu.Game.Rulesets.Taiko.Difficulty;
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

    [TestCaseSource(nameof(CalculateTestCases))]
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

    [TestCaseSource(nameof(CalculateTimedTestCases))]
    public void CalculateTimed_Success(string beatmapFilename, string? mods, int attributesIndex, NativeTimedTaikoDifficultyAttributes expectedAttributes)
    {
        NativeBeatmap nativeBeatmap = TestUtils.CreateBeatmap(beatmapFilename);
        NativeModsCollection nativeModsCollection = TestUtils.CreateNativeModsCollection(mods);

        NativeTaikoDifficultyCalculator nativeDifficultyCalculator;
        TaikoDifficultyCalculatorObject.Create(_nativeRuleset.Handle, nativeBeatmap.Handle, &nativeDifficultyCalculator);

        int size = 0;
        TaikoDifficultyCalculatorObject.CalculateTimed(nativeDifficultyCalculator.Handle, nativeModsCollection.Handle, null, &size);
        NativeTimedTaikoDifficultyAttributes[] nativeAttributes = new NativeTimedTaikoDifficultyAttributes[size];
        ErrorCode errorCode;
        fixed (NativeTimedTaikoDifficultyAttributes* ptr = nativeAttributes)
            errorCode = TaikoDifficultyCalculatorObject.CalculateTimed(nativeDifficultyCalculator.Handle, nativeModsCollection.Handle, ptr, &size);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
        Assert.That(nativeAttributes[attributesIndex], Is.EqualTo(expectedAttributes));
    }

    private static IEnumerable<TestCaseData> CalculateTestCases()
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

    private static IEnumerable<TestCaseData> CalculateTimedTestCases()
    {
        yield return new(
            "beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu",
            null,
            112,
            new NativeTimedTaikoDifficultyAttributes(new(81368, new TaikoDifficultyAttributes()
            {
                StarRating = 1.1098320129641495,
                MaxCombo = 106,
                MechanicalDifficulty = 1.0604533327053272,
                RhythmDifficulty = 0.04937868007513404,
                ReadingDifficulty = 1.836883991158528E-10,
                ColourDifficulty = 0.19860377525560924,
                StaminaDifficulty = 0.8618495574497179,
                MonoStaminaFactor = 0.7720121953149288,
                ConsistencyFactor = 0.6708255013231751,
                StaminaTopStrains = 32.34253680705717
            }))
        );

        yield return new(
            "beatmaps/taiko/Nanamori-chu  Goraku-bu - Happy Time wa Owaranai (eiri-) [Oni].osu",
            null,
            387,
            new NativeTimedTaikoDifficultyAttributes(new(69687, new TaikoDifficultyAttributes()
            {
                StarRating = 4.11242111015801,
                MaxCombo = 388,
                MechanicalDifficulty = 3.175040354229718,
                RhythmDifficulty = 0.9373792794516127,
                ReadingDifficulty = 1.4764766786072943E-06,
                ColourDifficulty = 0.9882162567571725,
                StaminaDifficulty = 2.1868240974725457,
                MonoStaminaFactor = 7.869817236005582E-09,
                ConsistencyFactor = 0.7997705233931964,
                StaminaTopStrains = 165.398224897263
            }))
        );

        yield return new(
            "beatmaps/taiko/AliA - Kakurenbo (Santi199) [From Here].osu",
            "HDDT",
            971,
            new NativeTimedTaikoDifficultyAttributes(new(145410, new TaikoDifficultyAttributes()
            {
                StarRating = 6.770099393569792,
                MaxCombo = 972,
                MechanicalDifficulty = 5.04386048822962,
                RhythmDifficulty = 1.4174179017169288,
                ReadingDifficulty = 0.3088210036232424,
                ColourDifficulty = 1.4432878759657388,
                StaminaDifficulty = 3.600572612263881,
                MonoStaminaFactor = 1.703595911720861E-08,
                ConsistencyFactor = 0.7443737731512136,
                StaminaTopStrains = 244.09359807054483
            }))
        );

        yield return new(
            "beatmaps/taiko/The Quick Brown Fox - The Big Black (Blue Dragon) [Ono's Taiko Oni].osu",
            "FLSR",
            474,
            new NativeTimedTaikoDifficultyAttributes(new(73077, new TaikoDifficultyAttributes()
            {
                StarRating = 5.03445225816122,
                MaxCombo = 475,
                MechanicalDifficulty = 4.194497362649164,
                RhythmDifficulty = 0.8399543162218412,
                ReadingDifficulty = 5.792902144577381E-07,
                ColourDifficulty = 1.3248447829121202,
                StaminaDifficulty = 2.869652579737044,
                MonoStaminaFactor = 8.47015536456639E-05,
                ConsistencyFactor = 0.7405942248649898,
                StaminaTopStrains = 170.27077455498798
            }))
        );
    }
}