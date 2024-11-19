#pragma warning disable CS8321

using osu.Native.Bindings;


string osuFile = @"C:\Users\mini\Desktop\test.osu";


TaikoDifficultyCalculator calculator = new TaikoDifficultyCalculator(new FileInfo(osuFile));

TaikoDifficultyAttributes diffAttributes = calculator.CalculateDifficulty(0);
TaikoPerformanceAttributes perfAttributes = calculator.CalculatePerformance(diffAttributes, 0, new TaikoScore() { MaxCombo = 581 });

Console.WriteLine("Star Rating: " + diffAttributes.StarRating);
Console.WriteLine("Total PP: " + perfAttributes.Total);