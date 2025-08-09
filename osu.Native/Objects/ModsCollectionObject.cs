using osu.Game.Online.API;
using osu.Native.Compiler;

namespace osu.Native.Objects;

internal unsafe partial class ModsCollectionObject : IOsuNativeObject<List<APIMod>>
{
  [OsuNativeFunction]
  public static ErrorCode Create(NativeModsCollection* nativeModsCollectionPtr)
  {
    int objectId = ObjectContainer<List<APIMod>>.Add([]);
    *nativeModsCollectionPtr = new NativeModsCollection()
    {
      ObjectId = objectId
    };

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode Add(NativeModsCollection nativeModsCollection, NativeMod nativeMod)
  {
    List<APIMod> mods = nativeModsCollection.Resolve();
    APIMod mod = nativeMod.Resolve();
    mods.Add(mod);

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode Remove(NativeModsCollection nativeModsCollection, NativeMod nativeMod)
  {
    List<APIMod> mods = nativeModsCollection.Resolve();
    APIMod mod = nativeMod.Resolve();
    mods.Remove(mod);

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode Debug(NativeModsCollection nativeModsCollection)
  {
    List<APIMod> mods = nativeModsCollection.Resolve();

    Console.WriteLine($"ModsCollection({nativeModsCollection.ObjectId}):");
    foreach (APIMod mod in mods)
    {
      Console.WriteLine($"  Mod: {mod.Acronym}");
      foreach (var setting in mod.Settings)
        Console.WriteLine($"    {setting.Key}: {setting.Value}");
    }

    return ErrorCode.Success;
  }
}
