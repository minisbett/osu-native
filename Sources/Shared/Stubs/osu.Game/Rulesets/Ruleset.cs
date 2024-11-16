// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Legacy;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu.Difficulty;

// ReSharper disable once CheckNamespace

namespace osu.Game.Rulesets
{
    public abstract class Ruleset
    {
        public List<Mod> AllMods = default!;

        public RulesetInfo RulesetInfo { get; }

        protected Ruleset()
        {
            RulesetInfo = new RulesetInfo
            {
                Name = Description,
                ShortName = ShortName,
                OnlineID = (this as ILegacyRuleset)?.LegacyID ?? -1
            };
        }

        public abstract string Description { get; }

        public abstract string ShortName { get; }

        public abstract Mod? CreateMod<T>();

        public virtual IEnumerable<Mod> ConvertFromLegacyMods(LegacyMods mods) => Array.Empty<Mod>();

        public abstract DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap);

        public abstract PerformanceCalculator CreatePerformanceCalculator();

        public abstract IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap);

        public abstract IBeatmapProcessor? CreateBeatmapProcessor(IBeatmap beatmap);
    }
}
