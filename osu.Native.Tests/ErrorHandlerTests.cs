using System.Runtime.InteropServices.Marshalling;
using osu.Native.Objects;

namespace osu.Native.Tests;

[TestFixture]
internal unsafe class ErrorHandlerTests
{
    private static string? GetLastMessage()
        => Utf8StringMarshaller.ConvertToManaged(((delegate* unmanaged[Cdecl]<byte*>)&ErrorHandler.GetLastMessage)());

    /// <summary>
    /// Sets the last error message and expects GetLastMessage() to return said message.
    /// </summary>
    [Test]
    public void SetLastMessage_SameThread_ReturnsSameMessage()
    {
        ErrorHandler.SetLastMessage("Foo");

        Assert.That(GetLastMessage(), Is.EqualTo("Foo"));
    }

    /// <summary>
    /// Sets the last error message to a string, followed by null and expects GetLastMessage() to return null / a null pointer.
    /// </summary>
    [Test]
    public void SetLastMessage_Null_NullifiesPointer()
    {
        ErrorHandler.SetLastMessage("Dean");
        ErrorHandler.SetLastMessage(null);

        Assert.That(GetLastMessage(), Is.Null);
    }

    /// <summary>
    /// Sets the last error message twice and expects GetLastMessage() to return the second one.
    /// </summary>
    [Test]
    public void SetLastMessage_Twice_KeepsLatest()
    {
        ErrorHandler.SetLastMessage("Herbert");
        ErrorHandler.SetLastMessage("Bar");

        Assert.That(GetLastMessage(), Is.EqualTo("Bar"));
    }

    /// <summary>
    /// Sets the last error message in the current and a newly created thread and expects the calls to GetLastMessage() return the correct messages.
    /// </summary>
    [Test]
    public void SetLastMessage_DifferentThreads_IsThreadLocal()
    {
        ErrorHandler.SetLastMessage("A");
        string? messageA = GetLastMessage();

        string? messageB = null;

        Thread otherThread = new(() =>
        {
            ErrorHandler.SetLastMessage("B");
            messageB = GetLastMessage();
        });


        otherThread.Start();
        otherThread.Join();

        Assert.That(messageA, Is.EqualTo("A"));
        Assert.That(messageB, Is.EqualTo("B"));
    }

    /// <summary>
    /// Handles an error via Return() and expects the correct last error message to be set and correct error code to be returned.
    /// </summary>
    [Test]
    public void Return_SetsMessageAndReturnsCode()
    {
        ErrorCode errorCode = ErrorHandler.Return(ErrorCode.RulesetUnavailable, "Baz");

        Assert.That(errorCode, Is.EqualTo(ErrorCode.RulesetUnavailable));
        Assert.That(GetLastMessage(), Is.EqualTo("Baz"));
    }

    /// <summary>
    /// Handles an ObjectNotResolvedException via HandleException() and expects ObjectNotResolved to be returned.
    /// </summary>
    [Test]
    public void HandleException_ObjectNotResolved_ReturnsObjectNotResolved()
    {
        ErrorCode errorCode = ErrorHandler.HandleException(new ObjectNotResolvedException(typeof(void), 1));

        Assert.That(errorCode, Is.EqualTo(ErrorCode.ObjectNotResolved));
    }

    /// <summary>
    /// Handles an unspecific exception via HandleException() and expects Failure to be returned.
    /// </summary>
    [Test]
    public void HandleException_Any_ReturnsFailure()
    {
        ErrorCode errorCode = ErrorHandler.HandleException(new());

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Failure));
    }

    /// <summary>
    /// Handles an exception via HandleException() and expects the last error message to be the exceptions' string representation.
    /// </summary>
    [Test]
    public void HandleException_Any_SetsLastErrorMessage()
    {
        try
        {
            throw new Exception();
        }
        catch (Exception ex)
        {
            ErrorHandler.HandleException(ex);

            Assert.That(GetLastMessage(), Is.EqualTo(ex.ToString()));
        }
    }
}
