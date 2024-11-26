using osu.Native.Bindings.Difficulty;
using osu.Native.Bindings.Models;
using osu.Native.Bindings.Structures.Difficulty;
using osu.Native.Bindings.Structures.Performance;
using osu.Native.Bindings.Structures.Scores;

string osuFile = @"C:\Users\mini\Desktop\test.osu";

OsuDifficultyCalculator calculator = new OsuDifficultyCalculator(new FileInfo(osuFile));

Mod[] mods = [new Mod("DT", new() { ["speed_change"] = 2 }), new Mod("HD")];
OsuDifficultyAttributes diffAttributes = calculator.CalculateDifficulty(mods);
OsuPerformanceAttributes perfAttributes = calculator.CalculatePerformance(diffAttributes, new OsuScore(mods, 587));

Console.WriteLine("Star Rating: " + diffAttributes.StarRating);
Console.WriteLine("Total PP: " + perfAttributes.Total);
mods = [new Mod("CL")];

diffAttributes = calculator.CalculateDifficulty(mods);
perfAttributes = calculator.CalculatePerformance(diffAttributes, new OsuScore(mods, 587));

Console.WriteLine("Star Rating: " + diffAttributes.StarRating);
Console.WriteLine("Total PP: " + perfAttributes.Total);