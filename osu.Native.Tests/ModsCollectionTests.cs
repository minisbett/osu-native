using System;
using System.Collections.Generic;
using System.Text;
using osu.Native.Objects;

namespace osu.Native.Tests;

[TestFixture]
internal class ModsCollectionTests
{
    [Test]
    public void ModsCollection_Create_SuccessAndExpectedHandle()
    {
        ErrorCode errorCode = FFI.ModsCollection_Create(out NativeModsCollection modsCollection);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
    }

    [Test]
    public void ModsCollection_Add_Success()
    {
        FFI.Mod_Create("HD", out NativeMod nativeMod);
        FFI.ModsCollection_Create(out NativeModsCollection modsCollection);
        ErrorCode errorCode = FFI.ModsCollection_Add(modsCollection.Handle.Id, nativeMod.Handle.Id);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
    }

    [Test]
    public void ModsCollection_AddAndRemove_Success()
    {
        FFI.Mod_Create("HD", out NativeMod nativeMod);
        FFI.ModsCollection_Create(out NativeModsCollection modsCollection);
        ErrorCode errorCode1 = FFI.ModsCollection_Add(modsCollection.Handle.Id, nativeMod.Handle.Id);
        ErrorCode errorCode2 = FFI.ModsCollection_Remove(modsCollection.Handle.Id, nativeMod.Handle.Id);

        Assert.That(errorCode1, Is.EqualTo(ErrorCode.Success));
        Assert.That(errorCode2, Is.EqualTo(ErrorCode.Success));
    }
}
