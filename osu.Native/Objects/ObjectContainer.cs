using System.Collections.Concurrent;

namespace osu.Native.Objects;

/// <summary>
/// A type-specific container for storing managed objects associated with an object ID.
/// </summary>
/// <typeparam name="T">The managed type of the container.</typeparam>
internal static class ObjectContainer<T> where T : notnull
{
  private static readonly ConcurrentDictionary<int, T> _objects = [];
  private static int _nextId = 0;

  /// <summary>
  /// Adds the specified object to the container and returns a object ID for it.
  /// </summary>
  /// <param name="obj">The object.</param>
  /// <returns>The object ID of the object.</returns>
  public static int Add(T obj)
  {
    int objectId = Interlocked.Increment(ref _nextId);
    _objects[objectId] = obj;
    return objectId;
  }

  /// <summary>
  /// Returns the managed object associated with the specified ID.
  /// </summary>
  /// <param name="objectId">The object ID.</param>
  /// <returns>The associated managed object.</returns>
  public static T Get(int objectId)
  {
    if (_objects.TryGetValue(objectId, out T? value))
      return value;

    throw new ObjectNotFoundException(typeof(T), objectId);
  }

  /// <summary>
  /// Removes the object associated with the specified ID from the container.
  /// </summary>
  /// <param name="objectId">The object ID.</param>
  public static void Remove(int objectId)
  {
    _objects.TryRemove(objectId, out _);
  }
}

/// <summary>
/// Exception thrown when an object with the specified ID is not found in the container.
/// </summary>
internal class ObjectNotFoundException(Type type, int id) : Exception($"No '{type.Name}' with ID '{id}' found.");