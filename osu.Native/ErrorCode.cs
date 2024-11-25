// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Native;

/// <summary>
/// Contains all error codes that can be returned from unmanaged functions.
/// </summary>
public enum ErrorCode : byte
{
    /// <summary>
    /// Indicates a successful operation.
    /// </summary>
    Success = 0,

    /// <summary>
    /// Indicates that a file was not found.
    /// </summary>
    FileNotFound = 1,

    /// <summary>
    /// Indicates that the managed object referenced by a native object was not found.
    /// </summary>
    ObjectNotFound = 2,

    /// <summary>
    /// Indicates that mods parsing failed.
    /// </summary>
    ModsParsingFailed = 3,

    /// <summary>
    /// Indicates an unspecific operation failure.
    /// </summary>
    Failure = 255
}