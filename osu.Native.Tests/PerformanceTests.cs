using osu.Native.Bindings;
using osu.Native.Bindings.Models;
using osu.Native.Bindings.Structures.Difficulty;
using osu.Native.Bindings.Structures.Performance;
using osu.Native.Bindings.Structures.Scores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu.Native.Tests;

internal class PerformanceTests
{
    private int _beatmapContextId;
    private OsuDifficultyAttributes _attributes;

    [SetUp]
    public void Setup()
    {
        OsuNative.Beatmap_CreateFromText(Shared.BEATMAP_TEXT, out _beatmapContextId);
        OsuNative.Difficulty_ComputeOsu(_beatmapContextId, "", out _attributes);
    }

    [Test]
    public void Compute_WithoutMods_Success()
    {
        ErrorCode error = OsuNative.Performance_ComputeOsu(_beatmapContextId, _attributes, new OsuScore().ToNative(), out _);

        Assert.That(error, Is.EqualTo(ErrorCode.Success));
    }

    [Test]
    public void Compute_WithMods_Success()
    {
        // TODO: Find a more consistent way to check if the mods affected the outcome. Right now it assumes HD will change the total PP
        ErrorCode error1 = OsuNative.Performance_ComputeOsu(_beatmapContextId, _attributes, new OsuScore().ToNative(), out OsuPerformanceAttributes attributes1);
        ErrorCode error2 = OsuNative.Performance_ComputeOsu(_beatmapContextId, _attributes, new OsuScore([new Mod("HD")]).ToNative(), out OsuPerformanceAttributes attributes2);

        Assert.That(error1, Is.EqualTo(ErrorCode.Success));
        Assert.That(error2, Is.EqualTo(ErrorCode.Success));
        Assert.That(attributes1.Total, Is.Not.EqualTo(attributes2.Total));
    }

    [Test]
    public void Compute_InvalidMods_Failure()
    {
        ErrorCode error = OsuNative.Performance_ComputeOsu(_beatmapContextId, _attributes, new OsuScore([new Mod("invalid mod")]).ToNative(), out _);

        Assert.That(error, Is.EqualTo(ErrorCode.ModsParsingFailed));
    }

    [Test]
    public void Compute_NotExistentBeatmap_Failure()
    {
        ErrorCode error = OsuNative.Performance_ComputeOsu(-1, _attributes, new OsuScore().ToNative(), out _);

        Assert.That(error, Is.EqualTo(ErrorCode.ObjectNotFound));
    }
}
