using osu.Native.Bindings;

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
    public void SetLastError_SetsError()
    {
        string? errorMessage1 = OsuNative.GetLastError();
        ErrorCode error = OsuNative.Difficulty_ComputeOsu(_beatmapId, "invalid mods", out _);
        string? errorMessage2 = OsuNative.GetLastError();

        Assert.That(error, Is.EqualTo(ErrorCode.ModsParsingFailed));
        Assert.That(errorMessage1, Is.Null);
        Assert.That(errorMessage2, Is.Not.Null);
    }

    [Test]
    public void SetLastError_NullIfSuccess()
    {
        ErrorCode error = OsuNative.Difficulty_ComputeOsu(_beatmapId, "", out _);
        string? errorMessage = OsuNative.GetLastError();

        Assert.That(error, Is.EqualTo(ErrorCode.Success));
        Assert.That(errorMessage, Is.Null);
    }
}
