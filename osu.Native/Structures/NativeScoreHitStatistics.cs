using osu.Game.Rulesets.Scoring;

namespace osu.Native.Structures;

/// <summary>
/// Contains all <see cref="HitResult"/> for a score that are relevant for difficulty and performance calculations.
/// </summary>
internal struct NativeScoreHitStatistics
{
  public int Miss;
  public int Meh;
  public int Ok;
  public int Good;
  public int Great;
  public int Perfect;
  public int SliderTailHit;
  public int LargeTickMiss;

  /// <summary>
  /// Converts the struct data into a <see cref="Dictionary{HitResult, int}"/> for easier access and manipulation.
  /// </summary>
  public readonly Dictionary<HitResult, int> ToDictionary()
  {
    return new Dictionary<HitResult, int>
    {
      { HitResult.Miss, Miss },
      { HitResult.Meh, Meh },
      { HitResult.Ok, Ok },
      { HitResult.Good, Good },
      { HitResult.Great, Great },
      { HitResult.Perfect, Perfect },
      { HitResult.SliderTailHit, SliderTailHit },
      { HitResult.LargeTickMiss, LargeTickMiss }
    };
  }
}
