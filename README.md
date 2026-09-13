# osu-native

**osu-native** is a C# NativeAOT library that exposes selected features from [osu!(lazer)](https://github.com/ppy/osu) over a C-declaration FFI.
It allows other languages and runtimes to interface with parts of osu!(lazer) without depending on .NET directly.

It is primarily designed to allow the osu!(stable) client to access osu!(lazer)'s difficulty and performance calculation algorithms, but furthermore aims to provide the ability for certain parts of osu! to be integrated in other languages.

Refer to the [wiki](https://github.com/minisbett/osu-native/wiki) for more information.

Here is a list of wrappers created for osu-native in other programming languages:

|Language|Author|Package|
|-|-|-|
|Rust|[Chiffa](https://github.com/chiffario)|[![](https://img.shields.io/badge/source-github-181717?logo=github)](https://github.com/chiffario/osu-native-rs)[![](https://img.shields.io/crates/v/osu-native-rs?logo=rust)](https://crates.io/crates/osu-native-rs)
|Python|[7mochi](https://github.com/7mochi)|[![](https://img.shields.io/badge/source-github-181717?logo=github)](https://github.com/7mochi/osu-native-py) [![](https://img.shields.io/pypi/v/osu-native-py?logo=pypi)](https://pypi.org/project/osu-native-py/)
|Java|[7mochi](https://github.com/7mochi)|[![](https://img.shields.io/badge/source-github-181717?logo=github)](https://github.com/7mochi/osu-native-jar) [![](https://img.shields.io/maven-central/v/io.github.7mochi/osu-native-jar?logo=apachemaven)](https://central.sonatype.com/artifact/io.github.7mochi/osu-native-jar)
|Node|[tosuapp](https://github.com/tosuapp)|[![](https://img.shields.io/badge/source-github-181717?logo=github)](https://github.com/tosuapp/osu-native-napi) [![](https://img.shields.io/npm/v/@tosuapp/osu-native-wrapper?logo=npm)](https://www.npmjs.com/package/@tosuapp/osu-native-wrapper)

> [!NOTE]
> The listed wrappers are maintained by 3rd-parties, and support for these is not provided via this repository. Please refer to the repository of the wrapper for any help.
>
> The wrappers may not be entirely up-to-date or choose to version their releases differently.

## Features

- Beatmap file parsing (providing limited information for demonstration, mostly to be used in other features)
- Full osu!(lazer) mods construction for usage in other features
- Calculation of difficulty and performance attributes in all rulesets

If you would like to see support for more osu!(lazer) features, feel free to open an issue.

## Concept
osu-native aims to mirror the object-oriented infrastructure of osu!(lazer). Managed .NET objects are created via a function, stored in osu-native, and handles (sometimes whole native objects) are returned that can be used by the caller to refer to the them.

As an example, osu!(lazer)'s `FlatWorkingBeatmap` is represented by a `NativeBeatmap` over the FFI, including a handle to refer to the managed object.

In a similar fashion, exception handling aims to offer a similar experience to interaction you would have with osu!(lazer) within the .NET ecosystem. Thrown exceptions' messages and stack trace are directly exposed in error messages. For more information see [Error Handling (wiki)](https://github.com/minisbett/osu-native/wiki/Error-Handling).

# Contribute

<ins>Human</ins> contributions are always welcome. If you would like to request a feature, feel free to do so via an [issue](https://github.com/minisbett/osu-native/issues). If you intend to PR something, discussing it before-hand (eg. via an issue) is appreciated, but not a necessity.

If your changes involve updating the targetted osu!(lazer) version, please check [this wiki entry](https://github.com/minisbett/osu-native/wiki/Updating-osu!(lazer)).

If you would like to learn more about the project before getting started on contributing, make sure to check out the [wiki](https://github.com/minisbett/osu-native/wiki) as well. It contains some information on the inner workings of osu-native, such as error-handling and source-generation.