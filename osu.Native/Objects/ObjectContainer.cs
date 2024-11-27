// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Threading;

namespace osu.Native.Objects;

/// <summary>
/// A static container for all stored objects of a type and their corresponding IDs.
/// </summary>
/// <typeparam name="T">The type of objects this container holds.</typeparam>
public static class ObjectContainer<T> where T : notnull
{
    /// <summary>
    /// Holds all managed objects with their associated IDs.
    /// </summary>
    private static Dictionary<int, T> Objects { get; } = [];

    /// <summary>
    /// The next available ID. This should only be increased via <see cref="Interlocked.Increment(ref int)"/>.
    /// </summary>
    private static int _nextId = 0;

    /// <summary>
    /// Associates the specified managed object with a ID and returns it.
    /// </summary>
    /// <param name="obj">The managed object.</param>
    /// <returns>The ID referencing the managed object.</returns>
    public static int Add(T obj)
    {
        int id = Interlocked.Increment(ref _nextId);
        Objects[id] = obj;
        return id;
    }

    /// <summary>
    /// Returns the managed object the ID refers to.
    /// </summary>
    /// <param name="id">The ID referring to the managed object.</param>
    /// <returns>The managed object.</returns>
    /// <exception cref="ObjectNotFoundException">Throws if the managed object was destroyed or was never allocated.</exception>
    public static T Get(int id)
    {
        if (!Objects.TryGetValue(id, out T? value) || value is null)
            throw new ObjectNotFoundException();

        return value;
    }

    /// <summary>
    /// Disposes the managed object the ID refers to and removes it from the dictionary, gausing it to be considered garbage.
    /// </summary>
    /// <param name="id">The ID referring to the managed object.</param>
    public static void Destroy(int id)
    {
        T obj = Get(id);
        if (obj is IDisposable disposable)
            disposable.Dispose();

        Objects.Remove(id);
    }
}

/// <summary>
/// Indicates that the object referenced by the native object was not found, either because the object was destroyed or an invalid ID passed.
/// </summary>
public class ObjectNotFoundException() : Exception("The specified ID has no associated object.");