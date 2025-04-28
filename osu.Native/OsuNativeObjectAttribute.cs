using System;

namespace osu.Native;

/// <summary>
/// Marks a class as an osu! native object, allowing the source generator to generate the native struct.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
sealed class OsuNativeObjectAttribute() : Attribute;