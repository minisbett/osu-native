// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Native.Objects;

/// <summary>
/// Provides helper methods for <see cref="INativeObject{T}"/>.
/// </summary>
public static class NativeObjectExtensions
{
    /// <summary>
    /// Returns the managed object the native object refers to.
    /// </summary>
    /// <returns>The managed object.</returns>
    public static T Resolve<T>(this INativeObject<T> nativeObject) where T : notnull
    {
        return ObjectContainer<T>.Get(nativeObject.Id);
    }

    /// <summary>
    /// Destroys the managed object the native object refers to.
    /// </summary>
    /// <exception cref="ObjectNotFoundException">Throws if the managed object was already destroyed.</exception>
    public static void Destroy<T>(this INativeObject<T> nativeObject) where T : notnull
    {
        ObjectContainer<T>.Destroy(nativeObject.Id);
    }
}