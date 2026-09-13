using System.Runtime.InteropServices.Marshalling;
using System.Text;
using osu.Game.Beatmaps;
using osu.Native.Objects;
using osu.Native.Structures;

namespace osu.Native.Tests.Objects;

[TestFixture]
internal unsafe class BeatmapTests
{
    private NativeBeatmap _nativeBeatmap;

    [SetUp]
    public void Setup()
    {
        byte[] beatmap = TestUtils.GetResource("beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu");

        fixed (byte* ptr = beatmap)
        fixed (NativeBeatmap* nativePtr = &_nativeBeatmap)
            BeatmapObject.CreateFromText(ptr, nativePtr);
    }

    /// <summary>
    /// Creates a beatmap from the string contents of a .osu file, resolves it and expects the parsed beatmap and ruleset ID to be correct.
    /// </summary>
    [TestCase("beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu", 75, 0)]
    [TestCase("beatmaps/taiko/Nanamori-chu  Goraku-bu - Happy Time wa Owaranai (eiri-) [Oni].osu", 1625591, 1)]
    [TestCase("beatmaps/catch/Lite Show Magic (t+pazolite vs C-Show) - Crack Traxxxx (Fatfan Kolek) [Spec's Hi-Speed Overdose].osu", 481938, 2)]
    [TestCase("beatmaps/mania/xi - FREEDOM DiVE (razlteh) [Blocko's 7K Black Another].osu", 480207, 3)]
    public void CreateFromText_SuccessAndCorrectRulesetBeatmapIds(string fileName, int beatmapId, int rulesetId)
    {
        byte[] beatmapData = TestUtils.GetResource(fileName);

        NativeBeatmap nativeBeatmap;
        ErrorCode errorCode;
        fixed (byte* ptr = beatmapData)
            errorCode = BeatmapObject.CreateFromText(ptr, &nativeBeatmap);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
        Assert.That(nativeBeatmap.BeatmapId, Is.EqualTo(beatmapId));
        Assert.That(nativeBeatmap.RulesetId, Is.EqualTo(rulesetId));

        FlatWorkingBeatmap beatmap = nativeBeatmap.Handle.Resolve();

        Assert.That(beatmap.BeatmapInfo.OnlineID, Is.EqualTo(beatmapId));
        Assert.That(beatmap.BeatmapInfo.Ruleset.OnlineID, Is.EqualTo(rulesetId));
    }

    /// <summary>
    /// Creates a beatmap from the file path to a .osu file (via temp file), resolves it and expects the parsed beatmap and ruleset ID to be correct.
    /// </summary>
    [TestCase("beatmaps/osu/Kenji Ninuma - DISCOPRINCE (peppy) [Normal].osu", 75, 0)]
    [TestCase("beatmaps/taiko/Nanamori-chu  Goraku-bu - Happy Time wa Owaranai (eiri-) [Oni].osu", 1625591, 1)]
    [TestCase("beatmaps/catch/Lite Show Magic (t+pazolite vs C-Show) - Crack Traxxxx (Fatfan Kolek) [Spec's Hi-Speed Overdose].osu", 481938, 2)]
    [TestCase("beatmaps/mania/xi - FREEDOM DiVE (razlteh) [Blocko's 7K Black Another].osu", 480207, 3)]
    public void CreateFromFile_SuccessAndCorrectRulesetBeatmapIds(string fileName, int beatmapId, int rulesetId)
    {
        string tempFilePath = Path.GetTempFileName();
        File.WriteAllBytes(tempFilePath, TestUtils.GetResource(fileName));

        NativeBeatmap nativeBeatmap;
        ErrorCode errorCode = BeatmapObject.CreateFromFile(Utf8StringMarshaller.ConvertToUnmanaged(tempFilePath), &nativeBeatmap);

        File.Delete(tempFilePath);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
        Assert.That(nativeBeatmap.BeatmapId, Is.EqualTo(beatmapId));
        Assert.That(nativeBeatmap.RulesetId, Is.EqualTo(rulesetId));

        FlatWorkingBeatmap beatmap = nativeBeatmap.Handle.Resolve();

        Assert.That(beatmap.BeatmapInfo.OnlineID, Is.EqualTo(beatmapId));
        Assert.That(beatmap.BeatmapInfo.Ruleset.OnlineID, Is.EqualTo(rulesetId));
    }

    /// <summary>
    /// Gets the title of a beatmap and expects the title to be correct.
    /// </summary>
    [Test]
    public void GetTitle_ReturnsCorrectTitle()
    {
        int size = 0;
        BeatmapObject.GetTitle(_nativeBeatmap.Handle, null, &size);
        byte[] buffer = new byte[size];
        fixed (byte* bufferPtr = buffer)
            BeatmapObject.GetTitle(_nativeBeatmap.Handle, bufferPtr, &size);

        Assert.That(Encoding.UTF8.GetString(buffer).TrimEnd(char.MinValue), Is.EqualTo("DISCO★PRINCE"));
    }

    /// <summary>
    /// Gets the artist of a beatmap and expects the title to be correct.
    /// </summary>
    [Test]
    public void GetArtist_ReturnsCorrectArtist()
    {
        int size = 0;
        BeatmapObject.GetArtist(_nativeBeatmap.Handle, null, &size);
        byte[] buffer = new byte[size];
        fixed (byte* bufferPtr = buffer)
            BeatmapObject.GetArtist(_nativeBeatmap.Handle, bufferPtr, &size);

        Assert.That(Encoding.UTF8.GetString(buffer).TrimEnd(char.MinValue), Is.EqualTo("Kenji Ninuma"));
    }

    /// <summary>
    /// Gets the difficulty version name of a beatmap and expects the title to be correct.
    /// </summary>
    [Test]
    public void GetVersion_ReturnsCorrectVersion()
    {
        int size = 0;
        BeatmapObject.GetVersion(_nativeBeatmap.Handle, null, &size);
        byte[] buffer = new byte[size];
        fixed (byte* bufferPtr = buffer)
            BeatmapObject.GetVersion(_nativeBeatmap.Handle, bufferPtr, &size);

        Assert.That(Encoding.UTF8.GetString(buffer).TrimEnd(char.MinValue), Is.EqualTo("Normal"));
    }
}
