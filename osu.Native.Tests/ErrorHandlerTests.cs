using System.Runtime.InteropServices.Marshalling;
using osu.Native.Objects;

namespace osu.Native.Tests;

[TestFixture]
internal unsafe class ErrorHandlerTests
{
    private static string? GetLastMessage()
        => Utf8StringMarshaller.ConvertToManaged(((delegate* unmanaged[Cdecl]<byte*>)&ErrorHandler.GetLastMessage)());

    [Test]
    public void SetLastMessage_SameThread_ReturnsSameMessage()
    {
        ErrorHandler.SetLastMessage("Foo");

        Assert.That(GetLastMessage(), Is.EqualTo("Foo"));
    }

    [Test]
    public void SetLastMessage_Null_NullifiesPointer()
    {
        ErrorHandler.SetLastMessage("Dean");
        ErrorHandler.SetLastMessage(null);

        Assert.That(GetLastMessage(), Is.Null);
    }

    [Test]
    public void SetLastMessage_Twice_KeepsLatest()
    {
        ErrorHandler.SetLastMessage("Herbert");
        ErrorHandler.SetLastMessage("Bar");

        Assert.That(GetLastMessage(), Is.EqualTo("Bar"));
    }

    [Test]
    public async Task SetLastMessage_DifferentThreads_IsThreadLocal()
    {
        ErrorHandler.SetLastMessage("A");
        string? messageA = GetLastMessage();

        string? messageB = null;

        Thread otherThread = new(() =>
        {
            unsafe
            {
                ErrorHandler.SetLastMessage("B");
                messageB = GetLastMessage();
            }
        });


        otherThread.Start();
        otherThread.Join();

        Assert.That(messageA, Is.EqualTo("A"));
        Assert.That(messageB, Is.EqualTo("B"));
    }

    [Test]
    public void Return_SetsMessageAndReturnsCode()
    {
        ErrorCode errorCode1 = ErrorHandler.Return(ErrorCode.Failure, "Baz");

        Assert.That(errorCode1, Is.EqualTo(ErrorCode.Failure));
        Assert.That(GetLastMessage(), Is.EqualTo("Baz"));

        ErrorCode errorCode2 = ErrorHandler.Return(ErrorCode.RulesetUnavailable, "osu");

        Assert.That(errorCode2, Is.EqualTo(ErrorCode.RulesetUnavailable));
        Assert.That(GetLastMessage(), Is.EqualTo("osu"));
    }

    [Test]
    public void HandleException_ObjectNotResolved_ReturnsObjectNotResolved()
    {
        ErrorCode errorCode = ErrorHandler.HandleException(new ObjectNotResolvedException(typeof(void), 1));

        Assert.That(errorCode, Is.EqualTo(ErrorCode.ObjectNotResolved));
    }

    [Test]
    public void HandleException_Any_ReturnsFailure()
    {
        ErrorCode errorCode = ErrorHandler.HandleException(new Exception());

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Failure));
    }

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
