using Newtonsoft.Json.Linq;
using osu.Game.Online.API;
using osu.Native.Structures;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Tests.Objects;

[TestFixture(typeof(NativeOsuDifficultyAttributes), typeof(NativeTimedOsuDifficultyAttributes))]
[TestFixture(typeof(NativeTaikoDifficultyAttributes), typeof(NativeTimedTaikoDifficultyAttributes))]
[TestFixture(typeof(NativeCatchDifficultyAttributes), typeof(NativeTimedCatchDifficultyAttributes))]
[TestFixture(typeof(NativeManiaDifficultyAttributes), typeof(NativeTimedManiaDifficultyAttributes))]
internal unsafe class DifficultyCalculatorTests<TAttributes, TTimedAttributes>
    where TAttributes : unmanaged
    where TTimedAttributes : unmanaged
{
    private static readonly int _ruleset = TestUtils.GetRuleset<TAttributes>();
    private const string beatmap = "beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu";

    private NativeTestUtils _native = null!;
    private NativeRuleset _nativeRuleset;

    [SetUp]
    public void Setup()
    {
        _native = new(_ruleset);
        _nativeRuleset = _native.CreateRuleset(_ruleset);
    }

    [TearDown]
    public void Teardown() => _native.Dispose();

    [Test]
    public void Create_ExpectedRuleset_Success()
    {
        NativeBeatmap nativeBeatmap = _native.CreateBeatmap(beatmap);
        uint calculator;
        NativeTestUtils.AssertSuccess(_native.CreateDifficulty(_nativeRuleset.Handle.Id, nativeBeatmap.Handle.Id, &calculator));
    }

    [Test]
    public void Create_UnexpectedRuleset_Errors()
    {
        NativeRuleset ruleset = _native.CreateRuleset((_ruleset + 2) % 4);
        NativeBeatmap nativeBeatmap = _native.CreateBeatmap(beatmap);
        uint calculator;
        ErrorCode error = _native.CreateDifficulty(ruleset.Handle.Id, nativeBeatmap.Handle.Id, &calculator);

        Assert.That(error, Is.EqualTo(ErrorCode.UnexpectedRuleset), NativeTestUtils.GetLastError());
    }

    [TestCaseSource(nameof(CalculateTestCases))]
    public void Calculate_Success(string filename, List<APIMod> mods, JObject expected)
    {
        NativeBeatmap nativeBeatmap = _native.CreateBeatmap(filename);
        NativeModsCollection nativeMods = _native.CreateMods(mods);
        uint calculator;
        NativeTestUtils.AssertSuccess(_native.CreateDifficulty(_nativeRuleset.Handle.Id, nativeBeatmap.Handle.Id, &calculator));

        TAttributes attributes;
        NativeTestUtils.AssertSuccess(_native.CalculateDifficulty(calculator, nativeMods.Handle.Id, &attributes));
        TestUtils.AssertEqualAttributes(attributes, expected);
    }

    [TestCaseSource(nameof(CalculateTimedTestCases))]
    public void CalculateTimed_Success(string filename, List<APIMod> mods, int index, JObject expected)
    {
        NativeBeatmap nativeBeatmap = _native.CreateBeatmap(filename);
        NativeModsCollection nativeMods = _native.CreateMods(mods);
        uint calculator;
        NativeTestUtils.AssertSuccess(_native.CreateDifficulty(_nativeRuleset.Handle.Id, nativeBeatmap.Handle.Id, &calculator));

        int size = 0;
        ErrorCode error = _native.CalculateTimed(calculator, nativeMods.Handle.Id, null, &size);
        Assert.That(error, Is.EqualTo(ErrorCode.BufferSizeQuery), NativeTestUtils.GetLastError());

        TTimedAttributes[] attributes = new TTimedAttributes[size];
        fixed (TTimedAttributes* ptr = attributes)
            NativeTestUtils.AssertSuccess(_native.CalculateTimed(calculator, nativeMods.Handle.Id, ptr, &size));

        TestUtils.AssertEqualAttributes(attributes[index], expected);
    }

    private static IEnumerable<TestCaseData> CalculateTestCases()
        => TestUtils.GetCalculatorTestCases(_ruleset, "difficulty.json");

    private static IEnumerable<TestCaseData> CalculateTimedTestCases()
        => TestUtils.GetCalculatorTestCases(_ruleset, "timed-difficulty.json");
}