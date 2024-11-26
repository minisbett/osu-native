using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu.Native.Tests;

internal static class Shared
{
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
