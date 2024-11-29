using osu.Native.Bindings;
using osu.Native.Bindings.Structures.Difficulty;

namespace osu.Native.Tests;

public class DifficultyTests
{
    private int _beatmapId;

    [SetUp]
    public void Setup()
    {
        OsuNative.Beatmap_CreateFromText(Shared.BEATMAP_TEXT, out _beatmapId);
    }

    [Test]
    public void Compute_WithoutMods_Success()
    {
        ErrorCode error = OsuNative.Difficulty_ComputeOsu(_beatmapId, "", out _);

        Assert.That(error, Is.EqualTo(ErrorCode.Success));
    }

    [Test]
    public void Compute_WithMods_Success()
    {
        // TODO: Find a more consistent way to check if the mods affected the outcome. Right now it assumes DT affected the star rating
        ErrorCode error1 = OsuNative.Difficulty_ComputeOsu(_beatmapId, "", out OsuDifficultyAttributes attributes1);
        ErrorCode error2 = OsuNative.Difficulty_ComputeOsu(_beatmapId, "[{\"acronym\":\"DT\"}]", out OsuDifficultyAttributes attributes2);

        Assert.Multiple(() =>
        {
            Assert.That(error1, Is.EqualTo(ErrorCode.Success));
            Assert.That(error2, Is.EqualTo(ErrorCode.Success));
            Assert.That(attributes1.StarRating, Is.EqualTo(attributes2.StarRating));
        });
    }

    [Test]
    public void Compute_Multiple_Success()
    {
        ErrorCode error = OsuNative.Difficulty_ComputeOsu(_beatmapId, "", out OsuDifficultyAttributes attributes1);
        Assert.That(error, Is.EqualTo(ErrorCode.Success));

        for (int i = 0; i < 5; i++)
        {
            error = OsuNative.Difficulty_ComputeOsu(_beatmapId, "", out OsuDifficultyAttributes attributes2);
            Assert.Multiple(() =>
            {
                Assert.That(error, Is.EqualTo(ErrorCode.Success));
                Assert.That(attributes1.StarRating, Is.EqualTo(attributes2.StarRating));
            });
        }
    }

    [Test]
    public void Compute_InvalidMods_Failure()
    {
        ErrorCode error = OsuNative.Difficulty_ComputeOsu(_beatmapId, "invalid mods", out _);

        Assert.That(error, Is.EqualTo(ErrorCode.ModsParsingFailed));
    }

    [Test]
    public void Compute_NotExistentBeatmap_Failure()
    {
        ErrorCode error = OsuNative.Difficulty_ComputeOsu(-1, "", out _);

        Assert.That(error, Is.EqualTo(ErrorCode.ObjectNotFound));
    }
}