#pragma warning disable CS8321

using osu.Native.Bindings;


string osuFile = @"C:\Users\mini\Desktop\test.osu";


OsuDifficultyCalculator calculator = new OsuDifficultyCalculator(new FileInfo(osuFile));

OsuDifficultyAttributes diffAttriutes = calculator.CalculateDifficulty(0);
OsuPerformanceAttributes perfAttributes = calculator.CalculatePerformance(diffAttriutes, 0, new OsuScore() { MaxCombo = 587, Count100 = 2 });

Console.WriteLine("Star Rating: " + diffAttriutes.StarRating);
Console.WriteLine("Total PP: " + perfAttributes.Total);