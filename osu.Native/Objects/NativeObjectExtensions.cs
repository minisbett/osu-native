namespace osu.Native.Objects;

/// <summary>
/// Provides extension methods for resolving and destroying native objects.
/// </summary>
internal static class NativeObjectExtensions
{
  /// <summary>
  /// Resolves the native object to its managed type using the <see cref="ObjectContainer{T}"/>.
  /// </summary>
  /// <param name="obj">The native object.</param>
  /// <returns>The associated managed object.</returns>
  public static T Resolve<T>(this INativeObject<T> obj) where T : notnull
  {
    return ObjectContainer<T>.Get(obj.Id);
  }

  /// <summary>
  /// Destroys the native object by removing it from the <see cref="ObjectContainer{T}"/> and disposing of it if necessary.
  /// </summary>
  /// <param name="obj">The native object.</param>
  public static void Destroy<T>(this INativeObject<T> obj) where T : notnull
  {
    if (ObjectContainer<T>.Get(obj.Id) is IDisposable disposable)
      disposable.Dispose();

    ObjectContainer<T>.Remove(obj.Id);
  }
}
