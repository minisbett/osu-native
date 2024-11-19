using System.Runtime.InteropServices;

namespace osu.Native.Bindings;

#region Difficulty Attributes

public interface IDifficultyAttributes { }

[StructLayout(LayoutKind.Sequential)]
public struct OsuDifficultyAttributes : IDifficultyAttributes
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
public struct TaikoDifficultyAttributes : IDifficultyAttributes
{
    public double StarRating;
    public int MaxCombo;
    public double StaminaDifficulty;
    public double RhythmDifficulty;
    public double ColourDifficulty;
    public double PeakDifficulty;
    public double GreatHitWindow;
    public double OkHitWindow;
}

[StructLayout(LayoutKind.Sequential)]
public struct CatchDifficultyAttributes : IDifficultyAttributes
{
    public double StarRating;
    public int MaxCombo;
    public double ApproachRate;
}

[StructLayout(LayoutKind.Sequential)]
public struct ManiaDifficultyAttributes : IDifficultyAttributes
{
    public double StarRating;
    public int MaxCombo;
    public double GreatHitWindow;
}

#endregion

#region Performance Attributes

public interface IPerformanceAttributes { }

[StructLayout(LayoutKind.Sequential)]
public struct OsuPerformanceAttributes : IPerformanceAttributes
{
    public double Total;
    public double Aim;
    public double Speed;
    public double Accuracy;
    public double Flashlight;
    public double EffectiveMissCount;
}

[StructLayout(LayoutKind.Sequential)]
public struct TaikoPerformanceAttributes : IPerformanceAttributes
{
    public double Total;
    public double Difficulty;
    public double Accuracy;
    public double EffectiveMissCount;
    public double EstimatedUnstableRate;
}

[StructLayout(LayoutKind.Sequential)]
public struct CatchPerformanceAttributes : IPerformanceAttributes
{
    public double Total;
}

[StructLayout(LayoutKind.Sequential)]
public struct ManiaPerformanceAttributes : IPerformanceAttributes
{
    public double Total;
    public double Difficulty;
}

#endregion

#region Scores

public interface IScore { }

[StructLayout(LayoutKind.Sequential)]
public struct OsuScore : IScore
{
    public int MaxCombo;
    public int Count100;
    public int Count50;
    public int CountMiss;
    public int CountLargeTickMiss;
    public int CountSliderTailMiss;
}

[StructLayout(LayoutKind.Sequential)]
public struct TaikoScore : IScore
{
    public int MaxCombo;
    public int CountGood;
    public int CountMiss;
}

[StructLayout(LayoutKind.Sequential)]
public struct CatchScore : IScore
{
    public int MaxCombo;
    public int CountDroplets;
    public int CountTinyDroplets;
    public int CountTinyMisses;
    public int CountMiss;
}

[StructLayout(LayoutKind.Sequential)]
public struct ManiaScore : IScore
{
    public int MaxCombo;
    public int CountGreat;
    public int CountGood;
    public int CountOk;
    public int CountMeh;
    public int CountMiss;
}

#endregion