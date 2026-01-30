# Run this script to use a local copy of osu rather than fetching it from nuget.
# It expects the osu directory to be at the same level as the osu-native directory

$DEPENDENCIES=@(
    "..\osu\osu.Game.Rulesets.Catch\osu.Game.Rulesets.Catch.csproj"
    "..\osu\osu.Game.Rulesets.Mania\osu.Game.Rulesets.Mania.csproj"
    "..\osu\osu.Game.Rulesets.Osu\osu.Game.Rulesets.Osu.csproj"
    "..\osu\osu.Game.Rulesets.Taiko\osu.Game.Rulesets.Taiko.csproj"
    "..\osu\osu.Game\osu.Game.csproj"
)

dotnet sln osu.Native.sln add $DEPENDENCIES

dotnet remove osu.Native\osu.Native.csproj package ppy.osu.Game
dotnet remove osu.Native\osu.Native.csproj package ppy.osu.Game.Rulesets.Osu
dotnet remove osu.Native\osu.Native.csproj package ppy.osu.Game.Rulesets.Taiko
dotnet remove osu.Native\osu.Native.csproj package ppy.osu.Game.Rulesets.Catch
dotnet remove osu.Native\osu.Native.csproj package ppy.osu.Game.Rulesets.Mania

dotnet add osu.Native\osu.Native.csproj reference $DEPENDENCIES