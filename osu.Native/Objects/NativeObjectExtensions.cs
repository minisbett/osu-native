// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

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
    /// <exception cref="ObjectNotFoundException">Throws if the managed object was destroyed.</exception>
    public static T Resolve<T>(this INativeObject<T> nativeObject) where T : notnull
    {
        if (!Context<T>.Objects.TryGetValue(nativeObject.ContextId, out T? value) || value is null)
            throw new ObjectNotFoundException();

        return value;
    }

    /// <summary>
    /// Destroys the managed object the native object refers to.
    /// </summary>
    /// <exception cref="ObjectNotFoundException">Throws if the managed object was already destroyed.</exception>
    public static void Destroy<T>(this INativeObject<T> nativeObject) where T : notnull
    {
        T obj = nativeObject.Resolve();
        if(obj is IDisposable disposable)
            disposable.Dispose();

        Context<T>.Objects.Remove(nativeObject.ContextId);
    }
}

/// <summary>
/// Indicates that the object referenced by the native object was not found, either because the object was destroyed or an invalid ID passed.
/// </summary>
public class ObjectNotFoundException() : Exception("The specified context ID has no associated object.");