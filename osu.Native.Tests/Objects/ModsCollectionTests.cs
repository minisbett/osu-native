using System.Runtime.InteropServices.Marshalling;
using osu.Game.Online.API;
using osu.Native.Objects;
using osu.Native.Structures;

namespace osu.Native.Tests.Objects;

[TestFixture]
internal unsafe class ModsCollectionTests
{
    /// <summary>
    /// Creates a mods collection object, resolves the handle and expects the collection to be empty.
    /// </summary>
    [Test]
    public void Create_Collection_SuccessAndEmpty()
    {
        NativeModsCollection nativeModsCollection;
        ErrorCode errorCode = ModsCollectionObject.Create(&nativeModsCollection);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));
        Assert.That(nativeModsCollection.Handle.Resolve(), Is.Empty);
    }

    /// <summary>
    /// Creates a mod and a mods collection object containing it, resolves the collection and mod and checks if the mod is contained.
    /// </summary>
    [Test]
    public void Add_Mod_Success()
    {
        NativeModsCollection nativeModsCollection;
        ModsCollectionObject.Create(&nativeModsCollection);
        List<APIMod> mods = nativeModsCollection.Handle.Resolve();

        NativeMod nativeMod;
        ModObject.Create(Utf8StringMarshaller.ConvertToUnmanaged("DT"), &nativeMod);
        APIMod mod = nativeMod.Handle.Resolve();

        ModsCollectionObject.Add(nativeModsCollection.Handle, nativeMod.Handle);

        Assert.That(mods, Contains.Item(mod));
    }

    /// <summary>
    /// Creates a mod and a mods collection object containing it, removes it again, resolves the collection and checks if it does not contain the mod.
    /// </summary>
    [Test]
    public void Remove_Mod_Success()
    {
        NativeModsCollection nativeModsCollection;
        ModsCollectionObject.Create(&nativeModsCollection);
        List<APIMod> mods = nativeModsCollection.Handle.Resolve();

        NativeMod nativeMod;
        ModObject.Create(Utf8StringMarshaller.ConvertToUnmanaged("DT"), &nativeMod);
        APIMod mod = nativeMod.Handle.Resolve();

        ModsCollectionObject.Add(nativeModsCollection.Handle, nativeMod.Handle);

        Assert.That(mods, Does.Contain(mod));

        ModsCollectionObject.Remove(nativeModsCollection.Handle, nativeMod.Handle);
        
        Assert.That(mods, Does.Not.Contain(mod));
    }
}