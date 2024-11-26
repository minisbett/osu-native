using osu.Native.Bindings;
using osu.Native.Bindings.Structures.Difficulty;

namespace osu.Native.Tests;

public class DifficultyTests
{
    private int _beatmapContextId;

    [SetUp]
    public void Setup()
    {
        OsuNative.Beatmap_CreateFromText(Shared.BEATMAP_TEXT, out _beatmapContextId);
    }

    [Test]
    public void Compute_WithoutMods_Success()
    {
        ErrorCode error = OsuNative.Difficulty_ComputeOsu(_beatmapContextId, "", out _);

        Assert.That(error, Is.EqualTo(ErrorCode.Success));
    }

    [Test]
    public void Compute_WithMods_Success()
    {
        // TODO: Find a more consistent way to check if the mods affected the outcome. Right now it assumes DT affected the approach rate
        ErrorCode error1 = OsuNative.Difficulty_ComputeOsu(_beatmapContextId, "", out OsuDifficultyAttributes attributes1);
        ErrorCode error2 = OsuNative.Difficulty_ComputeOsu(_beatmapContextId, "[{\"acronym\":\"DT\"}]", out OsuDifficultyAttributes attributes2);

        Assert.That(error1, Is.EqualTo(ErrorCode.Success));
        Assert.That(error2, Is.EqualTo(ErrorCode.Success));
        Assert.That(attributes1.ApproachRate, Is.Not.EqualTo(attributes2.ApproachRate));
    }

    [Test]
    public void Compute_InvalidMods_Failure()
    {
        ErrorCode error = OsuNative.Difficulty_ComputeOsu(_beatmapContextId, "invalid mods", out _);

        Assert.That(error, Is.EqualTo(ErrorCode.ModsParsingFailed));
    }

    [Test]
    public void Compute_NotExistentBeatmap_Failure()
    {
        ErrorCode error = OsuNative.Difficulty_ComputeOsu(-1, "", out _);

        Assert.That(error, Is.EqualTo(ErrorCode.ObjectNotFound));
    }
}