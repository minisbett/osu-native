using osu.Game.Rulesets;

namespace osu.Native.Objects;

internal unsafe readonly partial struct NativeRuleset : INativeObject<Ruleset>
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

  [OsuNativeObject]
  private static ErrorCode CreateFromId(int rulesetId, NativeRuleset* nativeRuleset)
  {
    RulesetInfo? ruleset = _rulesetStore.GetRuleset(rulesetId);
    if (ruleset is null || !ruleset.Available)
      return ErrorCode.RulesetUnavailable;

    *nativeRuleset = Create(ruleset);

    return ErrorCode.Success;
  }


  [OsuNativeObject]
  private static ErrorCode CreateFromShortName(char* shortName, NativeRuleset* nativeRuleset)
  {
    string shortNameStr = new(shortName);

    RulesetInfo? ruleset = _rulesetStore.GetRuleset(shortNameStr);
    if (ruleset is null || !ruleset.Available)
      return ErrorCode.RulesetUnavailable;

    *nativeRuleset = Create(ruleset);

    return ErrorCode.Success;
  }

  [OsuNativeObject]
  private static ErrorCode GetShortName(NativeRuleset nativeRuleset, char* shortNameBuffer, int* shortNameBufferSize)
    => BufferHelper.String(nativeRuleset.Resolve().ShortName, shortNameBuffer, shortNameBufferSize);
}