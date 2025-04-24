using System.Reflection;
using System.Runtime.CompilerServices;

namespace osu.Native;

internal static class OsuNative
{
#pragma warning disable CA2255
  [ModuleInitializer]
#pragma warning restore CA2255
  public static void Initialize()
  {
    // The entry assembly is null in AOT-compiled assemblies, but is needed for osu!framework's DebugUtils to work correctly.
    Assembly.SetEntryAssembly(typeof(OsuNative).Assembly);
  }
}
