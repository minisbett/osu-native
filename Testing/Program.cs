#pragma warning disable CS8321

using osu.Native.Bindings;


string osuFile = @"C:\Users\mini\Desktop\test.osu";


OsuDifficultyCalculator calculator = new OsuDifficultyCalculator(new FileInfo(osuFile));

OsuDifficultyAttributes diffAttributes = calculator.CalculateDifficulty();
OsuPerformanceAttributes perfAttributes = calculator.CalculatePerformance(diffAttributes, new OsuScore() { MaxCombo = 587 });

Console.WriteLine("Star Rating: " + diffAttributes.StarRating);
Console.WriteLine("Total PP: " + perfAttributes.Total);