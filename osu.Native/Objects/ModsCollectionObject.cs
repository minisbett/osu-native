using osu.Game.Online.API;
using osu.Native.Compiler;

namespace osu.Native.Objects;

/// <summary>
/// Represents a collection of mods (<see cref="List{T}"/> of <see cref="APIMod"/>).
/// </summary>
internal unsafe partial class ModsCollectionObject : IOsuNativeObject<List<APIMod>>
{
  /// <summary>
  /// Creates an instance of a <see cref="List{APIMod}"/>.
  /// </summary>
  /// <param name="nativeModsCollectionPtr">A pointer to write the resulting native mods collection object to.</param>
  [OsuNativeFunction]
  public static ErrorCode Create(NativeModsCollection* nativeModsCollectionPtr)
  {
    *nativeModsCollectionPtr = new NativeModsCollection { Handle = ManagedObjectRegistry.Register(new List<APIMod>()) };

    return ErrorCode.Success;
  }

  /// <summary>
  /// Adds the specified mod to the specified mods collection.
  /// </summary>
  /// <param name="modsHandle">The handle of the mods collection.</param>
  /// <param name="modHandle">The handle of the mod to add.</param>
  [OsuNativeFunction]
  public static ErrorCode Add(ManagedObjectHandle<List<APIMod>> modsHandle, ManagedObjectHandle<APIMod> modHandle)
  {
    List<APIMod> mods = modsHandle.Resolve();
    APIMod mod = modHandle.Resolve();

    mods.Add(mod);

    return ErrorCode.Success;
  }


  /// <summary>
  /// Removes the specified mod from the specified mods collection.
  /// </summary>
  /// <param name="modsHandle">The handle of the mods collection.</param>
  /// <param name="modHandle">The handle of the mod to remove.</param>
  [OsuNativeFunction]
  public static ErrorCode Remove(ManagedObjectHandle<List<APIMod>> modsHandle, ManagedObjectHandle<APIMod> modHandle)
  {
    List<APIMod> mods = modsHandle.Resolve();
    APIMod mod = modHandle.Resolve();

    mods.Remove(mod);

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode Debug(ManagedObjectHandle<List<APIMod>> modsHandle)
  {
    List<APIMod> mods = modsHandle.Resolve();

    Console.WriteLine($"ModsCollection({modsHandle.Id}):");
    foreach (APIMod mod in mods)
    {
      Console.WriteLine($"  Mod: {mod.Acronym}");
      foreach (var setting in mod.Settings)
        Console.WriteLine($"    {setting.Key}: {setting.Value}");
    }

    return ErrorCode.Success;
  }
}
