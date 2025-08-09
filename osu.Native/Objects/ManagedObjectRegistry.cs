using System.Collections.Concurrent;

namespace osu.Native.Objects;

/// <summary>
/// A type-specific registry for storing managed objects associated with a handle.
/// </summary>
/// <typeparam name="T">The managed type of the registry.</typeparam>
internal static class ManagedObjectRegistry<T> where T : notnull
{
  private static readonly ConcurrentDictionary<int, T> _objects = [];
  private static int _nextId = 0;

  /// <summary>
  /// Adds the specified object to the container and returns a handle for it.
  /// </summary>
  /// <param name="obj">The object.</param>
  /// <returns>The handle of the object.</returns>
  public static ManagedObjectHandle<T> Register(T obj)
  {
    int objectId = Interlocked.Increment(ref _nextId);
    _objects[objectId] = obj;
    return new ManagedObjectHandle<T>(objectId);
  }

  /// <summary>
  /// Returns the managed object associated with the specified handle.
  /// </summary>
  /// <param name="objectId">The object handle.</param>
  /// <returns>The associated managed object.</returns>
  public static T Get(ManagedObjectHandle<T> handle)
  {
    if (_objects.TryGetValue(handle.Id, out T? value))
      return value;

    throw new ObjectNotFoundException(typeof(T), handle.Id);
  }

  /// <summary>
  /// Removes the object associated with the specified handle from the registry.
  /// </summary>
  /// <param name="objectId">The object ID.</param>
  public static void Remove(ManagedObjectHandle<T> handle)
  {
    _objects.TryRemove(handle.Id, out _);
  }
}

/// <summary>
/// Represents a handle to a managed object, containing the associated object ID assigned by the <see cref="ManagedObjectRegistry{T}"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="objectId"></param>
internal struct ManagedObjectHandle<T>(int objectId) where T : notnull
{
  /// <summary>
  /// The ID of the managed object assigned by the <see cref="ManagedObjectRegistry{T}"/>.
  /// </summary>
  public int Id = objectId;

  /// <summary>
  /// Resolves the managed object handle into the actual managed object.
  /// </summary>
  /// <returns></returns>
  public readonly T Resolve() => ManagedObjectRegistry<T>.Get(this);
}

/// <summary>
/// Exception thrown when an object with the specified ID is not found in the container.
/// </summary>
internal class ObjectNotFoundException(Type type, int id) : Exception($"No '{type.Name}' with ID '{id}' found.");