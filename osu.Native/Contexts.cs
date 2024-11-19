// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Native.Helpers;

namespace osu.Native;

/// <summary>
/// Contains static instances of all <see cref="Context{T}"/>.
/// </summary>
public static class Contexts
{
    /// <summary>
    /// Stores all beatmaps created and destroyed via <see cref="EntryPoints.BeatmapEntryPoints"/>.
    /// </summary>
    public static Context<FlatWorkingBeatmap> Beatmaps { get; } = new();
}
