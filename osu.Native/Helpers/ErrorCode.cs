// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#if !IS_BINDINGS
using System;
using System.IO;
using osu.Native.Objects;
#endif

namespace osu.Native.Helpers;

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
    /// Indicates that the object referenced by a native object was not found.
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

#if !IS_BINDINGS
/// <summary>
/// Provides helper methods for the <see cref="ErrorCode"/> enumerable.
/// </summary>
public static class ErrorCodeHelper
{
    /// <summary>
    /// Returns the error code associated with the specified exception. If no special rule is defined, <see cref="ErrorCode.Failure"/> is returned.
    /// </summary>
    /// <param name="ex">The exception.</param>
    /// <returns>The associated error code.</returns>
    public static ErrorCode FromException(Exception ex)
    {
        return ex switch
        {
            FileNotFoundException => ErrorCode.FileNotFound,
            ObjectNotFoundException => ErrorCode.ObjectNotFound,
            ModsParsingFailedException => ErrorCode.ModsParsingFailed,
            _ => ErrorCode.Failure
        };
    }
}
#endif