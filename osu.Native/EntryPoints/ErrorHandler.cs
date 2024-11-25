// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace osu.Native.EntryPoints;

public static class ErrorHandler
{
    /// <summary>
    /// A thread-unique pointer to the last set error message.
    /// </summary>
    [ThreadStatic]
    private static nint _lastErrorPtr;

    /// <summary>
    /// Returns the message of the last error in the thread this function was called from.
    /// </summary>
    /// <returns></returns>
    [UnmanagedCallersOnly(EntryPoint = "_GetLastError", CallConvs = [typeof(CallConvCdecl)])]
    public static nint GetLastError() => _lastErrorPtr;

    /// <summary>
    /// Sets the last error in the calling thread.
    /// </summary>
    /// <param name="error">The error message.</param>
    public static void SetLastError(string error)
    {
        Marshal.FreeHGlobal(_lastErrorPtr);
        _lastErrorPtr = Marshal.StringToHGlobalUni(error);
    }
}