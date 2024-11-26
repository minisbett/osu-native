using osu.Native.Bindings;

namespace osu.Native.Tests;

public class ErrorHandlerTests
{
    private int _beatmapContextId;

    [SetUp]
    public void Setup()
    {
        OsuNative.Beatmap_CreateFromText(Shared.BEATMAP_TEXT, out  _beatmapContextId);
    }

    [Test]
    public void SetLastError_SetsError()
    {
        ErrorCode error = OsuNative.Difficulty_ComputeOsu(_beatmapContextId, "invalid mods", out _);
        string? errorMessage = OsuNative.GetLastError();

        Assert.That(error, Is.EqualTo(ErrorCode.ModsParsingFailed));
        Assert.That(errorMessage, Is.Not.Empty);
    }

    [Test]
    public void SetLastError_NullIfSuccess()
    {
        ErrorCode error = OsuNative.Difficulty_ComputeOsu(_beatmapContextId, "", out _);
        string? errorMessage = OsuNative.GetLastError();

        Assert.That(error, Is.EqualTo(ErrorCode.Success));
        Assert.That(errorMessage, Is.Null);
    }
}
