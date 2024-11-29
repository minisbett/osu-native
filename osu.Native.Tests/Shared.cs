namespace osu.Native.Tests;

internal static class Shared
{
    /// <summary>
    /// An example .osu file content. For tests to work properly, the following criteria must be met:
    /// - The star rating for osu!standard is > 0
    /// - The total PP for osu!standard is > 0
    /// </summary>
    public const string BEATMAP_TEXT = """
                                       osu file format v14

                                       [Difficulty]
                                       ApproachRate:5

                                       [HitObjects]
                                       0,0,0,12,0,0
                                       0,0,100,12,0,0
                                       0,0,200,12,0,0
                                       0,0,300,12,0,0
                                       0,0,400,12,0,0
                                       """;
}
