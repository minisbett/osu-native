using System.Reflection;
using System.Runtime.CompilerServices;
using osu.Game.Beatmaps.Formats;

namespace osu.Native;

internal static class OsuNative
{
#pragma warning disable CA2255
    [ModuleInitializer]
    public static void Initialize()
    {
        // Setting an entry assembly is required for osu!frameworks error-handling to work properly.
        Assembly.SetEntryAssembly(typeof(OsuNative).Assembly);

        LegacyDifficultyCalculatorBeatmapDecoder.Register();
    }
}