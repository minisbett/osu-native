using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;


unsafe
{
  Native.Beatmap_CreateFromFile(@"C:\Users\mini\Desktop\a.osu", out NativeBeatmap nativeBeatmap);
  Console.WriteLine($"ID: {nativeBeatmap.Handle}");
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

  error = Native.Mod_Create("DT", out int modHandle);
  Console.WriteLine($"Error code: {error}");
  Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");
  error = Native.Mod_Create("EZ", out int modHandle2);
  Console.WriteLine($"Error code: {error}");
  Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");
  error = Native.Mod_SetSetting(modHandle, "speed_change", 1.3);
  Console.WriteLine($"Error code: {error}");
  Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");

  error = Native.ModsCollection_Create(out int modsHandle);
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

  Native.Ruleset_CreateFromId(0, out NativeRuleset nativeRuleset);
  Console.WriteLine($"Ruleset ID: {nativeRuleset.RulesetId}");

  error = Native.OsuDifficultyCalculator_Create(nativeRuleset.Handle, nativeBeatmap.Handle, out int osuDifficultyCalculatorHandle);
  Console.WriteLine($"Error code: {error}");
  Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");
  error = Native.OsuDifficultyCalculator_Calculate(osuDifficultyCalculatorHandle, out NativeOsuDifficultyAttributes attributes);
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

  error = Native.OsuDifficultyCalculator_CalculateMods(osuDifficultyCalculatorHandle, nativeRuleset.Handle, modsHandle, out attributes);
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

  Native.Ruleset_CreateFromId(2, out NativeRuleset nativeRuleset2);
  Console.WriteLine($"Ruleset ID: {nativeRuleset2.RulesetId}");

  error = Native.CatchDifficultyCalculator_Create(nativeRuleset2.Handle, nativeBeatmap.Handle, out int catchDifficultyCalculatorHandle);
  Console.WriteLine($"Error code: {error}");
  Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");
  error = Native.CatchDifficultyCalculator_Calculate(catchDifficultyCalculatorHandle, out NativeCatchDifficultyAttributes catchAttributes);
  Console.WriteLine($"Error code: {error}");
  Console.WriteLine($"Error message: {Native.ErrorHandler_GetLastMessage()}");
  Console.WriteLine($"StarRating: {catchAttributes.StarRating}");
  Console.WriteLine($"Max Combo: {catchAttributes.MaxCombo}");
}

public struct NativeBeatmap
{
  public int Handle;
  public float ApproachRate;
  public float DrainRate;
  public float OverallDifficulty;
  public float CircleSize;
  public double SliderMultiplier;
  public double SliderTickRate;
}

public struct NativeRuleset
{
  public int Handle;
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
  public double AimDifficultStrainCount;
  public double SpeedDifficultStrainCount;
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

public static unsafe partial class Native
{
  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll")]
  [return: MarshalUsing(typeof(Utf8NoFreeStringMarshaller))]
  public static partial string ErrorHandler_GetLastMessage();

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll",
    StringMarshalling = StringMarshalling.Utf8)]
  public static partial sbyte Beatmap_CreateFromFile(string file, out NativeBeatmap nativeBeatmap);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll")]
  public static partial sbyte Beatmap_GetTitle(int beatmapHandle, byte* buffer, int* bufferSize);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll")]
  public static partial sbyte Beatmap_GetArtist(int beatmapHandle, byte* buffer, int* bufferSize);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll")]
  public static partial sbyte Beatmap_GetVersion(int beatmapHandle, byte* buffer, int* bufferSize);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll",
    StringMarshalling = StringMarshalling.Utf8)]
  public static partial sbyte Mod_Create(string acronym, out int modHandle);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll",
    StringMarshalling = StringMarshalling.Utf8)]
  public static partial sbyte Mod_SetSetting(int modHandle, string key, double value);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll")]
  public static partial sbyte ModsCollection_Create(out int modsHandle);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll")]
  public static partial sbyte ModsCollection_Add(int modsHandle, int modHandle);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll")]
  public static partial sbyte ModsCollection_Remove(int modsHandle, int modHandle);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll")]
  public static partial sbyte Mod_Debug(int modHandle);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll")]
  public static partial sbyte ModsCollection_Debug(int modsHandle);

  [LibraryImport( "C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll")]
  public static partial sbyte OsuDifficultyCalculator_Create(int rulesetHandle, int beatmapHandle, out int osuDifficultyCalculatorHandle);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll")]
  public static partial sbyte OsuDifficultyCalculator_Calculate(int osuDifficultyCalculatorHandle, out NativeOsuDifficultyAttributes attributes);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll")]
  public static partial sbyte OsuDifficultyCalculator_CalculateMods(int osuDifficultyCalculatorHandle, int rulesetHandle, int modsHandle, out NativeOsuDifficultyAttributes attributes);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll")]
  public static partial sbyte Ruleset_CreateFromId(int rulesetId, out NativeRuleset ruleset);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll")]
  public static partial sbyte CatchDifficultyCalculator_Create(int rulesetHandle, int beatmapHandle, out int catchDifficultyCalculatorHandle);

  [LibraryImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\native\\osu.Native.dll")]
  public static partial sbyte CatchDifficultyCalculator_Calculate(int catchDifficultyCalculatorHandle, out NativeCatchDifficultyAttributes attributes);
}

[CustomMarshaller(typeof(string), MarshalMode.Default, typeof(Utf8NoFreeStringMarshaller))]
public static unsafe class Utf8NoFreeStringMarshaller
{
  public static string? ConvertToManaged(byte* unmanaged) => Utf8StringMarshaller.ConvertToManaged(unmanaged);
}