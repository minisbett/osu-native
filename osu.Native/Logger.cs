// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace osu.Native;

public unsafe static class Logger
{
    private delegate void LogDelegate(char* message);
    private static LogDelegate? logger;

    /// <summary>
    /// Sets the logger.
    /// </summary>
    /// <param name="handler">A <see cref="LogDelegate"/> callback to handle the message.</param>
    [UnmanagedCallersOnly(EntryPoint = "Logger_Set", CallConvs = [typeof(CallConvCdecl)])]
    public static ErrorCode SetLogger(IntPtr handler)
    {
        logger = Marshal.GetDelegateForFunctionPointer<LogDelegate>(handler);
        return ErrorCode.Success;
    }

    /// <summary>
    /// Reports the specified error to the logger and returns itself.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description passed to the logger.</param>
    /// <returns></returns>
    public static ErrorCode Error(ErrorCode code, string description)
    {
        if (logger != null)
        {
            IntPtr msgPtr = Marshal.StringToHGlobalUni(description);
            logger((char*)msgPtr);
            Marshal.FreeHGlobal(msgPtr);
        }

        return code;
    }
}

public enum ErrorCode : byte
{
    Success = 0,
    FileNotFound = 1,
    ContextNotFound = 2,
    Failure = 255
}
