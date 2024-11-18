using System;

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
    /// Indicates that a context in <see cref="Contexts"/> was not found.
    /// </summary>
    ContextNotFound = 2,

    /// <summary>
    /// Indicates an unspecific operation failure.
    /// </summary>
    Failure = 255
}

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
            ContextNotFoundException => ErrorCode.ContextNotFound,
            _ => ErrorCode.Failure
        };
    }
}