// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Users;

// ReSharper disable once CheckNamespace

namespace osu.Game.Scoring
{
    public class ScoreInfo : IScoreInfo
    {
        public BeatmapInfo? BeatmapInfo { get; set; }

        public IRulesetInfo Ruleset { get; set; } = null!;

        public int MaxCombo { get; set; }

        public double Accuracy { get; set; }

        public Mod[] Mods { get; set; } = Array.Empty<Mod>();

        public Dictionary<HitResult, int> Statistics { get; set; } = null!;

        public string BeatmapHash = string.Empty;
        public long OnlineID => default;
        public IUser User => default!;
        public long TotalScore => default;
        public long LegacyOnlineID => default;
        public DateTimeOffset Date => default;
        public double? PP => default;
        public IBeatmapInfo? Beatmap => default;
        public ScoreRank Rank => default;
        public Dictionary<HitResult, int> MaximumStatistics = default!;
    }
}
