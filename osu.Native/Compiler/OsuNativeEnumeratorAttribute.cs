namespace osu.Native.Compiler;

/// <summary>
/// Marks a method "Foo" as a function providing a native enumerator, and source-generates Foo_Next and Foo_Destroy functions for it.
/// </summary>
/// <typeparam name="T">The type of objects to enumerate through.</typeparam>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
internal class OsuNativeEnumeratorAttribute<T> : Attribute where T : unmanaged;