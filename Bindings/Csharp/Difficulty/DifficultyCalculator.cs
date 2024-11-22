// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using osu.Native.Bindings.Models;

namespace osu.Native.Bindings.Difficulty;

public abstract class DifficultyCalculator<TDiffAttr, TPerfAttr, TScore>
{
    protected readonly int _beatmapContextId;

    public DifficultyCalculator(FileInfo file)
    {
        OsuNative.Beatmap_CreateFromFile(file.FullName, out _beatmapContextId);
    }

    public DifficultyCalculator(string text)
    {
        OsuNative.Beatmap_CreateFromText(text, out _beatmapContextId);
    }

    ~DifficultyCalculator()
    {
        OsuNative.Beatmap_Destroy(_beatmapContextId);
    }

    public abstract TDiffAttr CalculateDifficulty(Mod[] mods);

    public abstract TPerfAttr CalculatePerformance(TDiffAttr diffAttributes, TScore score);
}
