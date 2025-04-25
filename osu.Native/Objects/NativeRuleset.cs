using osu.Game.Rulesets;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Taiko;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace osu.Native.Objects;

internal readonly partial struct NativeRuleset : INativeObject<Ruleset>
{
  private static readonly AssemblyRulesetStore _rulesetStore = new();

  public int Id { get; init; }

  public int RulesetId { get; init; }

  [UnmanagedCallersOnly(EntryPoint = "Ruleset_Create", CallConvs = [typeof(CallConvCdecl)])]
  private static unsafe ErrorCode Create(int rulesetId, NativeRuleset* nativeRuleset)
  {
    RulesetInfo? ruleset = _rulesetStore.GetRuleset(rulesetId);
    if(ruleset is null || !ruleset.Available)
      return ErrorHandler.Handle(ErrorCode.RulesetUnavailable, $"A ruleset with ID {rulesetId} is unavailable.");

    int id = ObjectContainer<Ruleset>.Add(ruleset.CreateInstance());
    *nativeRuleset = new NativeRuleset
    {
      Id = id,
      RulesetId = rulesetId
    };

    return ErrorCode.Success;
  }
}