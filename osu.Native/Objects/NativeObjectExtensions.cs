namespace osu.Native.Objects;

/// <summary>
/// Provides methods for <see cref="INativeObject{T}"/> that cannot be provided by the interface.
/// </summary>
internal static class NativeObjectExtensions
{
  /// <summary>
  /// Resolves the native object to its managed object. Equivalent to calling <see cref="ObjectContainer{T}.Get(int)"/>.
  /// </summary>
  /// <param name="obj">The native object.</param>
  /// <returns>The associated managed object.</returns>
  public static T Resolve<T>(this INativeObject<T> obj) where T : notnull
  {
    return ObjectContainer<T>.Get(obj.ObjectId);
  }

  /// <summary>
  /// Destroys the native object, releasing the managed object reference. Equivalent to calling <see cref="ObjectContainer{T}.Remove(int)"/>,
  /// but additionally calls <see cref="IDisposable.Dispose()"/> on the object if it implements <see cref="IDisposable"/>.
  /// </summary>
  /// <param name="obj">The native object.</param>
  public static void Destroy<T>(this INativeObject<T> obj) where T : notnull
  {
    if (ObjectContainer<T>.Get(obj.ObjectId) is IDisposable disposable)
      disposable.Dispose();

    ObjectContainer<T>.Remove(obj.ObjectId);
  }
}
