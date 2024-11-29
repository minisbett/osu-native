using osu.Native.Bindings;
using System.Runtime.InteropServices;

namespace osu.Native.Tests;

public class ErrorHandlerTests
{
    private int _beatmapId;

    [SetUp]
    public void Setup()
    {
        OsuNative.Beatmap_CreateFromText(Shared.BEATMAP_TEXT, out _beatmapId);
    }

    [Test]
    public unsafe void SetLastError_SetsError()
    {
        ErrorCode error1 = OsuNative.Difficulty_ComputeOsu(-1, "", out _);
        string errorMessage1 = new(OsuNative.GetLastError());
        ErrorCode error2 = OsuNative.Difficulty_ComputeOsu(_beatmapId, "invalid mods", out _);
        string errorMessage2 = new(OsuNative.GetLastError());

        Assert.Multiple(() =>
        {
            Assert.That(error1, Is.EqualTo(ErrorCode.ObjectNotFound));
            Assert.That(error2, Is.EqualTo(ErrorCode.ModsParsingFailed));
            Assert.That(errorMessage1, Is.Not.EqualTo(errorMessage2));
        });
    }
}
