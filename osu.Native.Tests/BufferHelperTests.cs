namespace osu.Native.Tests;

[TestFixture]
internal unsafe class BufferHelperTests
{
    [Test]
    public void String_BufferSizeQuery_CorrectErrorCodeAndSize()
    {
        int size = 0;
        ErrorCode errorCode = BufferHelper.String("foo", null, &size);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.BufferSizeQuery));
        Assert.That(size, Is.EqualTo(4)); // "foo" + zero-terminator
    }

    [Test]
    public void String_InsufficientSize_TruncatesString()
    {
        int size = 5;
        byte[] buffer = new byte[10];
        fixed (byte* bufferPtr = buffer)
            BufferHelper.String("Dean Herbert", bufferPtr, &size);

        Assert.That(buffer, Is.EquivalentTo("Dean\0\0\0\0\0\0"u8.ToArray()));
    }
    [Test]
    public void Write_InsufficientSize_TruncatesData()
    {
        int size = 3;
        int[] buffer = new int[5];
        fixed (int* bufferPtr = buffer)
            BufferHelper.Write([1, 2, 3, 4, 5], bufferPtr, &size);

        Assert.That(buffer, Is.EquivalentTo([1, 2, 3, 0, 0]));
    }
}
