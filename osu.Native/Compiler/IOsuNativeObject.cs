namespace osu.Native.Compiler;

/// <summary>
/// Types inheriting from this interface are considered native objects, and source-generation will generate native functions
/// for all methods marked with <see cref="OsuNativeFunctionAttribute"/>. Additionally, a Destroy function is generated for <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The managed type represented by this native object.</typeparam>
internal interface IOsuNativeObject<T> where T : notnull;