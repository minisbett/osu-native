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
  /// <item>If the buffer is null and <paramref name="bufferSize"/> 0, the size is written in <paramref name="bufferSize"/> and <see cref="ErrorCode.BufferSizeQuery"/> is returned</item>
  /// <item>If a buffer and size are provided, the string is written to the buffer. If the buffer is too small, the string will be truncated</item>
  /// </list>
  /// </summary>
  /// <param name="str">The string to be written into the buffer.</param>
  /// <param name="buffer">The string buffer.</param>
  /// <param name="bufferSize">The size of the string buffer.</param>
  /// <returns>The resulting error code.</returns>
  public static unsafe ErrorCode StringBuffer(string str, byte* buffer, int* bufferSize)
  {
    if (buffer is null && *bufferSize == 0)
    {
      *bufferSize = Encoding.UTF8.GetByteCount(str) + 1;
      return ErrorCode.BufferSizeQuery;
    }

    if (buffer is not null)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(str);
      int bytesToWrite = Math.Min(bytes.Length, *bufferSize - 1);
      bytes.AsSpan(0, bytesToWrite).CopyTo(new(buffer, bytesToWrite));
      buffer[bytesToWrite] = 0x0;
    }

    return ErrorCode.Success;
  }

  /// <summary>
  /// Converts the UTF-8 byte pointer into a managed string via <see cref="Marshal.PtrToStringUTF8(nint)"/>, returning an empty string for null.
  /// </summary>
  /// <param name="ptr">The pointer to the UTF-8 string.</param>
  /// <returns>The UTF-8 encoded string at the specified pointer.</returns>
  public static unsafe string ReadUtf8(byte* ptr) => Marshal.PtrToStringUTF8((nint)ptr) ?? string.Empty;

  /// <summary>
  /// Writes the specified string to the specified byte pointer in UTF-8 encoding.
  /// </summary>
  /// <param name="ptr">The pointer to the UTF-8 string.</param>
  /// <param name="str">The UTF-8 encoded string to write.</param>
  public static unsafe void WriteUtf8(ref byte* ptr, string str)
  {
    str += '\0';

    int length = Encoding.UTF8.GetByteCount(str);
    ptr = (byte*)Marshal.AllocHGlobal(length);

    fixed (char* p = str)
      Encoding.UTF8.GetBytes(p, str.Length, ptr, length);
  }
}
