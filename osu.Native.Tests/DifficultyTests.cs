using Newtonsoft.Json;
using osu.Native.Bindings;
using osu.Native.Bindings.Structures;
using osu.Native.Bindings.Structures.Difficulty;
using static osu.Native.Tests.TestUtils;

namespace osu.Native.Tests;

public class DifficultyTests
{
    private int _beatmapId;

    [SetUp]
    public void Setup()
    {
        OsuNative.Beatmap_CreateFromText(GetResourceString("beatmap.osu"), out _beatmapId);
    }

    [Test]
    public void Compute_WithoutMods_Success()
    {
        ErrorCode error = OsuNative.Difficulty_CalculateOsu(_beatmapId, "", out _);

        Assert.That(error, Is.EqualTo(ErrorCode.Success));
    }

    [Test]
    public void Compute_WithMods_Success()
    {
        ErrorCode error1 = OsuNative.Difficulty_CalculateOsu(_beatmapId, "", out OsuDifficultyAttributes attributes1);
        ErrorCode error2 = OsuNative.Difficulty_CalculateOsu(_beatmapId, "[{\"acronym\":\"DT\"}]", out OsuDifficultyAttributes attributes2);

        Assert.Multiple(() =>
        {
            Assert.That(error1, Is.EqualTo(ErrorCode.Success));
            Assert.That(error2, Is.EqualTo(ErrorCode.Success));
            Assert.That(attributes1.StarRating, Is.Not.EqualTo(attributes2.StarRating));
        });
    }

    [Test]
    public void Compute_WithModSettings_Success()
    {
        string mods1 = JsonConvert.SerializeObject(new Mod[] { new("DT") });
        string mods2 = JsonConvert.SerializeObject(new Mod[] { new("DT", new() { ["speed_change"] = 2 }) });
        ErrorCode error1 = OsuNative.Difficulty_CalculateOsu(_beatmapId, mods1, out OsuDifficultyAttributes attributes1);
        ErrorCode error2 = OsuNative.Difficulty_CalculateOsu(_beatmapId, mods2, out OsuDifficultyAttributes attributes2);

        Assert.Multiple(() =>
        {
            Assert.That(error1, Is.EqualTo(ErrorCode.Success));
            Assert.That(error2, Is.EqualTo(ErrorCode.Success));
            Assert.That(attributes1.StarRating, Is.Not.EqualTo(attributes2.StarRating));
        });
    }

    [Test]
    public void Compute_Multiple_Success()
    {
        ErrorCode error = OsuNative.Difficulty_CalculateOsu(_beatmapId, "", out OsuDifficultyAttributes attributes1);
        Assert.That(error, Is.EqualTo(ErrorCode.Success));

        for (int i = 0; i < 5; i++)
        {
            error = OsuNative.Difficulty_CalculateOsu(_beatmapId, "", out OsuDifficultyAttributes attributes2);
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
        ErrorCode error = OsuNative.Difficulty_CalculateOsu(_beatmapId, "invalid mods", out _);

        Assert.That(error, Is.EqualTo(ErrorCode.ModsParsingFailed));
    }

    [Test]
    public void Compute_NotExistentBeatmap_Failure()
    {
        ErrorCode error = OsuNative.Difficulty_CalculateOsu(-1, "", out _);

        Assert.That(error, Is.EqualTo(ErrorCode.ObjectNotFound));
    }
}