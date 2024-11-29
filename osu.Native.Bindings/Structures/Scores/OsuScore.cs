// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace osu.Native.Bindings.Structures.Scores;

/// <summary>
/// Represents all score information required for difficulty and performance processing of an osu!standard score.
/// </summary>
/// <param name="mods">The mods. Defaults to an empty array.</param>
/// <param name="maxCombo">The maximum combo of the score. Defaults to 0.</param>
/// <param name="count100">The amount of 100s/goods. Defaults to 0.</param>
/// <param name="count50">The amounf of 50s/mehs. Defaults to 0.</param>
/// <param name="countMiss">The amount of misses. Defaults to 0.</param>
/// <param name="countLargeTickMiss">The amount of large tick misses (slider ticks and slider reverse arrows). Defaults to 0.</param>
/// <param name="countSliderTailMiss">The amount of slider tail misses. Defaults to 0.</param>
public class OsuScore(Mod[]? mods = null, int? maxCombo = null, int? count100 = null, int? count50 = null, int? countMiss = null, int? countLargeTickMiss = null,
                      int? countSliderTailMiss = null)
{
    /// <summary>
    /// The mods.
    /// </summary>
    public Mod[] Mods { get; set; } = mods ?? [];

    /// <summary>
    /// The maximum combo of the score.
    /// </summary>
    public int MaxCombo { get; set; } = maxCombo ?? 0;

    /// <summary>
    /// The amount of 100s/goods.
    /// </summary>
    public int Count100 { get; set; } = count100 ?? 0;

    /// <summary>
    /// The amount of 50s/mehs.
    /// </summary>
    public int Count50 { get; set; } = count50 ?? 0;

    /// <summary>
    /// The amount of misses.
    /// </summary>
    public int CountMiss { get; set; } = countMiss ?? 0;

    /// <summary>
    /// The amount of large tick misses (slider ticks and slider reverse arrows).
    /// </summary>
    public int CountLargeTickMiss { get; set; } = countLargeTickMiss ?? 0;

    /// <summary>
    /// The amount of slider tail misses.
    /// </summary>
    public int CountSliderTailMiss { get; set; } = countSliderTailMiss ?? 0;

    /// <summary>
    /// Converts the managed <see cref="OsuScore"/> object into its <see cref="Native"/> representation.
    /// </summary>
    /// <returns>The native representation of the managed score information.</returns>
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

    /// <summary>
    /// A structure representation of <see cref="OsuScore"/>.
    /// </summary>
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
