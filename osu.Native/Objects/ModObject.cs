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
    ManagedObjectHandle<APIMod> handle = ManagedObjectRegistry<APIMod>.Register(new APIMod() { Acronym = acronym });

    *nativeModPtr = new NativeMod()
    {
      Handle = handle
    };

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode SetSetting(ManagedObjectHandle<APIMod> modHandle, byte* keyPtr, double value)
  {
    APIMod mod = modHandle.Resolve();

    string key = Utf8StringMarshaller.ConvertToManaged(keyPtr) ?? "";
    mod.Settings[key] = value;

    return ErrorCode.Success;
  }

  [OsuNativeFunction]
  public static ErrorCode Debug(ManagedObjectHandle<APIMod> modHandle)
  {
    APIMod mod = modHandle.Resolve();

    Console.WriteLine($"Mod({modHandle.Id}): {mod.Acronym}");
    foreach (var setting in mod.Settings)
      Console.WriteLine($"  {setting.Key}: {setting.Value}");

    return ErrorCode.Success;
  }
}
