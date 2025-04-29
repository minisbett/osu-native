using System.Runtime.InteropServices;
using System.Text;

namespace osu.Native;

/// <summary>
/// Provides utility methods for handling buffer-logic.
/// <list type="bullet">
/// <item>If a buffer and size are provided, the data is written to the buffer</item>
/// <item>If the buffer is null, the size is written in *bufferSize and ErrorCode.BufferSizeQuery is returned</item>
/// <item>If the buffer is not null and the size is too small, ErrorCode.BufferTooSmall is returned</item>
/// </list>
/// </summary>
internal static class NativeHelper
{
  /// <summary>
  /// Writes the specified string into the provided buffer in UTF-8 encoding or writes the required size if the buffer is null.
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

    if (bytes.Length + 1 > *bufferSize)
      return ErrorCode.BufferTooSmall;

    bytes.AsSpan().CopyTo(new(buffer, *bufferSize));
    buffer[str.Length] = 0x0;

    return ErrorCode.Success;
  }

  /// <summary>
  /// Writes the specified unmanaged objects into the provided buffer or writes the required size if the buffer is null.
  /// </summary>
  /// <param name="span">The objects to be written into the buffer.</param>
  /// <param name="buffer">The object buffer.</param>
  /// <param name="bufferSize">The size of the object buffer in element count.</param>
  /// <returns>The resulting error code.</returns>
  public static unsafe ErrorCode UnmanagedBuffer<T>(ReadOnlySpan<T> span, T* buffer, int* bufferSize) where T : unmanaged
  {
    int objSize = sizeof(T);
    if (buffer is null)
    {
      *bufferSize = objSize;
      return ErrorCode.BufferSizeQuery;
    }

    if (objSize > *bufferSize)
      return ErrorCode.BufferTooSmall;

    span.CopyTo(new(buffer, *bufferSize));

    return ErrorCode.Success;
  }

  /// <summary>
  /// Converts a UTF-8 byte pointer into a managed string via <see cref="Marshal.PtrToStringUTF8(nint)"/>, returning an empty string for null.
  /// </summary>
  /// <param name="ptr"></param>
  /// <returns></returns>
  public static unsafe string ToUtf8(byte* ptr) => Marshal.PtrToStringUTF8((nint)ptr) ?? string.Empty;
}
