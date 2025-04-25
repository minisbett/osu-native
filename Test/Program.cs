using System.Runtime.InteropServices;

[DllImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll",
  EntryPoint = "Ruleset_CreateFromId", CallingConvention = CallingConvention.Cdecl)]
static extern byte Ruleset_CreateFromId(int rulesetId, out NativeRuleset nativeRuleset);


[DllImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll",
  EntryPoint = "Ruleset_GetShortName", CallingConvention = CallingConvention.Cdecl)]
static extern unsafe byte Ruleset_GetShortName(NativeRuleset ruleset, char* buffer, int* bufferSize);

[DllImport("C:\\Users\\mini\\source\\repos\\minisbett\\osu-native-new\\osu.Native\\bin\\Release\\net9.0\\win-x64\\native\\osu.Native.dll",
  EntryPoint = "Ruleset_Destroy", CallingConvention = CallingConvention.Cdecl)]
static extern unsafe byte Ruleset_Destroy(NativeRuleset ruleset);


byte e = Ruleset_CreateFromId(0, out NativeRuleset nativeRuleset);

Ruleset_Destroy(nativeRuleset);


public struct NativeRuleset
{
  public int ObjectId;
  public int RulesetId;
}