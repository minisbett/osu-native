# osu-native

`osu-native` is a **C# NativeAOT library** that exposes selected features from [osu!(lazer)](https://github.com/ppy/osu) over a **C-declaration FFI**.
It allows other languages and runtimes to interface with parts of osu!(lazer) without depending on .NET directly.

It is primarily designed to allow the osu!stable client to access osu!(lazer)s' difficulty and performance calculation algorithms, but furthermore aims to provide the ability for certain parts of osu! to be integrated in other languages.

Refer to the [wiki](./wiki) for more information.

Here is a list of wrappers created for osu-native library in other programming languages:

|Language|Author|Package|
|-|-|-|
|Rust|[Chiffa](https://github.com/chiffario)|[![](https://img.shields.io/badge/source-github-181717?logo=github)](https://github.com/chiffario/osu-native-rs)[![](https://img.shields.io/crates/v/osu-native-rs?logo=rust)](https://crates.io/crates/osu-native-rs)
|Python|[7mochi](https://github.com/7mochi)|[![](https://img.shields.io/badge/source-github-181717?logo=github)](https://github.com/7mochi/osu-native-py) [![](https://img.shields.io/pypi/v/osu-native-py?logo=pypi)](https://pypi.org/project/osu-native-py/)
|Java|[7mochi](https://github.com/7mochi)|[![](https://img.shields.io/badge/source-github-181717?logo=github)](https://github.com/7mochi/osu-native-jar) [![](https://img.shields.io/maven-central/v/io.github.7mochi/osu-native-jar?logo=apachemaven)](https://central.sonatype.com/artifact/io.github.7mochi/osu-native-jar)
|Node|[tosuapp](https://github.com/tosuapp)|[![](https://img.shields.io/badge/source-github-181717?logo=github)](https://github.com/tosuapp/osu-native-napi) [![](https://img.shields.io/npm/v/@tosuapp/osu-native-wrapper?logo=npm)](https://www.npmjs.com/package/@tosuapp/osu-native-wrapper)

> [!NOTE]
> The listed wrappers are maintained by 3rd-parties, and support for these is not provided via this repository. Please refer to the repository of the wrapper for any help.
>
> The wrappers may not be entirely up-to-date or may choose to version their releases differently.

## Features

- Beatmap file parsing (for usage in other features)
- Difficulty and Performance calculation in all rulesets
- Full osu!(lazer) mods support

If you would like to see support for more osu!(lazer) features in `osu-native`, feel free to open an issue.

## Concept
`osu-native` aims to mirror the OOP infrastructure of osu!(lazer). Managed C# objects are created via a function and stored in osu-native, and handles are returned that will be used for the caller to refer to them.

In a similar fashion, exception handling is kept simple, offering a similar experience to interacting with osu!(lazer) directly. Exceptions thrown are directly exposed in error-messages. For more information see [Error Handling (wiki)](./wiki/Error-Handling).
