using osu.Native.Objects;

namespace osu.Native.Tests.Objects;

[TestFixture]
internal class ManagedObjectStoreTests
{
    /// <summary>
    /// Serves as a reference type that is exclusively used for tests in this class.
    /// </summary>
    private class DummyObject;

    /// <summary>
    /// Stores a dummy object to the managed object store and expects it to successfully resolve and be equal on reference.
    /// </summary>
    [Test]
    public void Store_Object_Resolves()
    {
        DummyObject obj1 = new();

        ManagedObjectHandle<DummyObject> handle = ManagedObjectStore.Store(obj1);

        DummyObject obj2 = null!;
        Assert.DoesNotThrow(() => obj2 = handle.Resolve());
        Assert.That(obj1, Is.EqualTo(obj2));
    }

    /// <summary>
    /// Stores two dummy objects to the managed object store and expects the handle ID to have incremented by 1.
    /// </summary>
    [Test]
    public void Store_TwoObjects_IncreasesId()
    {
        DummyObject obj1 = new();
        DummyObject obj2 = new();

        ManagedObjectHandle<DummyObject> handle1 = ManagedObjectStore.Store(obj1);
        ManagedObjectHandle<DummyObject> handle2 = ManagedObjectStore.Store(obj2);

        Assert.That(handle2.Id, Is.EqualTo(handle1.Id + 1));
    }

    /// <summary>
    /// Resolves a null-handle and expects an ObjectNotResolvedException to be thrown.
    /// </summary>
    [Test]
    public void Resolve_NonExistentHandle_Throws()
    {
        ManagedObjectHandle<DummyObject> handle = new(0); // 0 is impossible to be assigned via the ManagedObjectStore

        Assert.Throws<ObjectNotResolvedException>(() => handle.Resolve());
    }

    /// <summary>
    /// Stores a dummy object, removes it again, resolves it and expects an ObjectNotResolvedException to be thrown.
    /// </summary>
    [Test]
    public void Remove_Object_ThrowsOnResolve()
    {
        DummyObject obj = new();

        ManagedObjectHandle<DummyObject> handle = ManagedObjectStore.Store(obj);

        ManagedObjectStore<DummyObject>.Remove(handle);

        Assert.Throws<ObjectNotResolvedException>(() => handle.Resolve());
    }
}
