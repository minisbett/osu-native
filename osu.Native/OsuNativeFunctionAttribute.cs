using System;

namespace osu.Native;

/// <summary>
/// Marks a method as an osu! native endpoint function, allowing the source generator to generate the native method.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
sealed class OsuNativeFunctionAttribute() : Attribute;