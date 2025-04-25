using osu.Game.Rulesets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace osu.Native.Objects;

internal partial struct NativeRuleset : INativeObject<Ruleset>
{
  private static readonly AssemblyRulesetStore _rulesetStore = new();

  public int Id { get; init; }

  private int _rulesetId;

  [UnmanagedCallersOnly(EntryPoint = "Ruleset_CreateFromId", CallConvs = [typeof(CallConvCdecl)])]
  private static unsafe ErrorCode CreateFromId(int rulesetId, NativeRuleset* nativeRuleset)
  {
    RulesetInfo? ruleset = _rulesetStore.GetRuleset(rulesetId);
    if (ruleset is null || !ruleset.Available)
      return ErrorHandler.Handle(ErrorCode.RulesetUnavailable, $"A ruleset with ID {rulesetId} is unavailable.");

    int id = ObjectContainer<Ruleset>.Add(ruleset.CreateInstance());
    *nativeRuleset = new NativeRuleset
    {
      Id = id,
      _rulesetId = rulesetId
    };

    return ErrorCode.Success;
  }


  [UnmanagedCallersOnly(EntryPoint = "Ruleset_CreateFromShortName", CallConvs = [typeof(CallConvCdecl)])]
  private static unsafe ErrorCode CreateFromShortName(char* shortName, NativeRuleset* nativeRuleset)
  {
    string shortNameStr = new(shortName);

    RulesetInfo? ruleset = _rulesetStore.GetRuleset(shortNameStr);
    if (ruleset is null || !ruleset.Available)
      return ErrorHandler.Handle(ErrorCode.RulesetUnavailable, $"A ruleset with short name {shortNameStr} is unavailable.");

    int id = ObjectContainer<Ruleset>.Add(ruleset.CreateInstance());
    *nativeRuleset = new NativeRuleset
    {
      Id = id,
      _rulesetId = ruleset.OnlineID
    };

    return ErrorCode.Success;
  }

  [UnmanagedCallersOnly(EntryPoint = "Ruleset_GetShortName", CallConvs = [typeof(CallConvCdecl)])]
  private static unsafe ErrorCode GetShortName(NativeRuleset nativeRuleset, char* shortNameBuffer, int* shortNameBufferSize)
  {
    Ruleset ruleset = nativeRuleset.Resolve();

    if(shortNameBuffer is null)
    {
      *shortNameBufferSize = ruleset.ShortName.Length + 1;
      return ErrorCode.BufferSizeQuery;
    }

    if (ruleset.ShortName.Length + 1 > *shortNameBufferSize)
      return ErrorHandler.Handle(ErrorCode.BufferTooSmall, $"Expected: {ruleset.ShortName.Length + 1}, Actual: {*shortNameBufferSize}");

    ruleset.ShortName.AsSpan().CopyTo(new(shortNameBuffer, *shortNameBufferSize));
    shortNameBuffer[ruleset.ShortName.Length] = '\0';

    return ErrorCode.Success;
  }
}