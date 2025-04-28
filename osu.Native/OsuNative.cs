using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Taiko;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace osu.Native;

internal static class OsuNative
{
#pragma warning disable CA2255
  [ModuleInitializer]
  [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(OsuRuleset))]
  [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(TaikoRuleset))]
  [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(CatchRuleset))]
  [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(ManiaRuleset))]
  public static void Initialize()
  {
    // The entry assembly is null in AOT-compiled assemblies, but is needed for osu!framework's DebugUtils to work correctly.
    Assembly.SetEntryAssembly(typeof(OsuNative).Assembly);
  }
}
