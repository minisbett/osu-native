using osu.Game.Online.API;
using osu.Native.Compiler;
using System.Runtime.InteropServices.Marshalling;

namespace osu.Native.Objects;

/// <summary>
/// Represents a mod, consisting of the acronym and mod settings (<see cref="APIMod"/>).<br/>
/// The mod is represented via an <see cref="APIMod"/> object, and thus ruleset-agnostic and not validated for existence.
/// </summary>
internal unsafe partial class ModObject : IOsuNativeObject<APIMod>
{
  /// <summary>
  /// Creates an instance of an <see cref="APIMod"/> from the specified acronym.
  /// </summary>
  /// <param name="acronymPtr">The acronym of the mod.</param>
  /// <param name="nativeModPtr">A pointer to write the resulting native mod object to.</param>
  [OsuNativeFunction]
  public static ErrorCode Create(byte* acronymPtr, NativeMod* nativeModPtr)
  {
    string acronym = Utf8StringMarshaller.ConvertToManaged(acronymPtr) ?? "";

    *nativeModPtr = new NativeMod { Handle = ManagedObjectRegistry.Register(new APIMod() { Acronym = acronym }) };

    return ErrorCode.Success;
  }

  /// <summary>
  /// Sets the specified setting of the specified mod to the specified value.
  /// </summary>
  /// <param name="modHandle">The handle of the mod.</param>
  /// <param name="keyPtr">The name of the mod setting.</param>
  /// <param name="value">The value of the mod setting.</param>
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
