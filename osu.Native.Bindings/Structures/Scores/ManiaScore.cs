// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace osu.Native.Bindings.Structures.Scores;

/// <summary>
/// Represents all score information required for difficulty and performance processing of an osu!mania score.
/// </summary>
/// <param name="mods">The mods. Defaults to an empty array.</param>
/// <param name="maxCombo">The maximum combo of the score. Defaults to 0.</param>
/// <param name="countGreat">The amount of greats/300s. Defaults to 0.</param>
/// <param name="countGood">The amount of goods/200s. Defaults to 0.</param>
/// <param name="countOk">The amount of oks/100s. Defaults to 0.</param>
/// <param name="countMeh">The amount of mehs/50s. Defaults to 0.</param>
/// <param name="countMiss">The amount of misses. Defaults to 0.</param>
public class ManiaScore(Mod[]? mods = null, int? maxCombo = null, int? countGreat = null, int? countGood = null, int? countOk = null, int? countMeh = null,
                      int? countMiss = null)
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
    /// The amount of greats/300s.
    /// </summary>
    public int CountGreat { get; set; } = countGreat ?? 0;

    /// <summary>
    /// The amount of goods/200s.
    /// </summary>
    public int CountGood { get; set; } = countGood ?? 0;

    /// <summary>
    /// The amount of oks/100s.
    /// </summary>
    public int CountOk { get; set; } = countOk ?? 0;

    /// <summary>
    /// The amount of mehs/50s.
    /// </summary>
    public int CountMeh { get; set; } = countMeh ?? 0;

    /// <summary>
    /// The amount of misses.
    /// </summary>
    public int CountMiss { get; set; } = countMiss ?? 0;

    /// <summary>
    /// Converts the managed <see cref="ManiaScore"/> object into its <see cref="Native"/> representation.
    /// </summary>
    /// <returns>The native representation of the managed score information.</returns>
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

    /// <summary>
    /// A structure representation of <see cref="ManiaScore"/>.
    /// </summary>
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
