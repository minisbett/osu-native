#pragma warning disable CS8321

using osu.Native.Bindings;
using System.Text.Json;


string osuFile = @"C:\Users\mini\Desktop\test.osu";


OsuDifficultyCalculator calculator = new OsuDifficultyCalculator(new FileInfo(osuFile));

string mods = JsonSerializer.Serialize(new[]
{
    new
    {
        acronym = "DT"
    }
});

OsuDifficultyAttributes diffAttributes = calculator.CalculateDifficulty(mods);
OsuPerformanceAttributes perfAttributes = calculator.CalculatePerformance(diffAttributes, new OsuScore() { MaxCombo = 587, Mods = mods });

Console.WriteLine("Star Rating: " + diffAttributes.StarRating);
Console.WriteLine("Total PP: " + perfAttributes.Total);

// TODO: implement artifacts publish thing for osu.Native and osu.Native.Bindings
//       proper mod support
//       implement logger in osu.Native.Bindings
//       implement error handling in osu.Native.Bindings
//       implement tests