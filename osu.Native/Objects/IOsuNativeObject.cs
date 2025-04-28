namespace osu.Native.Objects;

/// <summary>
/// Types inheriting from this interface are considered native objects, and source-generation will generate a native struct for them.
/// </summary>
/// <typeparam name="T">The managed type represented by this native object.</typeparam>
internal interface IOsuNativeObject<T> where T : notnull;