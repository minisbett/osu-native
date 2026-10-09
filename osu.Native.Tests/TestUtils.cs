using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NUnit.Framework.Constraints;
using osu.Game.Online.API;
using osu.Native.Objects;
using osu.Native.Structures;
using osu.Native.Structures.Difficulty;

namespace osu.Native.Tests;

/// <summary>
/// Provides utility functionality for tests.
/// </summary>
internal static unsafe class TestUtils
{
    private static readonly SnakeCaseNamingStrategy _naming = new();
    private static readonly JsonSerializer _serializer = JsonSerializer.Create(new()
    {
        ContractResolver = new DefaultContractResolver { NamingStrategy = _naming }
    });

    /// <summary>
    /// Returns the binary content of the source file with the specified name.
    /// </summary>
    /// <param name="name">The resource name.</param>
    /// <returns>The resource binary.</returns>
    public static byte[] GetResource(string name)
    {
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"osu.Native.Tests.Resources.{name.Replace("/", ".")}")!;
        using MemoryStream ms = new();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    /// <summary>
    /// Returns the handle to a newly-created <see cref="NativeBeatmap"/>, parsed from the beatmap in the embedded resources with the specified filename.
    /// </summary>
    /// <param name="filename">The filename in the embedded resources.</param>
    /// <returns>The handle to the beatmap.</returns>
    public static NativeBeatmap CreateBeatmap(string filename)
    {
        byte[] beatmap = GetResource(filename);
        NativeBeatmap nativeBeatmap;
        fixed (byte* ptr = beatmap)
            BeatmapObject.CreateFromText(ptr, &nativeBeatmap);

        return nativeBeatmap;
    }

    /// <summary>
    /// Returns the handle to a newly-created <see cref="NativeModsCollection"/> holding the mods in the specified mod string (eg. HDDT).
    /// If <see langword="null"/> is specified, a null-handle is returned.
    /// </summary>
    /// <param name="mods">The mod string.</param>
    /// <returns>The handle to the mods collection.</returns>
    public static NativeModsCollection CreateNativeModsCollection(string? mods)
    {
        if (mods is null)
            return new() { Handle = new(0) };

        NativeModsCollection nativeModsCollection;
        ModsCollectionObject.Create(&nativeModsCollection);

        foreach (string acronym in mods.Chunk(2).Select(x => new string(x)))
        {
            NativeMod nativeMod;
            ModObject.Create(Utf8StringMarshaller.ConvertToUnmanaged(acronym), &nativeMod);
            ModsCollectionObject.Add(nativeModsCollection.Handle, nativeMod.Handle);
        }

        return nativeModsCollection;
    }

    /// <summary>
    /// Performs an assertion for equality on every field of the actual and expected attributes. If the field is a <see langword="float"/> or <see langword="double"/>,
    /// a tolerance in order to counter floating point inaccuracies between systems these tests run on is added.
    /// </summary>
    /// <param name="actual">The actual attributes.</param>
    /// <param name="expected">The expected attributes.</param>
    public static void AssertEqualAttributes(object actual, JObject expected)
    {
        Assert.Multiple(() =>
        {
            FieldInfo[] fields = actual.GetType().GetFields();
            Assert.That(expected.Count, Is.EqualTo(fields.Length));

            foreach (FieldInfo field in fields)
            {
                string name = _naming.GetPropertyName(field.Name, false);
                if (expected[name] is JObject nested)
                {
                    AssertEqualAttributes(field.GetValue(actual)!, nested);
                    continue;
                }

                object? value = expected[name]!.ToObject(field.FieldType, _serializer);
                EqualConstraint constraint = Is.EqualTo(value);
                if (value is float or double)
                    constraint = constraint.Within(0.00001);

                Assert.That(field.GetValue(actual), constraint, $"{field.Name} does not match.");
            }
        });
    }

    public static int GetRuleset<T>()
    {
        if (typeof(T) == typeof(NativeOsuDifficultyAttributes))
            return 0;
        if (typeof(T) == typeof(NativeTaikoDifficultyAttributes))
            return 1;
        if (typeof(T) == typeof(NativeCatchDifficultyAttributes))
            return 2;
        if (typeof(T) == typeof(NativeManiaDifficultyAttributes))
            return 3;

        throw new ArgumentException($"Unknown difficulty attributes: {typeof(T).Name}.");
    }

    public static IEnumerable<TestCaseData> GetCalculatorTestCases(int ruleset, string filename)
    {
        string path = Path.Combine(TestContext.CurrentContext.TestDirectory, filename);
        JArray cases = JArray.Parse(File.ReadAllText(path));

        foreach (JObject testCase in cases)
        {
            if (testCase.Value<int>("ruleset") != ruleset)
                continue;

            List<object> arguments =
            [
                testCase.Value<string>("beatmap")!,
                testCase["mods"]!.ToObject<List<APIMod>>()!
            ];
            if (testCase["score"] is JObject score)
                arguments.Add(score.ToObject<NativeScoreInfo>(_serializer));
            else if (testCase["index"] is JToken index)
                arguments.Add(index.Value<int>());

            arguments.Add(testCase["expected"]!);

            yield return new TestCaseData(arguments.ToArray());
        }
    }
}