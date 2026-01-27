using System.Runtime.InteropServices.Marshalling;
using osu.Game.Rulesets;
using osu.Native.Compiler;
using osu.Native.Structures;

namespace osu.Native.Objects;

/// <summary>
/// Represents a <see cref="Ruleset"/>.
/// </summary>
internal unsafe partial class RulesetObject : IOsuNativeObject<Ruleset>
{
    private static readonly AssemblyRulesetStore _rulesetStore = new();

    private static NativeRuleset Create(RulesetInfo ruleset)
    {
        return new()
        {
            Handle = ManagedObjectStore.Store(ruleset.CreateInstance()),
            RulesetId = ruleset.OnlineID
        };
    }

    /// <summary>
    /// Creates an instance of a <see cref="Ruleset"/> with the specified ruleset ID from the assembly ruleset store.
    /// </summary>
    /// <param name="rulesetId">The ID of the ruleset.</param>
    /// <param name="rulesetPtr">A pointer to write the resulting native ruleset object to.</param>
    [OsuNativeFunction]
    private static ErrorCode CreateFromId(int rulesetId, NativeRuleset* rulesetPtr)
    {
        RulesetInfo? ruleset = _rulesetStore.GetRuleset(rulesetId);
        if (ruleset is null || !ruleset.Available)
            return ErrorCode.RulesetUnavailable;

        *rulesetPtr = Create(ruleset);

        return ErrorCode.Success;
    }

    /// <summary>
    /// Creates an instance of a <see cref="Ruleset"/> with the specified ruleset short name from the assembly ruleset store.
    /// </summary>
    /// <param name="shortName">The short name of the ruleset.</param>
    /// <param name="rulesetPtr">A pointer to write the resulting native ruleset object to.</param>
    [OsuNativeFunction]
    private static ErrorCode CreateFromShortName(byte* shortName, NativeRuleset* rulesetPtr)
    {
        string shortNameStr = Utf8StringMarshaller.ConvertToManaged(shortName) ?? "";

        RulesetInfo? ruleset = _rulesetStore.GetRuleset(shortNameStr);
        if (ruleset is null || !ruleset.Available)
            return ErrorCode.RulesetUnavailable;

        *rulesetPtr = Create(ruleset);

        return ErrorCode.Success;
    }

    /// <summary>
    /// Writes the short name of the ruleset to the provided buffer.
    /// </summary>
    /// <param name="rulesetHandle">The handle of the ruleset to retrieve the short name of.</param>
    /// <param name="buffer">The buffer to write the title into.</param>
    /// <param name="bufferSize">The size of the provided buffer.</param>
    [OsuNativeFunction]
    private static ErrorCode GetShortName(RulesetHandle rulesetHandle, byte* buffer, int* bufferSize)
      => BufferHelper.String(rulesetHandle.Resolve().ShortName, buffer, bufferSize);
}