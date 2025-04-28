using System.Runtime.InteropServices;

namespace osu.Native;

/// <summary>
/// Provides utility methods for handling buffer-logic.
/// <list type="bullet">
/// <item>If a buffer and size are provided, the data is written to the buffer</item>
/// <item>If the buffer is null, the size is written in *bufferSize and ErrorCode.BufferSizeQuery is returned</item>
/// <item>If the buffer is not null and the size is too small, ErrorCode.BufferTooSmall is returned</item>
/// </list>
/// </summary>
internal static class BufferHelper
{
  /// <summary>
  /// Writes the specified string into the provided buffer or writes the required size if the buffer is null.
  /// </summary>
  /// <param name="str">The string to be written into the buffer.</param>
  /// <param name="buffer">The string buffer.</param>
  /// <param name="bufferSize">The size of the string buffer.</param>
  /// <returns>The resulting error code.</returns>
  public static unsafe ErrorCode String(string str, char* buffer, int* bufferSize)
  {
    if (buffer is null)
    {
      *bufferSize = str.Length + 1;
      return ErrorCode.BufferSizeQuery;
    }

    if (str.Length + 1 > *bufferSize)
      return ErrorCode.BufferTooSmall;

    str.AsSpan().CopyTo(new(buffer, *bufferSize));
    buffer[str.Length] = '\0';

    return ErrorCode.Success;
  }

  /// <summary>
  /// Writes the specified unmanaged objects into the provided buffer or writes the required size if the buffer is null.
  /// </summary>
  /// <param name="span">The objects to be written into the buffer.</param>
  /// <param name="buffer">The object buffer.</param>
  /// <param name="bufferSize">The size of the object buffer in element count.</param>
  /// <returns>The resulting error code.</returns>
  public static unsafe ErrorCode Unmanaged<T>(ReadOnlySpan<T> span, T* buffer, int* bufferSize) where T : unmanaged
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
}
