namespace osu.Native;

/// <summary>
/// Contains all error codes that can be returned from unmanaged functions.<br/>
/// <![CDATA[< 0 = special codes, 0 = success, > 0 = error]]>
/// </summary>
public enum ErrorCode : sbyte
{
    /// <summary>
    /// Indicates that the required size of a buffer was queried.
    /// </summary>
    BufferSizeQuery = -1,

    /// <summary>
    /// Indicates a successful operation.
    /// </summary>
    Success = 0,

    /// <summary>
    /// Indicates that the object referenced by a managed object handle was not resolved.
    /// </summary>
    ObjectNotResolved = 1,

    /// <summary>
    /// Indicates that the ruleset requested is not available (eg. not found).
    /// </summary>
    RulesetUnavailable = 2,

    /// <summary>
    /// Indicates that the specified ruleset instance is not of the expected ruleset for the operation context.
    /// (eg. Osu ruleset instance passed to a Catch difficulty calculator)
    /// </summary>
    UnexpectedRuleset = 3,

    /// <summary>
    /// Indicates an unspecific operation failure.
    /// </summary>
    Failure = 127
}