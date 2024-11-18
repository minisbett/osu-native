// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Threading;

namespace osu.Native.Helpers;

/// <summary>
/// Stores objects of type <typeparamref name="T"/> ans associates them with a context ID, allowing retrieval via the ID.
/// </summary>
/// <typeparam name="T">The object type.</typeparam>
public class Context<T>
{
    /// <summary>
    /// The next free context ID.
    /// </summary>
    private int _nextContextId = 0;

    /// <summary>
    /// Manages all created instances of <see cref="T"/> with their assigned ID.<br/>
    /// </summary>
    public static Dictionary<int, T> Objects { get; } = [];

    /// <summary>
    /// Stores the specified object and returns the associated ID for accessing it.
    /// </summary>
    /// <param name="obj">The object to store.</param>
    /// <returns>The context ID associated to the object.</returns>
    public int Create(T obj)
    {
        Interlocked.Increment(ref _nextContextId);
        Objects.Add(_nextContextId, obj);
        return _nextContextId;
    }

    /// <summary>
    /// Resolves the object associated with the specified context ID.
    /// </summary>
    /// <param name="contextId">The context ID associated with the object.</param>
    /// <returns>The object associated with the context ID.</returns>
    public T Resolve(int contextId)
    {
        if (!Objects.TryGetValue(contextId, out T? value) || value is null)
            throw new ContextNotFoundException();

        return value;
    }

    /// <summary>
    /// Destroys the object associated with the specifiec context ID.
    /// </summary>
    /// <param name="contextId">The context ID of the object to destroy.</param>
    public void Destroy(int contextId)
    {
        if (!Objects.Remove(_nextContextId))
            throw new ContextNotFoundException();
    }
}

/// <summary>
/// Indicates that the specified context ID is not associated with an object, either because the object was destroyed or the ID was never assigned.
/// </summary>
public class ContextNotFoundException : Exception
{
    public ContextNotFoundException() : base("The specified context ID has no associated object.") { }
}