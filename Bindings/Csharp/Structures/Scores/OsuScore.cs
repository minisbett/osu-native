// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Native.Bindings.Models;
using System.Runtime.InteropServices;

namespace osu.Native.Bindings.Structures.Scores;

public class OsuScore(Mod[]? mods = null, int? maxCombo = null, int? count100 = null, int? count50 = null, int? countMiss = null, int? countLargeTickMiss = null,
                      int? countSliderTailMiss = null)
{
    public Mod[] Mods { get; set; } = mods ?? [];

    public int MaxCombo { get; set; } = maxCombo ?? 0;

    public int Count100 { get; set; } = count100 ?? 0;

    public int Count50 { get; set; } = count50 ?? 0;

    public int CountMiss { get; set; } = countMiss ?? 0;

    public int CountLargeTickMiss { get; set; } = countLargeTickMiss ?? 0;

    public int CountSliderTailMiss { get; set; } = countSliderTailMiss ?? 0;

    public Native ToNative()
    {
        return new Native
        {
            Mods = JsonConvert.SerializeObject(Mods),
            MaxCombo = MaxCombo,
            Count100 = Count100,
            Count50 = Count50,
            CountLargeTickMiss = CountLargeTickMiss,
            CountSliderTailMiss = CountSliderTailMiss
        };
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct Native
    {
        public string Mods;
        public int MaxCombo;
        public int Count100;
        public int Count50;
        public int CountMiss;
        public int CountLargeTickMiss;
        public int CountSliderTailMiss;
    }
}
