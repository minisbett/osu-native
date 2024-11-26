using osu.Native.Bindings;

namespace osu.Native.Tests;

public class BeatmapTests
{
    [Test]
    public void CreateFromText_ValidBeatmapText_Success()
    {
        ErrorCode error = OsuNative.Beatmap_CreateFromText(Shared.BEATMAP_TEXT, out int contextId);
        OsuNative.Beatmap_Destroy(contextId);

        Assert.That(error, Is.EqualTo(ErrorCode.Success));
    }

    [Test]
    public void Create_TwoBeatmaps_IncreasedIds()
    {
        OsuNative.Beatmap_CreateFromText(Shared.BEATMAP_TEXT, out int contextId1);
        OsuNative.Beatmap_CreateFromText(Shared.BEATMAP_TEXT, out int contextId2);

        Assert.That(contextId1, Is.EqualTo(1));
        Assert.That(contextId2, Is.EqualTo(2));
    }

    [Test]
    public void Destroy_Success()
    {
        ErrorCode error1 = OsuNative.Beatmap_CreateFromText(Shared.BEATMAP_TEXT, out int contextId);
        ErrorCode error2 = OsuNative.Beatmap_Destroy(contextId);

        Assert.That(error1, Is.EqualTo(ErrorCode.Success));
        Assert.That(error2, Is.EqualTo(ErrorCode.Success));
    }

    [Test]
    public void Destroy_SameBeatmapTwice_Failure()
    {
        ErrorCode error1 = OsuNative.Beatmap_CreateFromText(Shared.BEATMAP_TEXT, out int contextId);
        ErrorCode error2 = OsuNative.Beatmap_Destroy(contextId);
        ErrorCode error3 = OsuNative.Beatmap_Destroy(contextId);

        Assert.That(error1, Is.EqualTo(ErrorCode.Success));
        Assert.That(error2, Is.EqualTo(ErrorCode.Success));
        Assert.That(error3, Is.EqualTo(ErrorCode.ObjectNotFound));
    }
}
