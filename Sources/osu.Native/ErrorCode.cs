// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Native;

public enum ErrorCode : byte
{
    Success = 0,
    FileNotFound = 1,
    ContextNotFound = 2,
    Failure = 255
}
