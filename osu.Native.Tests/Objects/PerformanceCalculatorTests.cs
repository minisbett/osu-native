using Newtonsoft.Json.Linq;
using osu.Game.Online.API;
using osu.Native.Structures;
using osu.Native.Structures.Difficulty;
using osu.Native.Structures.Performance;

namespace osu.Native.Tests.Objects;

[TestFixture(typeof(NativeOsuDifficultyAttributes), typeof(NativeOsuPerformanceAttributes))]
[TestFixture(typeof(NativeTaikoDifficultyAttributes), typeof(NativeTaikoPerformanceAttributes))]
[TestFixture(typeof(NativeCatchDifficultyAttributes), typeof(NativeCatchPerformanceAttributes))]
[TestFixture(typeof(NativeManiaDifficultyAttributes), typeof(NativeManiaPerformanceAttributes))]
internal unsafe class PerformanceCalculatorTests<TDifficultyAttributes, TPerformanceAttributes>
    where TDifficultyAttributes : unmanaged
    where TPerformanceAttributes : unmanaged
{
    private static readonly int _ruleset = TestUtils.GetRuleset<TDifficultyAttributes>();

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
    public void Create_Success()
    {
        uint calculator;
        NativeTestUtils.AssertSuccess(_native.CreatePerformance(&calculator));
    }

    [TestCaseSource(nameof(GetTestCases))]
    public void Calculate_Success(string filename, List<APIMod> mods, NativeScoreInfo score, JObject expected)
    {
        NativeBeatmap beatmap = _native.CreateBeatmap(filename);
        NativeModsCollection nativeMods = _native.CreateMods(mods);

        score.RulesetHandle = _nativeRuleset.Handle;
        score.BeatmapHandle = beatmap.Handle;
        score.ModsHandle = nativeMods.Handle;

        uint difficultyCalculator;
        NativeTestUtils.AssertSuccess(_native.CreateDifficulty(_nativeRuleset.Handle.Id, beatmap.Handle.Id, &difficultyCalculator));

        TDifficultyAttributes difficulty;
        NativeTestUtils.AssertSuccess(_native.CalculateDifficulty(difficultyCalculator, nativeMods.Handle.Id, &difficulty));

        uint calculator;
        NativeTestUtils.AssertSuccess(_native.CreatePerformance(&calculator));

        TPerformanceAttributes attributes;
        NativeTestUtils.AssertSuccess(_native.CalculatePerformance(calculator, score, &difficulty, &attributes));
        TestUtils.AssertEqualAttributes(attributes, expected);
    }

    private static IEnumerable<TestCaseData> GetTestCases()
        => TestUtils.GetCalculatorTestCases(_ruleset, "performance.json");
}