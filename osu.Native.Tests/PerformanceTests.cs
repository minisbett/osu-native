using osu.Native.Bindings;
using osu.Native.Bindings.Structures;
using osu.Native.Bindings.Structures.Difficulty;
using osu.Native.Bindings.Structures.Performance;
using osu.Native.Bindings.Structures.Scores;
using static osu.Native.Tests.TestUtils;

namespace osu.Native.Tests;

internal class PerformanceTests
{
    private int _beatmapId;
    private OsuDifficultyAttributes _attributes;

    [SetUp]
    public void Setup()
    {
        OsuNative.Beatmap_CreateFromText(GetResourceString("beatmap.osu"), out _beatmapId);
        OsuNative.Difficulty_ComputeOsu(_beatmapId, "", out _attributes);
    }

    [Test]
    public void Compute_WithoutMods_Success()
    {
        ErrorCode error = OsuNative.Performance_ComputeOsu(_beatmapId, _attributes, new OsuScore().ToNative(), out _);

        Assert.That(error, Is.EqualTo(ErrorCode.Success));
    }

    [Test]
    public void Compute_WithMods_Success()
    {
        // TODO: Find a more consistent way to check if the mods affected the outcome. Right now it assumes HD will change the total PP
        ErrorCode error1 = OsuNative.Performance_ComputeOsu(_beatmapId, _attributes, new OsuScore().ToNative(), out OsuPerformanceAttributes attributes1);
        ErrorCode error2 = OsuNative.Performance_ComputeOsu(_beatmapId, _attributes, new OsuScore([new Mod("HD")]).ToNative(), out OsuPerformanceAttributes attributes2);

        Assert.Multiple(() =>
        {
            Assert.That(error1, Is.EqualTo(ErrorCode.Success));
            Assert.That(error2, Is.EqualTo(ErrorCode.Success));
            Assert.That(attributes1.Total, Is.Not.EqualTo(attributes2.Total));
        });
    }

    [Test]
    public void Compute_Multiple_Success()
    {
        ErrorCode error = OsuNative.Performance_ComputeOsu(_beatmapId, _attributes, new OsuScore().ToNative(), out OsuPerformanceAttributes attributes1);
        Assert.That(error, Is.EqualTo(ErrorCode.Success));

        for (int i = 0; i < 5; i++)
        {
            error = OsuNative.Performance_ComputeOsu(_beatmapId, _attributes, new OsuScore().ToNative(), out OsuPerformanceAttributes attributes2);
            Assert.Multiple(() =>
            {
                Assert.That(error, Is.EqualTo(ErrorCode.Success));
                Assert.That(attributes2.Total, Is.EqualTo(attributes1.Total));
            });
        }
    }

    [Test]
    public void Compute_InvalidMods_Failure()
    {
        ErrorCode error = OsuNative.Performance_ComputeOsu(_beatmapId, _attributes, new OsuScore([new Mod("invalid mod")]).ToNative(), out _);

        Assert.That(error, Is.EqualTo(ErrorCode.ModsParsingFailed));
    }

    [Test]
    public void Compute_NotExistentBeatmap_Failure()
    {
        ErrorCode error = OsuNative.Performance_ComputeOsu(-1, _attributes, new OsuScore().ToNative(), out _);

        Assert.That(error, Is.EqualTo(ErrorCode.ObjectNotFound));
    }
}
