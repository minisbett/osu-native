// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Taiko;
using osu.Native.EntryPoints;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace osu.Native;

public static class OsuNative
{
#pragma warning disable CA2255
    /// <summary>
    /// The "DllMain" of osu-native, initializing important runtime components.
    /// </summary>
    [ModuleInitializer]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(OsuRuleset))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(TaikoRuleset))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(CatchRuleset))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(ManiaRuleset))]
    public static void Initialize()
    {
        // EntryAssembly is null for NativeAOT, but is needed for DebugUtils to work correctly.
        Assembly.SetEntryAssembly(typeof(Logger).Assembly);
    }
}
