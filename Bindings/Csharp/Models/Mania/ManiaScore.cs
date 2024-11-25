// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace osu.Native.Bindings.Models.Mania;

public class ManiaScore(Mod[]? mods = null, int? maxCombo = null, int? countGreat = null, int? countGood = null, int? countOk = null, int? countMeh = null,
                      int? countMiss = null)
{
    public Mod[] Mods { get; set; } = mods ?? [];

    public int MaxCombo { get; set; } = maxCombo ?? 0;

    public int CountGreat { get; set; } = countGreat ?? 0;

    public int CountGood { get; set; } = countGood ?? 0;

    public int CountOk { get; set; } = countOk ?? 0;

    public int CountMeh { get; set; } = countMeh ?? 0;

    public int CountMiss { get; set; } = countMiss ?? 0;

    public Native ToNative()
    {
        return new Native
        {
            Mods = JsonConvert.SerializeObject(Mods),
            MaxCombo = MaxCombo,
            CountGreat = CountGreat,
            CountGood = CountGood,
            CountOk = CountOk,
            CountMeh = CountMeh,
            CountMiss = CountMiss
        };
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct Native
    {
        public string Mods;
        public int MaxCombo;
        public int CountGreat;
        public int CountGood;
        public int CountOk;
        public int CountMeh;
        public int CountMiss;
    }
}
