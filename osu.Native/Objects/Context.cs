// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Threading;

namespace osu.Native.Objects;

/// <summary>
/// A static container for all stored objects of a type and their corresponding context IDs.
/// </summary>
/// <typeparam name="T">The type of objects this container holds.</typeparam>
public static class Context<T> where T : notnull
{
    /// <summary>
    /// Holds all managed objects with their associated content IDs.
    /// </summary>
    public static Dictionary<int, T> Objects { get; } = new();

    /// <summary>
    /// Provides the next available context ID via <see cref="Interlocked.Increment(ref int)"/> and increases it by 1.
    /// </summary>
    public static int NextContextId => Interlocked.Increment(ref _nextContextId);
    private static int _nextContextId = 0;
}

