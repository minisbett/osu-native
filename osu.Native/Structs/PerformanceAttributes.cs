using System.Runtime.InteropServices;

namespace osu.Native.Structs;

[StructLayout(LayoutKind.Sequential)]
public struct NativeOsuPerformanceAttributes
{
    public double Total;
    public double Aim;
    public double Speed;
    public double Accuracy;
    public double Flashlight;
    public double EffectiveMissCount;
}

[StructLayout(LayoutKind.Sequential)]
public struct NativeTaikoPerformanceAttributes
{
    public double Total;
    public double Difficulty;
    public double Accuracy;
    public double EffectiveMissCount;
    public double? EstimatedUnstableRate;
}

[StructLayout(LayoutKind.Sequential)]
public struct NativeCatchPerformanceAttributes
{
    public double Total;
}

[StructLayout(LayoutKind.Sequential)]
public struct NativeManiaPerformanceAttributes
{
    public double Total;
    public double Difficulty;
}