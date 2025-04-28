using System;

namespace osu.Native.Objects;

/// <summary>
/// Marks a method as a native endpoint function, allowing the source generator to generate the native method.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
internal sealed class OsuNativeFunctionAttribute() : Attribute;