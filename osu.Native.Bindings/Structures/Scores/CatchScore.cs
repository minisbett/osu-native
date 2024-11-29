// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace osu.Native.Bindings.Structures.Scores;

/// <summary>
/// Represents all score information required for difficulty and performance processing of an osu!catch score.
/// <param name="mods">The mods. Defaults to an empty array.</param>
/// <param name="maxCombo">The maximum combo of the score. Defaults to 0.</param>
/// <param name="countDroplets">The amount of droplets. Defaults to 0.</param>
/// <param name="countTinyDroplets">The amount of tiny droplets. Defaults to 0.</param>
/// <param name="countTinyMisses">The amount of tiny misses. Defaults to 0.</param>
/// <param name="countMiss">The amount of misses. Defaults to 0.</param>
/// </summary>
public class CatchScore(Mod[]? mods = null, int? maxCombo = null, int? countDroplets = null, int? countTinyDroplets = null, int? countTinyMisses = null,
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
    /// The amount of droplets.
    /// </summary>
    public int CountDroplets { get; set; } = countDroplets ?? 0;

    /// <summary>
    /// The amount of tiny droplets.
    /// </summary>
    public int CountTinyDroplets { get; set; } = countTinyDroplets ?? 0;

    /// <summary>
    /// The amount of tiny misses.
    /// </summary>
    public int CountTinyMisses { get; set; } = countTinyMisses ?? 0;

    /// <summary>
    /// The amount of misses.
    /// </summary>
    public int CountMiss { get; set; } = countMiss ?? 0;

    /// <summary>
    /// Converts the managed <see cref="CatchScore"/> object into its <see cref="Native"/> representation.
    /// </summary>
    /// <returns>The native representation of the managed score information.</returns>
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

    /// <summary>
    /// A structure representation of <see cref="CatchScore"/>.
    /// </summary>
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
