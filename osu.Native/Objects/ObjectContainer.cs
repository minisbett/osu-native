using System.Collections.Concurrent;

namespace osu.Native.Objects;

/// <summary>
/// A type-specific container for storing managed objects associated with a native ID.
/// </summary>
/// <typeparam name="T">The managed type of the container.</typeparam>
internal static class ObjectContainer<T> where T : notnull
{
  private static ConcurrentDictionary<int, T> _objects = [];
  private static int _nextId = 0;

  /// <summary>
  /// Adds the specified object to the container and returns a native ID for it.
  /// </summary>
  /// <param name="obj">The object.</param>
  /// <returns>The native ID of the object.</returns>
  public static int Add(T obj)
  {
    int id = Interlocked.Increment(ref _nextId);
    _objects[id] = obj;
    return id;
  }

  /// <summary>
  /// Returns the managed object associated with the specified ID.
  /// </summary>
  /// <param name="id">The native ID.</param>
  /// <returns>The associated managed object.</returns>
  public static T Get(int id)
  {
    if (_objects.TryGetValue(id, out T? value))
      return value;

    throw new ObjectNotFoundException(typeof(T), id);
  }

  /// <summary>
  /// Removes the object associated with the specified ID from the container.
  /// </summary>
  /// <param name="id">The native ID.</param>
  public static void Remove(int id)
  {
    _objects.TryRemove(id, out _);
  }
}

/// <summary>
/// Exception thrown when an object with the specified ID is not found in the container.
/// </summary>
/// <param name="objectType">The type of the managed object.</param>
/// <param name="id">The native ID.</param>
internal class ObjectNotFoundException(Type objectType, int id) : Exception($"{objectType.Name} with ID {id} was not found.");