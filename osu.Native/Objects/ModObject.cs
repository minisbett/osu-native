using osu.Game.Online.API;
using osu.Native.Compiler;
using System.Runtime.InteropServices.Marshalling;

namespace osu.Native.Objects;

internal unsafe partial class ModObject : IOsuNativeObject<APIMod>
{
  [OsuNativeFunction]
  public static ErrorCode Create(byte* acronymPtr, NativeMod* nativeModPtr)
  {
    string acronym = Utf8StringMarshaller.ConvertToManaged(acronymPtr) ?? "";

    int objectId = ObjectContainer<APIMod>.Add(new APIMod() { Acronym = acronym });
    *nativeModPtr = new NativeMod()
    {
      ObjectId = objectId
    };

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode SetSetting(NativeMod nativeMod, byte* keyPtr, double value)
  {
    APIMod mod = nativeMod.Resolve();
    string key = Utf8StringMarshaller.ConvertToManaged(keyPtr) ?? "";
    mod.Settings[key] = value;

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode Debug(NativeMod nativeMod)
  {
    APIMod mod = nativeMod.Resolve();

    Console.WriteLine($"Mod({nativeMod.ObjectId}): {mod.Acronym}");
    foreach (var setting in mod.Settings)
      Console.WriteLine($"  {setting.Key}: {setting.Value}");

    return ErrorCode.Success;
  }
}
