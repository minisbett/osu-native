# osu-native

`osu-native` is a **C# NativeAOT library** that exposes selected features from [osu!(lazer)](https://github.com/ppy/osu) over a **C-declaration FFI**.
It allows other languages and runtimes to interface with parts of osu!(lazer) without depending on .NET directly.

It is primarily designed to allow the osu!stable client to access osu!(lazer)s' difficulty and performance calculation algorithms, but furthermore aims to provide the ability for certain parts of osu! to be integrated in other languages.

Refer to the [wiki](./wiki) for more information.

> [!CAUTION]
>
> Work in progress! The API is currently considered unstable.

## Features

- Beatmap file parsing (for usage in other features)
- Difficulty and Performance calculation in all rulesets
- Full osu!(lazer) mods support

If you would like to see support for more osu!(lazer) features in `osu-native`, feel free to open an issue.

## Concept
`osu-native` aims to mirror the OOP infrastructure of osu!(lazer). Managed C# objects are created via a function and stored in osu-native, and handles are returned that will be used for the caller to refer to them.

In a similar fashion, exception handling is kept simple, offering a similar experience to interacting with osu!(lazer) directly. Exceptions thrown are directly exposed in error-messages. For more information see [Error Handling (wiki)](./wiki/Error-Handling).
