// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using osu.Native.Helpers;

namespace osu.Native.EntryPoints;

public unsafe static class Logger
{
    /// <summary>
    /// The logging delegate, containing the error code and associated message.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message. (eg. exception message, stack trace)</param>
    private delegate void LogDelegate(ErrorCode code, char* message);
    
    private static LogDelegate? _logger;

    /// <summary>
    /// Sets the logger.
    /// </summary>
    /// <param name="ptr">A <see cref="LogDelegate"/> callback to handle the message.</param>
    [UnmanagedCallersOnly(EntryPoint = "Logger_Set", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode Set(nint ptr)
    {
        _logger = Marshal.GetDelegateForFunctionPointer<LogDelegate>(ptr);
        return ErrorCode.Success;
    }

    /// <summary>
    /// Reports the specified error to the logger and returns itself.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description passed to the logger.</param>
    /// <returns>The reported error code.</returns>
    public static ErrorCode Error(ErrorCode code, string description)
    {
        if (_logger != null)
        {
            nint msgPtr = Marshal.StringToHGlobalUni(description);
            _logger(code, (char*)msgPtr);
            Marshal.FreeHGlobal(msgPtr);
        }

        return code;
    }

    /// <summary>
    /// Reports the error associated with the specified exception to the logger and returns the corresponding error code.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <returns>The error code associate with the exception.</returns>
    public static ErrorCode Error(Exception exception) => Error(ErrorCodeHelper.FromException(exception), exception.ToString());
}