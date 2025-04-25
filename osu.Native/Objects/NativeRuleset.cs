using osu.Game.Rulesets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace osu.Native.Objects;

internal partial struct NativeRuleset : INativeObject<Ruleset>
{
  private static readonly AssemblyRulesetStore _rulesetStore = new();

  public int ObjectId { get; private init; }

  public int Id { get; private init; }

  private static NativeRuleset Create(RulesetInfo ruleset)
  {
    int objectId = ObjectContainer<Ruleset>.Add(ruleset.CreateInstance());
    return new NativeRuleset
    {
      ObjectId = objectId,
      Id = ruleset.OnlineID
    };
  }

  [UnmanagedCallersOnly(EntryPoint = "Ruleset_CreateFromId", CallConvs = [typeof(CallConvCdecl)])]
  private static unsafe ErrorCode CreateFromId(int rulesetId, NativeRuleset* nativeRuleset)
  {
    RulesetInfo? ruleset = _rulesetStore.GetRuleset(rulesetId);
    if (ruleset is null || !ruleset.Available)
      return ErrorCode.RulesetUnavailable;

    *nativeRuleset = Create(ruleset);

    return ErrorCode.Success;
  }


  [UnmanagedCallersOnly(EntryPoint = "Ruleset_CreateFromShortName", CallConvs = [typeof(CallConvCdecl)])]
  private static unsafe ErrorCode CreateFromShortName(char* shortName, NativeRuleset* nativeRuleset)
  {
    string shortNameStr = new(shortName);

    RulesetInfo? ruleset = _rulesetStore.GetRuleset(shortNameStr);
    if (ruleset is null || !ruleset.Available)
      return ErrorCode.RulesetUnavailable;

    *nativeRuleset = Create(ruleset);

    return ErrorCode.Success;
  }

  [UnmanagedCallersOnly(EntryPoint = "Ruleset_GetShortName", CallConvs = [typeof(CallConvCdecl)])]
  private static unsafe ErrorCode GetShortName(NativeRuleset nativeRuleset, char* shortNameBuffer, int* shortNameBufferSize)
  {
    try
    {
      return BufferHelper.String(nativeRuleset.Resolve().ShortName, shortNameBuffer, shortNameBufferSize);
    }
    catch (Exception ex)
    {
      return ErrorHandler.Handle(ex);
    }
  }
}