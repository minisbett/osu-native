using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using osu.Game.Beatmaps;
using osu.Game.IO;
using osu.Game.Online.API;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Taiko;
using osu.Game.Scoring;
using osu.Native.Structures.Difficulty;
using Decoder = osu.Game.Beatmaps.Formats.Decoder;

namespace osu.Native.Tests;

internal static class TestCaseGenerator
{
    private static readonly SnakeCaseNamingStrategy _naming = new();

    public static void Main(string[] args)
    {
        string[] files = ["difficulty.json", "timed-difficulty.json", "performance.json"];
        foreach (string filename in files)
        {
            JArray cases = JArray.Parse(Encoding.UTF8.GetString(TestUtils.GetResource(filename)));
            foreach (JObject testCase in cases)
                testCase["expected"] = CalculateExpected(testCase);

            File.WriteAllText(Path.Combine(args[0], filename), cases.ToString(Formatting.Indented) + Environment.NewLine);
        }
    }

    private static JObject CalculateExpected(JObject testCase)
    {
        Ruleset ruleset = testCase.Value<int>("ruleset") switch
        {
            0 => new OsuRuleset(),
            1 => new TaikoRuleset(),
            2 => new CatchRuleset(),
            3 => new ManiaRuleset(),
            _ => throw new InvalidOperationException($"Unknown ruleset: {testCase["ruleset"]}")
        };

        using MemoryStream stream = new(TestUtils.GetResource(testCase.Value<string>("beatmap")!));
        using LineBufferedReader reader = new(stream);
        FlatWorkingBeatmap beatmap = new(Decoder.GetDecoder<Beatmap>(reader).Decode(reader));
        Mod[] mods = testCase["mods"]!.ToObject<List<APIMod>>()!.Select(mod => mod.ToMod(ruleset)).ToArray();
        DifficultyCalculator calculator = ruleset.CreateDifficultyCalculator(beatmap);
        if (testCase["index"] is JToken index)
        {
            TimedDifficultyAttributes timed = calculator.CalculateTimed(mods)[index.Value<int>()];
            return new()
            {
                ["time"] = timed.Time,
                ["attributes"] = GetAttributes(timed.Attributes)
            };
        }

        DifficultyAttributes difficulty = calculator.Calculate(mods);
        if (testCase["score"] is JObject input)
        {
            ScoreInfo score = CreateScore(input, ruleset, beatmap, mods);
            return GetAttributes(ruleset.CreatePerformanceCalculator()!.Calculate(score, difficulty));
        }

        return GetAttributes(difficulty);
    }

    private static ScoreInfo CreateScore(JObject input, Ruleset ruleset, FlatWorkingBeatmap beatmap, Mod[] mods)
    {
        ScoreInfo score = new(beatmap.BeatmapInfo, ruleset.RulesetInfo)
        {
            Mods = mods,
            MaxCombo = input.Value<int>("max_combo"),
            Accuracy = input.Value<double>("accuracy"),
            LegacyTotalScore = input.Value<long?>("legacy_total_score")
        };

        foreach (HitResult result in Enum.GetValues<HitResult>())
        {
            string name = "count_" + _naming.GetPropertyName(result.ToString(), false);
            if (input[name] is JToken count)
                score.Statistics[result] = count.Value<int>();
        }

        return score;
    }

    private static JObject GetAttributes(object attributes)
    {
        Type managedType = attributes.GetType();
        string category = attributes is DifficultyAttributes ? "Difficulty" : "Performance";
        string typeName = $"osu.Native.Structures.{category}.Native{managedType.Name}";
        Type nativeType = typeof(NativeOsuDifficultyAttributes).Assembly.GetType(typeName, throwOnError: true)!;
        JObject expected = new();

        foreach (FieldInfo field in nativeType.GetFields())
        {
            PropertyInfo property = managedType.GetProperty(field.Name)
                ?? throw new InvalidOperationException($"No managed attribute matches {field.Name}.");
            string name = _naming.GetPropertyName(field.Name, false);
            expected[name] = JToken.FromObject(property.GetValue(attributes) ?? JValue.CreateNull());
        }

        return expected;
    }
}