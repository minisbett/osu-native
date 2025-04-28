using System;

namespace osu.Native.Objects.Internal;

/// <summary>
/// Marks a field as a native struct field, allowing the source generator to generate the native field.
/// </summary>
[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
internal sealed class OsuNativeFieldAttribute() : Attribute;