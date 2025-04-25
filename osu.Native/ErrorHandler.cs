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
  /// Sets the last error message in the calling thread to the specified message and returns the specified error code.
  /// </summary>
  /// <param name="errorCode">The error code.</param>
  /// <param name="message">The error message.</param>
  public static ErrorCode Handle(ErrorCode errorCode, string message)
  {
    if (_lastErrorPtr is not null)
      Marshal.FreeHGlobal((nint)_lastErrorPtr);

    _lastErrorPtr = (char*)Marshal.StringToHGlobalUni(message);
    return errorCode;
  }

  /// <summary>
  /// Sets the last error message in the calling thread to the exception and returns the matching error code for the exception type.
  /// </summary>
  /// <param name="exception"></param>
  /// <returns></returns>
  public static ErrorCode Handle(Exception exception)
  {
    ErrorCode errorCode = exception switch
    {
      ObjectNotFoundException => ErrorCode.ObjectNotFound,
      _ => ErrorCode.Failure
    };

    return Handle(errorCode, exception.ToString());
  }
}

/// <summary>
/// Exception thrown when an object with the specified ID is not found in the container.
/// </summary>
/// <param name="objectType">The type of the managed object.</param>
/// <param name="id">The native ID.</param>
internal class ObjectNotFoundException(Type objectType, int id) : Exception($"{objectType.Name} with ID {id} was not found.");