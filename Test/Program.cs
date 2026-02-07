using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using Test;

//OsuDifficultyBenchmark.Main(args);

unsafe
{
    Native.Beatmap_CreateFromFile(@"C:\Users\mini\Desktop\test.osu", out NativeBeatmap nativeBeatmap);
    Console.WriteLine($"ID: {nativeBeatmap.Handle}");
    Console.WriteLine($"Ruleset ID: {nativeBeatmap.RulesetId}");
    Console.WriteLine($"AR: {nativeBeatmap.ApproachRate}");
    Console.WriteLine($"HP: {nativeBeatmap.DrainRate}");
    Console.WriteLine($"OD: {nativeBeatmap.OverallDifficulty}");
    Console.WriteLine($"CS: {nativeBeatmap.CircleSize}");
    Console.WriteLine($"SM: {nativeBeatmap.SliderMultiplier}");
    Console.WriteLine($"ST: {nativeBeatmap.SliderTickRate}");
    Console.WriteLine($"AR: {nativeBeatmap.ApproachRate}");

    nativeBeatmap.Handle = 2;

    int size;
    int error = Native.Beatmap_GetTitle(nativeBeatmap.Handle, null, &size);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

    nativeBeatmap.Handle = 1;

    error = Native.Beatmap_GetTitle(nativeBeatmap.Handle, null, &size);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

    byte[] buffer = new byte[size];
    fixed (byte* p = buffer)
        Native.Beatmap_GetTitle(nativeBeatmap.Handle, p, &size);

    Console.WriteLine($"Title: {Encoding.UTF8.GetString(buffer)}");

    error = Native.Mod_Create("DT", out uint modHandle);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");
    error = Native.Mod_Create("EZ", out uint modHandle2);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");
    error = Native.Mod_SetSettingFloat(modHandle, "speed_change", 1.3f);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

    error = Native.ModsCollection_Create(out uint modsHandle);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

    error = Native.ModsCollection_Add(modsHandle, modHandle);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

    error = Native.ModsCollection_Add(modsHandle, modHandle2);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

    Native.Mod_Debug(modHandle);
    Native.Mod_Debug(modHandle2);
    Native.ModsCollection_Debug(modsHandle);

    error = Native.ModsCollection_Remove(modsHandle, modHandle2);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

    Native.ModsCollection_Debug(modsHandle);

    error = Native.Ruleset_CreateFromId(0, out NativeRuleset nativeRuleset);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");
    Console.WriteLine($"Ruleset ID: {nativeRuleset.RulesetId}");

    error = Native.OsuDifficultyCalculator_Create(nativeRuleset.Handle, nativeBeatmap.Handle, out uint osuDifficultyCalculatorHandle);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");
    error = Native.OsuDifficultyCalculator_Calculate(osuDifficultyCalculatorHandle, 0, out NativeOsuDifficultyAttributes attributes);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");
    Console.WriteLine($"StarRating: {attributes.StarRating}");
    Console.WriteLine($"Max Combo: {attributes.MaxCombo}");
    Console.WriteLine($"Aim Difficulty: {attributes.AimDifficulty}");
    Console.WriteLine($"Aim Difficult Slider Count: {attributes.AimDifficultSliderCount}");
    Console.WriteLine($"Speed Difficulty: {attributes.SpeedDifficulty}");
    Console.WriteLine($"Speed Note Count: {attributes.SpeedNoteCount}");
    Console.WriteLine($"Flashlight Difficulty: {attributes.FlashlightDifficulty}");
    Console.WriteLine($"Slider Factor: {attributes.SliderFactor}");
    Console.WriteLine($"Aim Difficult Strain Count: {attributes.AimDifficultStrainCount}");
    Console.WriteLine($"Speed Difficult Strain Count: {attributes.SpeedDifficultStrainCount}");
    Console.WriteLine($"Drain Rate: {attributes.DrainRate}");
    Console.WriteLine($"Hit Circle Count: {attributes.HitCircleCount}");
    Console.WriteLine($"Slider Count: {attributes.SliderCount}");
    Console.WriteLine($"Spinner Count: {attributes.SpinnerCount}");

    error = Native.OsuDifficultyCalculator_Calculate(osuDifficultyCalculatorHandle, modsHandle, out NativeOsuDifficultyAttributes attributes2);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");
    Console.WriteLine($"StarRating: {attributes2.StarRating}");
    Console.WriteLine($"Max Combo: {attributes2.MaxCombo}");
    Console.WriteLine($"Aim Difficulty: {attributes2.AimDifficulty}");
    Console.WriteLine($"Aim Difficult Slider Count: {attributes2.AimDifficultSliderCount}");
    Console.WriteLine($"Speed Difficulty: {attributes2.SpeedDifficulty}");
    Console.WriteLine($"Speed Note Count: {attributes2.SpeedNoteCount}");
    Console.WriteLine($"Flashlight Difficulty: {attributes2.FlashlightDifficulty}");
    Console.WriteLine($"Slider Factor: {attributes2.SliderFactor}");
    Console.WriteLine($"Aim Difficult Strain Count: {attributes2.AimDifficultStrainCount}");
    Console.WriteLine($"Speed Difficult Strain Count: {attributes2.SpeedDifficultStrainCount}");
    Console.WriteLine($"Drain Rate: {attributes2.DrainRate}");
    Console.WriteLine($"Hit Circle Count: {attributes2.HitCircleCount}");
    Console.WriteLine($"Slider Count: {attributes2.SliderCount}");
    Console.WriteLine($"Spinner Count: {attributes2.SpinnerCount}");

    Native.Ruleset_CreateFromId(2, out NativeRuleset nativeRuleset2);
    Console.WriteLine($"Ruleset ID: {nativeRuleset2.RulesetId}");

    error = Native.CatchDifficultyCalculator_Create(nativeRuleset2.Handle, nativeBeatmap.Handle, out uint catchDifficultyCalculatorHandle);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");
    error = Native.CatchDifficultyCalculator_Calculate(catchDifficultyCalculatorHandle, 0, out NativeCatchDifficultyAttributes catchAttributes);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");
    Console.WriteLine($"StarRating: {catchAttributes.StarRating}");
    Console.WriteLine($"Max Combo: {catchAttributes.MaxCombo}");

    error = Native.OsuPerformanceCalculator_Create(out uint osuPerformanceCalculatorHandle);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

    error = Native.ModsCollection_Remove(modsHandle, modHandle);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

    Native.ModsCollection_Debug(modsHandle);

    NativeScoreInfo scoreInfo = new()
    {
        RulesetHandle = nativeRuleset.Handle,
        BeatmapHandle = nativeBeatmap.Handle,
        ModsHandle = modsHandle,
        MaxCombo = 342,
        Accuracy = 0.9304,
        CountGreat = 938,
        CountOk = 37,
        CountMeh = 2,
        CountMiss = 53,
        CountSliderTailHit = 299
    };

    error = Native.OsuPerformanceCalculator_Calculate(osuPerformanceCalculatorHandle, scoreInfo, attributes, out NativeOsuPerformanceAttributes performanceAttributes);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

    Console.WriteLine($"Total: {performanceAttributes.Total}");
    Console.WriteLine($"Aim: {performanceAttributes.Aim}");
    Console.WriteLine($"Speed: {performanceAttributes.Speed}");
    Console.WriteLine($"Accuracy: {performanceAttributes.Accuracy}");
    Console.WriteLine($"Flashlight: {performanceAttributes.Flashlight}");
    Console.WriteLine($"Effective Miss Count: {performanceAttributes.EffectiveMissCount}");
    if (performanceAttributes.SpeedDeviation.HasValue)
        Console.WriteLine($"Speed Deviation: {performanceAttributes.SpeedDeviation}");
    else
        Console.WriteLine("Speed Deviation: null");

    error = Native.Ruleset_CreateFromId(0, out NativeRuleset r);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");
    Native.Beatmap_CreateFromFile(@"C:\Users\mini\Desktop\w.osu", out NativeBeatmap b);
    error = Native.OsuDifficultyCalculator_Create(r.Handle, b.Handle, out uint d);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

    error = Native.OsuDifficultyCalculator_CalculateTimed(d, null, &size);
    Console.WriteLine($"Error code: {error}");
    Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

    NativeTimedOsuDifficultyAttributes[] attributesBuffer = new NativeTimedOsuDifficultyAttributes[size];
    fixed (NativeTimedOsuDifficultyAttributes* p = attributesBuffer)
        Native.OsuDifficultyCalculator_CalculateTimed(d, p, &size);

    ;
}

public struct NativeBeatmap
{
    public uint Handle;
    public int RulesetId;
    public int BeatmapId;
    public float ApproachRate;
    public float DrainRate;
    public float OverallDifficulty;
    public float CircleSize;
    public double SliderMultiplier;
    public double SliderTickRate;
}

public struct NativeRuleset
{
    public uint Handle;
    public int RulesetId;
}

public struct NativeOsuDifficultyAttributes
{
    public double StarRating;
    public int MaxCombo;
    public double AimDifficulty;
    public double AimDifficultSliderCount;
    public double SpeedDifficulty;
    public double SpeedNoteCount;
    public double FlashlightDifficulty;
    public double SliderFactor;
    public double AimTopWeightedSliderFactor;
    public double SpeedTopWeightedSliderFactor;
    public double AimDifficultStrainCount;
    public double SpeedDifficultStrainCount;
    public double NestedScorePerObject;
    public double LegacyScoreBaseMultiplier;
    public double MaximumLegacyComboScore;
    public double DrainRate;
    public int HitCircleCount;
    public int SliderCount;
    public int SpinnerCount;
}

public struct NativeCatchDifficultyAttributes
{
    public double StarRating;
    public int MaxCombo;
}

public struct NativeScoreInfo
{
    public uint RulesetHandle;
    public uint BeatmapHandle;
    public uint ModsHandle;
    public int MaxCombo;
    public double Accuracy;
    public long? LegacyTotalScore;
    public int CountMiss;
    public int CountMeh;
    public int CountOk;
    public int CountGood;
    public int CountGreat;
    public int CountPerfect;
    public int CountSliderTailHit;
    public int CountLargeTickMiss;
}

public struct NativeOsuPerformanceAttributes
{
    public double Total;
    public double Aim;
    public double Speed;
    public double Accuracy;
    public double Flashlight;
    public double EffectiveMissCount;
    public double? SpeedDeviation;
    public double ComboBasedEstimatedMissCount;
    public double? ScoreBasedEstimatedMissCount;
    public double AimEstimatedSliderBreaks;
    public double SpeedEstimatedSliderBreaks;
}

public struct NativeTimedOsuDifficultyAttributes
{
    public double Time;
    public NativeOsuDifficultyAttributes Attributes;
}


public static unsafe partial class Native
{
    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    [return: MarshalUsing(typeof(Utf8NoFreeStringMarshaller))]
    public static partial string ErrorHandler_GetLastMessage();

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll",
      StringMarshalling = StringMarshalling.Utf8)]
    public static partial sbyte Beatmap_CreateFromFile(string file, out NativeBeatmap nativeBeatmap);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    public static partial sbyte Beatmap_GetTitle(uint beatmapHandle, byte* buffer, int* bufferSize);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    public static partial sbyte Beatmap_GetArtist(uint beatmapHandle, byte* buffer, int* bufferSize);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    public static partial sbyte Beatmap_GetVersion(uint beatmapHandle, byte* buffer, int* bufferSize);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll",
      StringMarshalling = StringMarshalling.Utf8)]
    public static partial sbyte Mod_Create(string acronym, out uint modHandle);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll",
      StringMarshalling = StringMarshalling.Utf8)]
    public static partial sbyte Mod_SetSettingFloat(uint modHandle, string key, float value);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    public static partial sbyte ModsCollection_Create(out uint modsHandle);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    public static partial sbyte ModsCollection_Add(uint modsHandle, uint modHandle);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    public static partial sbyte ModsCollection_Remove(uint modsHandle, uint modHandle);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    public static partial sbyte Mod_Debug(uint modHandle);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    public static partial sbyte ModsCollection_Debug(uint modsHandle);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    public static partial sbyte OsuDifficultyCalculator_Create(uint rulesetHandle, uint beatmapHandle, out uint osuDifficultyCalculatorHandle);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    public static partial sbyte OsuDifficultyCalculator_Calculate(uint osuDifficultyCalculatorHandle, uint modsHandle, out NativeOsuDifficultyAttributes attributes);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    public static partial sbyte OsuDifficultyCalculator_CalculateTimed(uint osuDifficultyCalculatorHandle, NativeTimedOsuDifficultyAttributes* attributesBuffer, int* bufferSize);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    public static partial sbyte Ruleset_CreateFromId(uint rulesetId, out NativeRuleset ruleset);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    public static partial sbyte CatchDifficultyCalculator_Create(uint rulesetHandle, uint beatmapHandle, out uint catchDifficultyCalculatorHandle);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    public static partial sbyte CatchDifficultyCalculator_Calculate(uint catchDifficultyCalculatorHandle, uint modsHandle, out NativeCatchDifficultyAttributes attributes);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    public static partial sbyte OsuPerformanceCalculator_Create(out uint osuPerformanceCalculatorHandle);

    [LibraryImport(@"C:\Users\mini\source\repos\minisbett\osu-native\Artifacts\bin\osu.Native\release\native\osu.Native.dll")]
    public static partial sbyte OsuPerformanceCalculator_Calculate(uint osuPerformanceCalculatorHandle, NativeScoreInfo scoreInfo, NativeOsuDifficultyAttributes difficultyAttributes, out NativeOsuPerformanceAttributes attributes);
}

[CustomMarshaller(typeof(string), MarshalMode.Default, typeof(Utf8NoFreeStringMarshaller))]
public static unsafe class Utf8NoFreeStringMarshaller
{
    public static string? ConvertToManaged(byte* unmanaged) => Utf8StringMarshaller.ConvertToManaged(unmanaged);
}