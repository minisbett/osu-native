<div align="center">

[![NuGet version](https://badge.fury.io/nu/smoogipoo.osu.Native.Bindings.svg)](https://badge.fury.io/nu/smoogipoo.osu.Native.Bindings)

</div>

# osu-native

A native (AOT-compiled) library for accessing different components of the [osu!lazer](https://github.com/ppy/osu) codebase.

> [!CAUTION]
>
> This library is work in progress. Feature requests are welcome!

### Features
- Difficulty & PP calculation for all modes (including Lazer mods & mod settings)

## Build
```
dotnet publish --ucr osu.Native
```
Building it via `dotnet build` will __not__ produce a NativeAOT-compiled library. `--ucr` builds `osu.Native` on the current platform. Cross-compilation is not supported on NativeAOT.

## Tests
```
dotnet test
```
The pre-build events for `osu.Native.Tests` will build `osu.Native` and copy it to `osu.Native.Tests`' compilation output in order for tests to work.
