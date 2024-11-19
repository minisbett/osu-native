#pragma warning disable CS8321

using osu.Native.Bindings;


string osuFile = @"C:\Users\mini\Desktop\test.osu";


TaikoDifficultyCalculator calculator = new TaikoDifficultyCalculator(new FileInfo(osuFile));

TaikoDifficultyAttributes diffAttributes = calculator.CalculateDifficulty(0);
TaikoPerformanceAttributes perfAttributes = calculator.CalculatePerformance(diffAttributes, 0, new TaikoScore() { MaxCombo = 581 });

Console.WriteLine("[osu-native-bindings] Star Rating: " + diffAttributes.StarRating);
Console.WriteLine("[osu-native-bindings] Max Combo: " + diffAttributes.MaxCombo);
Console.WriteLine("[osu-native-bindings] Stamina Difficulty: " + diffAttributes.StaminaDifficulty);
Console.WriteLine("[osu-native-bindings] Mono Stamina Factor: " + diffAttributes.MonoStaminaFactor);
Console.WriteLine("[osu-native-bindings] Rhythm Difficulty: " + diffAttributes.RhythmDifficulty);
Console.WriteLine("[osu-native-bindings] Colour Difficulty: " + diffAttributes.ColourDifficulty);
Console.WriteLine("[osu-native-bindings] Peak Difficulty: " + diffAttributes.PeakDifficulty);
Console.WriteLine("[osu-native-bindings] Great Hit Window: " + diffAttributes.GreatHitWindow);
Console.WriteLine("[osu-native-bindings] Ok Hit Window: " + diffAttributes.OkHitWindow);

Console.WriteLine("[osu-native-bindings] Difficulty PP: " + perfAttributes.Difficulty);
Console.WriteLine("[osu-native-bindings] Accuracy PP: " + perfAttributes.Accuracy);
Console.WriteLine("[osu-native-bindings] Effective Misscount: " + perfAttributes.EffectiveMissCount);
Console.WriteLine("[osu-native-bindings] Estimated Unstable Rate: " + perfAttributes.EstimatedUnstableRate);
Console.WriteLine("[osu-native-bindings] Total PP: " + perfAttributes.Total);