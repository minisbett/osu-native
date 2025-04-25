using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace osu.Native;

/// <summary>
/// Manages error handling for native calls, either mapping exceptions to error codes or declaring an error code as a failure
/// and storing additional information about the failure as a string, provided via a native function.
/// </summary>
internal static unsafe class ErrorHandler
{
  /// <summary>
  /// A thread-unique pointer to a string containing information about the last <see cref="ErrorCode.Failure"/> error.
  /// </summary>
  [ThreadStatic]
  private static char* _lastFailurePtr;

  /// <summary>
  /// Returns the message of the last failure in the thread this function was called from.
  /// </summary>
  /// <returns>The pointer to the last error.</returns>
  [UnmanagedCallersOnly(EntryPoint = "GetLastFailure", CallConvs = [typeof(CallConvCdecl)])]
  public static char* GetFailure() => _lastFailurePtr;

  /// <summary>
  /// Considers the error a <see cref="ErrorCode.Failure"/> and makes additional information available via a native function.
  /// </summary>
  /// <param name="message">The message containing information about the failure.</param>
  /// <returns><see cref="ErrorCode.Failure"/></returns>
  public static ErrorCode Failure(string message)
  {
    if (_lastFailurePtr is not null)
      Marshal.FreeHGlobal((nint)_lastFailurePtr);

    _lastFailurePtr = (char*)Marshal.StringToHGlobalUni(message);
    return ErrorCode.Failure;
  }

  /// <summary>
  /// Maps the exception to an error code or, if no mapping is available, considers the error a <see cref="ErrorCode.Failure"/>,
  /// making the exception message and stacktrace available as the failure information via a native function.
  /// </summary>
  /// <param name="exception"></param>
  /// <returns></returns>
  public static ErrorCode Handle(Exception exception)
  {
    return exception switch
    {
      ObjectNotFoundException => ErrorCode.ObjectNotFound,
      _ => Failure(exception.ToString()),
    };
  }
}

/// <summary>
/// Exception thrown when an object with the specified ID is not found in the container.
/// </summary>
internal class ObjectNotFoundException : Exception;