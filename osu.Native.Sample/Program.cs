using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using osu.Native.Bindings.PP;
using osu.Native.Bindings.Structures;
using osu.Native.Bindings.Structures.Difficulty;
using osu.Native.Bindings.Structures.Performance;
using osu.Native.Bindings.Structures.Scores;

Mod[] mods = args.Length > 1 ? args[1].Chunk(2).Select(x => new Mod(new(x))).ToArray() : [];
string beatmapText = await new HttpClient().GetStringAsync($"https://osu.ppy.sh/osu/{args[0]}");

OsuPPCalculator calc = new(beatmapText);
OsuDifficultyAttributes diffAttributes = calc.CalculateDifficulty(mods);
OsuPerformanceAttributes perfAttributes = calc.CalculatePerformance(diffAttributes, new OsuScore(mods));

Console.WriteLine($"Star Rating: {Math.Round(diffAttributes.StarRating, 2).ToString(CultureInfo.InvariantCulture)}");
Console.WriteLine($"PP: {Math.Round(perfAttributes.Total, 2).ToString(CultureInfo.InvariantCulture)}");