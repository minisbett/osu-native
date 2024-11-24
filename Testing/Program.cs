using osu.Native.Bindings.Difficulty;
using osu.Native.Bindings.Models;
using osu.Native.Bindings.Models.Osu;


string osuFile = @"C:\Users\mini\Desktop\test.osu";

OsuDifficultyCalculator calculator = new OsuDifficultyCalculator(new FileInfo(osuFile));

Mod[] mods = [new Mod("DT", new() { ["speed_change"] = 2 }), new Mod("HD")];
OsuDifficultyAttributes diffAttributes = calculator.CalculateDifficulty(mods);
OsuPerformanceAttributes perfAttributes = calculator.CalculatePerformance(diffAttributes, new OsuScore(mods, 587));

Console.WriteLine("Star Rating: " + diffAttributes.StarRating);
Console.WriteLine("Total PP: " + perfAttributes.Total);

// TODO: implement artifacts publish thing for osu.Native and osu.Native.Bindings
//       implement logger in osu.Native.Bindings
//       implement error handling in osu.Native.Bindings
//       implement tests