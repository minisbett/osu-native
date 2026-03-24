using System.Reflection;

namespace osu.Native.Tests;

/// <summary>
/// Provides utility functionality for tests.
/// </summary>
internal static class TestUtils
{
    /// <summary>
    /// Returns the binary content of the source file with the specified name.
    /// </summary>
    /// <param name="name">The resource name.</param>
    /// <returns>The resource binary.</returns>
    public static byte[] GetResource(string name)
    {
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"osu.Native.Tests.Resources.{name}")!;
        using MemoryStream ms = new();
        stream.CopyTo(ms);
        return ms.ToArray();
    }
}
