using System.Text;

namespace osu.Native;

/// <summary>
/// Provides utility methods for writing to buffers.
/// </summary>
internal static unsafe class BufferHelper
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
    public static ErrorCode String(string str, byte* buffer, int* bufferSize)
    {
        if (buffer is null)
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
    /// Writes the specified values of unmanaged type into the provided buffer. If the buffer is too small, the array will be truncated.
    /// </summary>
    /// <param name="values">The array of values to be written into the buffer.</param>
    /// <param name="buffer">The buffer.</param>
    /// <param name="bufferSize">The size of the buffer. This is measured in elements, not bytes.</param>
    /// <returns>The resulting error code.</returns>
    public static void Write<T>(T[] values, T* buffer, int* bufferSize) where T : unmanaged
    {
        int elementsToWrite = Math.Min(values.Length, *bufferSize);
        values.AsSpan(0, elementsToWrite).CopyTo(new(buffer, elementsToWrite));
    }
}
