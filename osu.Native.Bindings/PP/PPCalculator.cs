// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using osu.Native.Bindings.Structures;

namespace osu.Native.Bindings.PP;

/// <summary>
/// The base class for a difficulty and performance calculator.
/// </summary>
/// <typeparam name="TDiffAttr">The difficulty attributes type.</typeparam>
/// <typeparam name="TPerfAttr">The performance attributes type.</typeparam>
/// <typeparam name="TScore">The score type.</typeparam>
public abstract class PPCalculator<TDiffAttr, TPerfAttr, TScore>
{
    protected int _beatmapId;

    /// <summary>
    /// Creates a difficulty calculator for the beatmap at the specified .osu file.
    /// </summary>
    /// <param name="file">The <see cref="FileInfo"/> instance of the .osu file.</param>
    public PPCalculator(FileInfo file)
    {
        OsuNative.Execute(() => OsuNative.Beatmap_CreateFromFile(file.FullName, out _beatmapId));
    }

    /// <summary>
    /// Creates a difficulty calculator for the beatmap in the specified string.
    /// </summary>
    /// <param name="text">The beatmap text, in .osu file format.</param>
    public PPCalculator(string text)
    {
        OsuNative.Execute(() => OsuNative.Beatmap_CreateFromText(text, out _beatmapId));
    }

    ~PPCalculator()
    {
        OsuNative.Execute(() => OsuNative.Beatmap_Destroy(_beatmapId));
    }

    /// <summary>
    /// Calculates the difficulty of the beatmap.
    /// </summary>
    /// <returns>The calculated difficulty attributes.</returns>
    public TDiffAttr CalculateDifficulty() => CalculateDifficulty([]);

    /// <summary>
    /// Calculates the difficulty of the beatmap with the specified mods.
    /// </summary>
    /// <param name="mods">The mods.</param>
    /// <returns>The calculated difficulty attributes.</returns>
    public abstract TDiffAttr CalculateDifficulty(Mod[] mods);

    /// <summary>
    /// Calculates the performance of the beatmap for the specified score and difficulty attributes.
    /// </summary>
    /// <param name="diffAttributes">The difficulty attributes.</param>
    /// <param name="score">The score.</param>
    /// <returns>The calculated difficulty attributes.</returns>
    public abstract TPerfAttr CalculatePerformance(TDiffAttr diffAttributes, TScore score);
}
