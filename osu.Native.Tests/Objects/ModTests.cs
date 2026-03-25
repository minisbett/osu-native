using System.Runtime.InteropServices.Marshalling;
using osu.Game.Online.API;
using osu.Native.Objects;
using osu.Native.Structures;

namespace osu.Native.Tests.Objects;

[TestFixture]
internal unsafe class ModTests
{
    [Test]
    public void Create_Mod_Success()
    {
        NativeMod nativeMod;
        ErrorCode errorCode = ModObject.Create(Utf8StringMarshaller.ConvertToUnmanaged("DT"), &nativeMod);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));

        APIMod mod = nativeMod.Handle.Resolve();

        Assert.That(mod.Acronym, Is.EqualTo("DT"));
    }

    [Test]
    public void SetSetting_Success()
    {
        NativeMod nativeMod;
        ModObject.Create(Utf8StringMarshaller.ConvertToUnmanaged("HD"), &nativeMod);

        ErrorCode errorCode1 = ModObject.SetSettingBool(nativeMod.Handle, Utf8StringMarshaller.ConvertToUnmanaged("foo"), true);
        ErrorCode errorCode2 = ModObject.SetSettingInteger(nativeMod.Handle, Utf8StringMarshaller.ConvertToUnmanaged("bar"), 42);
        ErrorCode errorCode3 = ModObject.SetSettingFloat(nativeMod.Handle, Utf8StringMarshaller.ConvertToUnmanaged("baz"), 1.5f);

        Assert.That(errorCode1, Is.EqualTo(ErrorCode.Success));
        Assert.That(errorCode2, Is.EqualTo(ErrorCode.Success));
        Assert.That(errorCode3, Is.EqualTo(ErrorCode.Success));

        APIMod mod = nativeMod.Handle.Resolve();

        Assert.That(mod.Settings, Contains.Key("foo"));
        Assert.That(mod.Settings, Contains.Key("bar"));
        Assert.That(mod.Settings, Contains.Key("baz"));
        Assert.That(mod.Settings["foo"], Is.True);
        Assert.That(mod.Settings["bar"], Is.EqualTo(42));
        Assert.That(mod.Settings["baz"], Is.EqualTo(1.5));
    }
}