using osu.Game.Online.API;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using osu.Native.Objects;

namespace osu.Native.Structures;

/// <summary>
/// Holds all information about a score that are relevant for performance calculations.
/// </summary>
internal struct NativeScore
{
  public ManagedObjectHandle<ModsCollection> ModsHandle;
  public int MaxCombo;
  public double Accuracy;
  public int CountMiss;
  public int CountMeh;
  public int CountOk;
  public int CountGood;
  public int CountGreat;
  public int CountPerfect;
  public int CountSliderTailHit;
  public int CountLargeTickMiss;

  /// <summary>
  /// Constructs a <see cref="ScoreInfo"/> from the native score information.
  /// </summary>
  /// <param name="ruleset">The ruleset instance used to convert the native mods to their managed implementation.</param>
  /// <returns>The constructed <see cref="ScoreInfo"/>.</returns>
  public readonly ScoreInfo ToScoreInfo(Ruleset ruleset)
  {
    Mod[] mods = [.. ModsHandle.Resolve().Select(x => x.ToMod(ruleset))];

    return new ScoreInfo
    {
      Mods = mods,
      MaxCombo = MaxCombo,
      Accuracy = Accuracy,
      Statistics =
      {
        [HitResult.Miss] = CountMiss,
        [HitResult.Meh] = CountMeh,
        [HitResult.Ok] = CountOk,
        [HitResult.Good] = CountGood,
        [HitResult.Great] = CountGreat,
        [HitResult.Perfect] = CountPerfect,
        [HitResult.SliderTailHit] = CountSliderTailHit,
        [HitResult.LargeTickMiss] = CountLargeTickMiss
      }
    };
  }
}
