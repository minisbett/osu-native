using osu.Native.Objects;

namespace osu.Native.Tests;

[TestFixture]
internal class ModTests
{
    [Test]
    public void Mod_CreateFromAcronym_SuccessAndExpectedHandle()
    {
        ErrorCode errorCode = FFI.Mod_Create("DT", out NativeMod mod);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
            Assert.That(mod.Handle.Id, Is.EqualTo(1));
        }
    }

    [Test]
    public void Mod_SetSetting_Success()
    {
        FFI.Mod_Create("DT", out NativeMod mod);
        ErrorCode errorCode = FFI.Mod_SetSetting(mod.Handle.Id, "speed_multiplier", 1.34);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
    }
}
