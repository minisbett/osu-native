#pragma warning disable CS8321

using System.Runtime.InteropServices;

const string lib = @"C:\Users\mini\Desktop\osu-native\artifacts\bin\osu.Native\release\native\osu.Native.dll";

[DllImport(lib, EntryPoint = "Beatmap_ParseFile", CallingConvention = CallingConvention.Cdecl)]
static extern int BeatmapParseFile([MarshalAs(UnmanagedType.LPStr)] string filePath, out int contextId);

[DllImport(lib, EntryPoint = "Beatmap_ParseText", CallingConvention = CallingConvention.Cdecl)]
static extern int BeatmapParseText([MarshalAs(UnmanagedType.LPStr)] string text, out int contextId);

[DllImport(lib, EntryPoint = "Difficulty_ComputeOsu", CallingConvention = CallingConvention.Cdecl)]
static extern int DifficultyComputeOsu(int beatmapContextId, uint mods, out OsuDifficultyAttributes attributes);

[DllImport(lib, EntryPoint = "Beatmap_Destroy", CallingConvention = CallingConvention.Cdecl)]
static extern int BeatmapDestroy(int contextId);

[DllImport(lib, EntryPoint = "Performance_ComputeOsu", CallingConvention = CallingConvention.Cdecl)]
static extern int PerformanceComputeOsu(int beatmapContextId, OsuDifficultyAttributes diffAttributes, uint mods, int combo, OsuHitStatistics hitStatistics, 
                                        out OsuPerformanceAttributes attributes);


string osuFile = @"C:\Users\mini\Desktop\test.osu";

OsuHitStatistics statistics = new OsuHitStatistics()
{
    
};

BeatmapParseFile(osuFile, out int id);
DifficultyComputeOsu(id, 0, out OsuDifficultyAttributes attributes);
int x = PerformanceComputeOsu(id, attributes, 0, 587, statistics, out OsuPerformanceAttributes perf);
BeatmapDestroy(id);

Console.WriteLine(x);
Console.WriteLine(perf.Total);


[StructLayout(LayoutKind.Sequential)]
public struct OsuDifficultyAttributes
{
    public double StarRating;
    public int MaxCombo;
    public double AimDifficulty;
    public double SpeedDifficulty;
    public double SpeedNoteCount;
    public double FlashlightDifficulty;
    public double SliderFactor;
    public double AimDifficultStrainCount;
    public double SpeedDifficultStrainCount;
    public double ApproachRate;
    public double OverallDifficulty;
    public double DrainRate;
    public int HitCircleCount;
    public int SliderCount;
    public int SpinnerCount;
}

[StructLayout(LayoutKind.Sequential)]
public struct OsuPerformanceAttributes
{
    public double Total;
    public double Aim;
    public double Speed;
    public double Accuracy;
    public double Flashlight;
    public double EffectiveMissCount;
}

[StructLayout(LayoutKind.Sequential)]
public struct OsuHitStatistics
{
    public int Count100;
    public int Count50;
    public int CountMiss;
    public int CountLargeTickMiss;
    public int CountSliderTailMiss;
}