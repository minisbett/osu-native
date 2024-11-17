using osu.Game.Rulesets.Osu.Difficulty;
using System.Runtime.InteropServices;

namespace osu.Native.Structs;

public interface IHitStatistics { }

[StructLayout(LayoutKind.Sequential)]
public struct OsuHitStatistics : IHitStatistics
{
    public int Count100;
    public int Count50;
    public int CountMiss;
    public int CountLargeTickMiss;
    public int CountSliderTailMiss;
}

[StructLayout(LayoutKind.Sequential)]
public struct TaikoHitStatistics : IHitStatistics
{
    public int CountGood;
    public int CountMiss;
}

[StructLayout(LayoutKind.Sequential)]
public struct CatchHitStatistics : IHitStatistics
{
    public int CountDroplets;
    public int CountTinyDroplets;
    public int CountTinyMisses;
    public int CountMiss;
}

[StructLayout(LayoutKind.Sequential)]
public struct ManiaHitStatistics : IHitStatistics
{
    public int CountGreat;
    public int CountGood;
    public int CountOk;
    public int CountMeh;
    public int CountMiss;
}