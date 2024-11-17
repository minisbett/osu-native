// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Beatmaps;

namespace osu.Native;

public static class Context
{
    /// <summary>
    /// The next free context ID for <see cref="Beatmaps"/>.
    /// </summary>
    public static int NextBeatmapContextId = 0;

    /// <summary>
    /// Manages all created instances of <see cref="FlatWorkingBeatmap"/> with their assigned ID.<br/>
    /// The ID must be determined via <see cref="System.Threading.Interlocked.Increment(ref int)"/> of <see cref="NextBeatmapContextId"/>.
    /// </summary>
    public static Dictionary<int, FlatWorkingBeatmap> Beatmaps { get; } = [];
}
