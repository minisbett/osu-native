using osu.Native.Objects;

namespace osu.Native.Tests.Objects;

[TestFixture]
internal class ManagedObjectStoreTests
{
    private class DummyObject;

    [Test]
    public void Store_Object_Resolves()
    {
        DummyObject obj1 = new();

        ManagedObjectHandle<DummyObject> handle = ManagedObjectStore.Store(obj1);

        DummyObject obj2 = null!;
        Assert.DoesNotThrow(() => obj2 = handle.Resolve());
        Assert.That(obj1, Is.EqualTo(obj2));
    }

    [Test]
    public void Store_TwoObjects_IncreasesId()
    {
        DummyObject obj1 = new();
        DummyObject obj2 = new();

        ManagedObjectHandle<DummyObject> handle1 = ManagedObjectStore.Store(obj1);
        ManagedObjectHandle<DummyObject> handle2 = ManagedObjectStore.Store(obj2);

        Assert.That(handle2.Id, Is.EqualTo(handle1.Id + 1));
    }

    [Test]
    public void Resolve_NonExistentHandle_Throws()
    {
        ManagedObjectHandle<DummyObject> handle = new(0); // 0 is impossible to be assigned via the ManagedObjectStore

        Assert.Throws<ObjectNotResolvedException>(() => handle.Resolve());
    }

    [Test]
    public void Remove_Object_ThrowsOnResolve()
    {
        DummyObject obj = new();

        ManagedObjectHandle<DummyObject> handle = ManagedObjectStore.Store(obj);

        Assert.DoesNotThrow(() => handle.Resolve());

        ManagedObjectStore<DummyObject>.Remove(handle);

        Assert.Throws<ObjectNotResolvedException>(() => handle.Resolve());
    }
}
