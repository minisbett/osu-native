using osu.Native.Bindings;

namespace osu.Native.Tests;

public class BeatmapTests
{
    [Test]
    public void CreateFromText_Success()
    {
        ErrorCode error = OsuNative.Beatmap_CreateFromText(Shared.BEATMAP_TEXT, out int id);
        OsuNative.Beatmap_Destroy(id);

        Assert.That(error, Is.EqualTo(ErrorCode.Success));
    }
    [Test]
    public void CreateFromFile_Success()
    {
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, Shared.BEATMAP_TEXT);

        ErrorCode error = OsuNative.Beatmap_CreateFromFile(tempFile, out int id);
        OsuNative.Beatmap_Destroy(id);
        File.Delete(tempFile);

        Assert.That(error, Is.EqualTo(ErrorCode.Success));
    }


    [Test]
    public void Create_TwoBeatmaps_IncreasedIds()
    {
        OsuNative.Beatmap_CreateFromText(Shared.BEATMAP_TEXT, out int id1);
        OsuNative.Beatmap_CreateFromText(Shared.BEATMAP_TEXT, out int id2);

        Assert.That(id1, Is.EqualTo(1));
        Assert.That(id2, Is.EqualTo(2));
    }

    [Test]
    public void Destroy_Success()
    {
        ErrorCode error1 = OsuNative.Beatmap_CreateFromText(Shared.BEATMAP_TEXT, out int id);
        ErrorCode error2 = OsuNative.Beatmap_Destroy(id);

        Assert.That(error1, Is.EqualTo(ErrorCode.Success));
        Assert.That(error2, Is.EqualTo(ErrorCode.Success));
    }

    [Test]
    public void Destroy_SameBeatmapTwice_Failure()
    {
        ErrorCode error1 = OsuNative.Beatmap_CreateFromText(Shared.BEATMAP_TEXT, out int id);
        ErrorCode error2 = OsuNative.Beatmap_Destroy(id);
        ErrorCode error3 = OsuNative.Beatmap_Destroy(id);

        Assert.That(error1, Is.EqualTo(ErrorCode.Success));
        Assert.That(error2, Is.EqualTo(ErrorCode.Success));
        Assert.That(error3, Is.EqualTo(ErrorCode.ObjectNotFound));
    }
}
