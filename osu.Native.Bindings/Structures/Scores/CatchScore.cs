// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace osu.Native.Bindings.Structures.Scores;

public class CatchScore(Mod[]? mods = null, int? maxCombo = null, int? countDroplets = null, int? countTinyDroplets = null, int? countTinyMisses = null,
                        int? countMiss = null)
{
    public Mod[] Mods { get; set; } = mods ?? [];

    public int MaxCombo { get; set; } = maxCombo ?? 0;

    public int CountDroplets { get; set; } = countDroplets ?? 0;

    public int CountTinyDroplets { get; set; } = countTinyDroplets ?? 0;

    public int CountTinyMisses { get; set; } = countTinyMisses ?? 0;

    public int CountMiss { get; set; } = countMiss ?? 0;

    public Native ToNative()
    {
        return new Native
        {
            Mods = JsonConvert.SerializeObject(Mods),
            MaxCombo = MaxCombo,
            CountDroplets = CountDroplets,
            CountTinyDroplets = CountTinyDroplets,
            CountTinyMisses = CountTinyMisses,
            CountMiss = CountMiss
        };
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct Native
    {
        public string Mods;
        public int MaxCombo;
        public int CountDroplets;
        public int CountTinyDroplets;
        public int CountTinyMisses;
        public int CountMiss;
    }
}
