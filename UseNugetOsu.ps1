# Run this script to revert back to using NuGet packages for osu dependencies
# rather than a local copy of osu.

$DEPENDENCIES=@(
    "..\osu\osu.Game.Rulesets.Catch\osu.Game.Rulesets.Catch.csproj"
    "..\osu\osu.Game.Rulesets.Mania\osu.Game.Rulesets.Mania.csproj"
    "..\osu\osu.Game.Rulesets.Osu\osu.Game.Rulesets.Osu.csproj"
    "..\osu\osu.Game.Rulesets.Taiko\osu.Game.Rulesets.Taiko.csproj"
    "..\osu\osu.Game\osu.Game.csproj"
)

dotnet sln osu-native.sln remove $DEPENDENCIES

dotnet remove osu.Native\osu.Native.csproj reference $DEPENDENCIES
dotnet remove osu.Native.Tests\osu.Native.Tests.csproj reference $DEPENDENCIES

dotnet add osu.Native\osu.Native.csproj package ppy.osu.Game
dotnet add osu.Native\osu.Native.csproj package ppy.osu.Game.Rulesets.Osu
dotnet add osu.Native\osu.Native.csproj package ppy.osu.Game.Rulesets.Taiko
dotnet add osu.Native\osu.Native.csproj package ppy.osu.Game.Rulesets.Catch
dotnet add osu.Native\osu.Native.csproj package ppy.osu.Game.Rulesets.Mania

dotnet add osu.Native.Tests\osu.Native.Tests.csproj package ppy.osu.Game
dotnet add osu.Native.Tests\osu.Native.Tests.csproj package ppy.osu.Game.Rulesets.Osu
dotnet add osu.Native.Tests\osu.Native.Tests.csproj package ppy.osu.Game.Rulesets.Taiko
dotnet add osu.Native.Tests\osu.Native.Tests.csproj package ppy.osu.Game.Rulesets.Catch
dotnet add osu.Native.Tests\osu.Native.Tests.csproj package ppy.osu.Game.Rulesets.Mania
