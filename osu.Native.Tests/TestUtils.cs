using System.IO;
using System.Reflection;

namespace osu.Native.Tests;

/// <summary>
/// Contains utility methods for the tests.
/// </summary>
internal static class TestUtils
{
    private const string RESOURCE_NAMESPACE = "osu.Native.Tests.Resources";

    /// <summary>
    /// Reads the specified resource file in the Resources folder and returns the content as a string.
    /// </summary>
    /// <param name="filename">The resource filename.</param>
    /// <returns>The file contents.</returns>
    public static string GetResourceString(string filename)
    {
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{RESOURCE_NAMESPACE}.{filename}")!;
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}
