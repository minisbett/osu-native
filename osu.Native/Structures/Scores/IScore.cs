// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Scoring;

namespace osu.Native.Structures.Scores;

/// <summary>
/// The common class for all structs representing score information in the context of PP calculation.
/// </summary>
public interface IScore
{
    /// <summary>
    /// Builds a <see cref="ScoreInfo"/> object with the available score information.
    /// </summary>
    /// <param name="beatmap">The beatmap the score corresponds to.</param>
    /// <returns>The built <see cref="ScoreInfo"/> object.</returns>
    public ScoreInfo ToScoreInfo(FlatWorkingBeatmap beatmap);
}
