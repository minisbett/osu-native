using osu.Game.Rulesets;
using osu.Native.Compiler;
using System.Runtime.InteropServices.Marshalling;

namespace osu.Native.Objects;

internal unsafe partial class RulesetObject : IOsuNativeObject<Ruleset>
{
  private static readonly AssemblyRulesetStore _rulesetStore = new();

  [OsuNativeField]
  private readonly int _rulesetId;

  private static NativeRuleset Create(RulesetInfo ruleset)
  {
    int objectId = ObjectContainer<Ruleset>.Add(ruleset.CreateInstance());
    return new()
    {
      ObjectId = objectId,
      RulesetId = ruleset.OnlineID
    };
  }

  [OsuNativeFunction]
  private static ErrorCode CreateFromId(int rulesetId, NativeRuleset* nativeRuleset)
  {
    RulesetInfo? ruleset = _rulesetStore.GetRuleset(rulesetId);
    if (ruleset is null || !ruleset.Available)
      return ErrorCode.RulesetUnavailable;

    *nativeRuleset = Create(ruleset);

    return ErrorCode.Success;
  }


  [OsuNativeFunction]
  private static ErrorCode CreateFromShortName(byte* shortName, NativeRuleset* nativeRuleset)
  {
    string shortNameStr = Utf8StringMarshaller.ConvertToManaged(shortName) ?? "";

    RulesetInfo? ruleset = _rulesetStore.GetRuleset(shortNameStr);
    if (ruleset is null || !ruleset.Available)
      return ErrorCode.RulesetUnavailable;

    *nativeRuleset = Create(ruleset);

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  private static ErrorCode GetShortName(NativeRuleset nativeRuleset, byte* buffer, int* bufferSize)
    => NativeHelper.StringBuffer(nativeRuleset.Resolve().ShortName, buffer, bufferSize);
}