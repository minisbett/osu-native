using System.Runtime.InteropServices;
using System.Text;

namespace osu.Native;

/// <summary>
/// Provides utility methods for native handling.
/// </summary>
internal static class NativeHelper
{
  /// <summary>
  /// Writes the specified string into the provided buffer in UTF-8 encoding.
  /// <list type="bullet">
  /// <item>If the buffer is null, the size is written in <paramref name="bufferSize"/> and <see cref="ErrorCode.BufferSizeQuery"/> is returned</item>
  /// <item>If a buffer and size are provided, the string is written to the buffer. If the buffer is too small, the string will be truncated</item>
  /// </list>
  /// </summary>
  /// <param name="str">The string to be written into the buffer.</param>
  /// <param name="buffer">The string buffer.</param>
  /// <param name="bufferSize">The size of the string buffer.</param>
  /// <returns>The resulting error code.</returns>
  public static unsafe ErrorCode StringBuffer(string str, byte* buffer, int* bufferSize)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(str);

    if (buffer is null)
    {
      *bufferSize = bytes.Length + 1;
      return ErrorCode.BufferSizeQuery;
    }

    int bytesToWrite = Math.Min(bytes.Length, *bufferSize - 1);
    bytes.AsSpan(0, bytesToWrite).CopyTo(new(buffer, bytesToWrite));
    buffer[bytesToWrite] = 0x0;

    return ErrorCode.Success;
  }

  /// <summary>
  /// Converts a UTF-8 byte pointer into a managed string via <see cref="Marshal.PtrToStringUTF8(nint)"/>, returning an empty string for null.
  /// </summary>
  /// <param name="ptr"></param>
  /// <returns></returns>
  public static unsafe string ToUtf8(byte* ptr) => Marshal.PtrToStringUTF8((nint)ptr) ?? string.Empty;
}
