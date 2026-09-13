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

    /// <summary>
    /// Creates a difficulty calculator while providing the correct ruleset and expects Success to return.
    /// </summary>
    [Test]
    public void Create_ExpectedRuleset_Success()
    {
        NativeTaikoDifficultyCalculator nativeDifficultyCalculator;
        ErrorCode errorCode = TaikoDifficultyCalculatorObject.Create(
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
        RulesetObject.CreateFromId(3, &nativeRuleset);

        NativeTaikoDifficultyCalculator nativeDifficultyCalculator;
        ErrorCode errorCode = TaikoDifficultyCalculatorObject.Create(
            nativeRuleset.Handle, _nativeBeatmap.Handle, &nativeDifficultyCalculator);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.UnexpectedRuleset));
    }

    /// <summary>
    /// Creates a difficulty calculator, performs difficulty calculation and expects the attributes to match the provided ones.
    /// </summary>
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
        TestUtils.AssertEqualAttributes(nativeAttributes, expectedAttributes);
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
        Assert.That(nativeAttributes[attributesIndex].Time, Is.EqualTo(expectedAttributes.Time));
        TestUtils.AssertEqualAttributes(nativeAttributes[attributesIndex].Attributes, expectedAttributes.Attributes);
    }

    private static IEnumerable<TestCaseData> CalculateTestCases()
    {
        yield return new(
            "beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu",
            null,
            new NativeTaikoDifficultyAttributes(new()
            {
                MechanicalDifficulty = 1.2050570070107374,
                RhythmDifficulty = 0.05302630146558138,
                ReadingDifficulty = 1.965985666625734E-10,
                ColourDifficulty = 0.28686337100890075,
                StaminaDifficulty = 0.9181936360018367,
                MonoStaminaFactor = 0.6825722747762477,
                ConsistencyFactor = 0.6539601881692547,
                StaminaTopStrains = 53.32358383837777,
                StarRating = 1.2580833086729175,
                MaxCombo = 208
            })
        );

        yield return new(
            "beatmaps/taiko/Nanamori-chu  Goraku-bu - Happy Time wa Owaranai (eiri-) [Oni].osu",
            null,
            new NativeTaikoDifficultyAttributes(new()
            {
                MechanicalDifficulty = 3.352986428324316,
                RhythmDifficulty = 0.7077453559470016,
                ReadingDifficulty = 1.5491563327327842E-06,
                ColourDifficulty = 1.0586613624796721,
                StaminaDifficulty = 2.294325065844644,
                MonoStaminaFactor = 3.153914838605005E-08,
                ConsistencyFactor = 0.8156118660190322,
                StaminaTopStrains = 321.9080028367808,
                StarRating = 4.060733333427651,
                MaxCombo = 774
            })
        );

        yield return new(
            "beatmaps/taiko/AliA - Kakurenbo (Santi199) [From Here].osu",
            "HDDT",
            new NativeTaikoDifficultyAttributes(new()
            {
                MechanicalDifficulty = 5.103515453183134,
                RhythmDifficulty = 1.0708932741559247,
                ReadingDifficulty = 0.7367334668605363,
                ColourDifficulty = 1.4371426959204827,
                StaminaDifficulty = 3.6663727572626517,
                MonoStaminaFactor = 2.5868877078195936E-08,
                ConsistencyFactor = 0.7352801443908512,
                StaminaTopStrains = 465.85213223380447,
                StarRating = 6.911142194199595,
                MaxCombo = 1942
            })
        );

        yield return new(
            "beatmaps/taiko/The Quick Brown Fox - The Big Black (Blue Dragon) [Ono's Taiko Oni].osu",
            "FLSR",
            new NativeTaikoDifficultyAttributes(new()
            {
                MechanicalDifficulty = 4.427780738636938,
                RhythmDifficulty = 0.7543393707179822,
                ReadingDifficulty = 6.176286161922799E-07,
                ColourDifficulty = 1.3836958470317249,
                StaminaDifficulty = 3.044084891605213,
                MonoStaminaFactor = 0.00033412449222804705,
                ConsistencyFactor = 0.7341295073172242,
                StaminaTopStrains = 304.4580806854654,
                StarRating = 5.182120726983537,
                MaxCombo = 947
            })
        );
    }

    private static IEnumerable<TestCaseData> CalculateTimedTestCases()
    {
        yield return new(
            "beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu",
            null,
            112,
            new NativeTimedTaikoDifficultyAttributes(new(81368, new TaikoDifficultyAttributes
            {
                MechanicalDifficulty = 1.063550201534299,
                RhythmDifficulty = 0.046246377267803875,
                ReadingDifficulty = 1.842248290179725E-10,
                ColourDifficulty = 0.19918376290988551,
                StaminaDifficulty = 0.8643664386244134,
                MonoStaminaFactor = 0.7720121953149287,
                ConsistencyFactor = 0.670746588533328,
                StaminaTopStrains = 32.3425368070572,
                StarRating = 1.1097965789863276,
                MaxCombo = 106
            }))
        );

        yield return new(
            "beatmaps/taiko/Nanamori-chu  Goraku-bu - Happy Time wa Owaranai (eiri-) [Oni].osu",
            null,
            387,
            new NativeTimedTaikoDifficultyAttributes(new(69687, new TaikoDifficultyAttributes
            {
                MechanicalDifficulty = 3.3531972267397654,
                RhythmDifficulty = 0.6794347245262176,
                ReadingDifficulty = 1.559324276762787E-06,
                ColourDifficulty = 1.0436667386488132,
                StaminaDifficulty = 2.309530488090952,
                MonoStaminaFactor = 7.86981723600558E-09,
                ConsistencyFactor = 0.8057167829629687,
                StaminaTopStrains = 165.3982248972631,
                StarRating = 4.0326335105902595,
                MaxCombo = 388
            }))
        );

        yield return new(
            "beatmaps/taiko/AliA - Kakurenbo (Santi199) [From Here].osu",
            "HDDT",
            971,
            new NativeTimedTaikoDifficultyAttributes(new(145410, new TaikoDifficultyAttributes
            {
                MechanicalDifficulty = 5.281946528674957,
                RhythmDifficulty = 1.0861084227587794,
                ReadingDifficulty = 0.32339832393782947,
                ColourDifficulty = 1.5114155921096193,
                StaminaDifficulty = 3.770530936565337,
                MonoStaminaFactor = 1.7035959117208612E-08,
                ConsistencyFactor = 0.7388489607310218,
                StaminaTopStrains = 244.09359807054506,
                StarRating = 6.691453275371566,
                MaxCombo = 972
            }))
        );

        yield return new(
            "beatmaps/taiko/The Quick Brown Fox - The Big Black (Blue Dragon) [Ono's Taiko Oni].osu",
            "FLSR",
            474,
            new NativeTimedTaikoDifficultyAttributes(new(73077, new TaikoDifficultyAttributes
            {
                MechanicalDifficulty = 4.353554106356055,
                RhythmDifficulty = 0.6715643792771339,
                ReadingDifficulty = 6.012570932531322E-07,
                ColourDifficulty = 1.3750833404478853,
                StaminaDifficulty = 2.97847076590817,
                MonoStaminaFactor = 8.47015536456639E-05,
                ConsistencyFactor = 0.7376235614970663,
                StaminaTopStrains = 170.27077455498815,
                StarRating = 5.025119086890283,
                MaxCombo = 475
            }))
        );
    }
}