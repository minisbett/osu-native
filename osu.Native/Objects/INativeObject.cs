// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Native.Objects;

/// <summary>
/// The base interface for native objects containing the context ID to reference it.
/// This can be implemented in a struct and extended with more information about the object.
/// </summary>
/// <typeparam name="T">The type of object this native object refers to.</typeparam>
public interface INativeObject<T> where T : notnull
{
    /// <summary>
    /// The ID associated with the referenced managed object in <see cref="Context{T}.Objects"/>.
    /// </summary>
    int ContextId { get; }
}