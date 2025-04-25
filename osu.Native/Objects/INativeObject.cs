namespace osu.Native.Objects;

/// <summary>
/// Represents the base of a native object, providing the type-specific ID the object is associated with in the <see cref="ObjectContainer{T}"/>.
/// </summary>
/// <typeparam name="T">The managed type represented by this native object.</typeparam>
internal interface INativeObject<T> where T : notnull
{
  /// <summary>
  /// The ID of the object in the <see cref="ObjectContainer{T}"/>, used to resolve the object to its managed type.
  /// <br/><br/>
  /// Furthermore, this ID is part of the layout of native structs. In order to allow parity with passing IDs instead of native structs,
  /// this property should always be declared at the start of any struct inheriting from this interface.
  /// </summary>
  public int ObjectId { get; }
}