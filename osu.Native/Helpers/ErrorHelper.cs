// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using osu.Native.Objects;

namespace osu.Native.Helpers;

/// <summary>
/// Provides helper methods for the <see cref="ErrorCode"/> enumerable.
/// </summary>
public static class ErrorHelper
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