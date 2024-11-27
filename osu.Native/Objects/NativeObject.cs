// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Native.Objects;

/// <summary>
/// A basic native object, only containing the object ID for referring to the associated managed object.
/// </summary>
/// <typeparam name="T">The type of object this native object refers to.</typeparam>
public readonly struct NativeObject<T> : INativeObject<T> where T : notnull
{
    /// <inheritdoc/>
    public int Id { get; }

    private NativeObject(int id)
    {
        Id = id;
    }

    /// <summary>
    /// Creates an instance of <see cref="NativeObject{T}"/> that refers to the specified managed object.
    /// </summary>
    /// <param name="obj">The managed object.</param>
    /// <returns>The native object.</returns>
    public static NativeObject<T> Create(T obj)
    {
        int id = ObjectContainer<T>.Add(obj);
        return new NativeObject<T>(id);
    }
}

