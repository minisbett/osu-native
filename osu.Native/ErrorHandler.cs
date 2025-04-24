using osu.Native.Objects;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace osu.Native;

/// <summary>
/// Manages the allocation and native providing of the latest error message in a thread.
/// </summary>
internal static unsafe class ErrorHandler
{
  /// <summary>
  /// A thread-unique pointer to the last set error message.
  /// </summary>
  [ThreadStatic]
  private static char* _lastErrorPtr;

  /// <summary>
  /// Returns the message of the last error in the thread this function was called from.
  /// </summary>
  /// <returns>The pointer to the last error.</returns>
  [UnmanagedCallersOnly(EntryPoint = "_GetLastError", CallConvs = [typeof(CallConvCdecl)])]
  public static char* GetLastError() => _lastErrorPtr;

  /// <summary>
  /// Sets the last error message in the calling thread to the specified message.
  /// </summary>
  /// <param name="error">The error message.</param>
  public static void Handle(string message)
  {
    if (_lastErrorPtr is not null)
    Marshal.FreeHGlobal((nint)_lastErrorPtr);

    _lastErrorPtr = (char*)Marshal.StringToHGlobalUni(message);
  }

  /// <summary>
  /// Sets the last error message in the calling thread to the message of the exception and returns the corresponding <see cref="ErrorCode"/>.
  /// </summary>
  /// <param name="exception">The exception.</param>
  /// <returns>The error code associated with the exception type.</returns>
  public static ErrorCode Handle(Exception exception)
  {
    Handle(exception.Message);

    return exception switch
    {
      ObjectNotFoundException => ErrorCode.ObjectNotFound,
      _ => ErrorCode.Failure
    };
  }
}
