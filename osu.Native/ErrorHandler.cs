using osu.Native.Objects;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using static osu.Game.Graphics.UserInterface.StarCounter;

namespace osu.Native;

/// <summary>
/// Manages error handling for native calls, storing the last thread-specific error messages and handling managed exceptions.
/// </summary>
internal static unsafe class ErrorHandler
{
  /// <summary>
  /// A thread-unique pointer to a string containing the last error message.
  /// </summary>
  private static byte* _lastMessagePtr;

  /// <summary>
  /// Returns the last error message in the calling thread.
  /// </summary>
  /// <returns>A pointer to the message.</returns>
  [UnmanagedCallersOnly(EntryPoint = "ErrorHandler_GetLastMessage", CallConvs = [typeof(CallConvCdecl)])]
  public static byte* GetLastMessage() => _lastMessagePtr;

  /// <summary>
  /// Sets the last error message in the calling thread to the specified message.
  /// If the message is null, the pointer will only be freed, and no new message will be set.
  /// </summary>
  /// <param name="message">The message containing information about the last error.</param>
  public static void SetLastMessage(string? message)
  {
    if (_lastMessagePtr is not null)
    {
      Utf8StringMarshaller.Free(_lastMessagePtr);
      _lastMessagePtr = null;
    }

    if (message is null)
      return;

    _lastMessagePtr = Utf8StringMarshaller.ConvertToUnmanaged(message);
  }

  /// <summary>
  /// Sets the last error message in the calling thread to the specified message and returns the specified error code.
  /// This method acts as a convenience for returning error codes from native functions that need to set an error message.
  /// </summary>
  /// <param name="code">The error code.</param>
  /// <param name="message">The error message to set for the calling thread.</param>
  /// <returns>The specified error code.</returns>
  public static ErrorCode Return(ErrorCode code, string message)
  {
    SetLastMessage(message);
    return code;
  }

  /// <summary>
  /// Called from the catch-blocks of source-generated native functions to handle any managed exceptions thrown.
  /// This function may cover all handling related to those exceptions. The error code will immediately be returned to the caller.
  /// </summary>
  /// <param name="ex">The thrown exception.</param>
  public static ErrorCode HandleException(Exception ex)
  {
    SetLastMessage(ex.ToString());

    return ex switch
    {
      ObjectNotFoundException => ErrorCode.ObjectNotFound,
      _ => ErrorCode.Failure
    };
  }
}