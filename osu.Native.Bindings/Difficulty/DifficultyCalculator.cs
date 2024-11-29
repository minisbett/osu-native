// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using osu.Native.Bindings.Structures;

namespace osu.Native.Bindings.Difficulty;

public abstract class DifficultyCalculator<TDiffAttr, TPerfAttr, TScore>
{
    protected int _beatmapId;

    public DifficultyCalculator(FileInfo file)
    {
        OsuNative.Execute(() => OsuNative.Beatmap_CreateFromFile(file.FullName, out _beatmapId));
    }

    public DifficultyCalculator(string text)
    {
        OsuNative.Execute(() => OsuNative.Beatmap_CreateFromText(text, out _beatmapId));
    }

    ~DifficultyCalculator()
    {
        OsuNative.Execute(() => OsuNative.Beatmap_Destroy(_beatmapId));
    }

    public abstract TDiffAttr CalculateDifficulty(Mod[] mods);

    public abstract TPerfAttr CalculatePerformance(TDiffAttr diffAttributes, TScore score);
}
