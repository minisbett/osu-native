namespace osu.Native;

/// <summary>
/// Contains all error codes that can be returned from unmanaged functions.
/// </summary>
internal enum ErrorCode : byte
{
  /// <summary>
  /// Indicates a successful operation.
  /// </summary>
  Success = 0,

  /// <summary>
  /// Indicates that the managed object referenced by a native object was not found.
  /// </summary>
  ObjectNotFound = 1,

  /// <summary>
  /// Indicates that the required size of a buffer was queried.
  /// </summary>
  BufferSizeQuery = 2,

  /// <summary>
  /// Indicates that the size of the provided buffer was too small to hold the requested data.
  /// </summary>
  BufferTooSmall = 3,

  /// <summary>
  /// Indicates that the ruleset requested is not available (eg. not found).
  /// </summary>
  RulesetUnavailable = 3,

  /// <summary>
  /// Indicates an unspecific operation failure.
  /// </summary>
  Failure = 255
}