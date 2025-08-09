using osu.Game.Online.API;
using osu.Native.Compiler;

namespace osu.Native.Objects;

internal unsafe partial class ModsCollectionObject : IOsuNativeObject<List<APIMod>>
{
  [OsuNativeFunction]
  public static ErrorCode Create(NativeModsCollection* nativeModsCollectionPtr)
  {
    ManagedObjectHandle<List<APIMod>> handle = ManagedObjectRegistry<List<APIMod>>.Register([]);

    *nativeModsCollectionPtr = new NativeModsCollection { Handle = handle };

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode Add(ManagedObjectHandle<List<APIMod>> modsHandle, ManagedObjectHandle<APIMod> modHandle)
  {
    List<APIMod> mods = modsHandle.Resolve();
    APIMod mod = modHandle.Resolve();

    mods.Add(mod);

    return ErrorCode.Success;
  }

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
