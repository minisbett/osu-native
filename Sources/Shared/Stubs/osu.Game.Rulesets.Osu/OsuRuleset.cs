// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Extensions.EnumExtensions;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Legacy;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Osu.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu.Beatmaps;
using osu.Game.Rulesets.Osu.Mods;

// ReSharper disable once CheckNamespace

namespace osu.Game.Rulesets.Osu
{
    public class OsuRuleset : Ruleset, ILegacyRuleset
    {
        public const string SHORT_NAME = "osu";

        public override string Description => "osu!";

        public override string ShortName => SHORT_NAME;

        public OsuRuleset()
        {
            RulesetInfo.CreateInstanceFunc = () => new OsuRuleset();
        }

        public override Mod CreateMod<T>()
        {
            if (typeof(T) == typeof(OsuModBlinds))
                return new OsuModBlinds();
            if (typeof(T) == typeof(OsuModDoubleTime))
                return new OsuModDoubleTime();
            if (typeof(T) == typeof(OsuModEasy))
                return new OsuModEasy();
            if (typeof(T) == typeof(OsuModFlashlight))
                return new OsuModFlashlight();
            if (typeof(T) == typeof(OsuModHalfTime))
                return new OsuModHalfTime();
            if (typeof(T) == typeof(OsuModHardRock))
                return new OsuModHardRock();
            if (typeof(T) == typeof(OsuModHidden))
                return new OsuModHidden();
            if (typeof(T) == typeof(OsuModNoFail))
                return new OsuModNoFail();
            if (typeof(T) == typeof(OsuModRelax))
                return new OsuModRelax();
            if (typeof(T) == typeof(OsuModSpunOut))
                return new OsuModSpunOut();
            if (typeof(T) == typeof(OsuModTouchDevice))
                return new OsuModTouchDevice();

            throw new System.NotImplementedException();
        }

        public override IEnumerable<Mod> ConvertFromLegacyMods(LegacyMods mods)
        {
            if (mods.HasFlagFast(LegacyMods.Nightcore))
                yield return new OsuModDoubleTime();
            else if (mods.HasFlagFast(LegacyMods.DoubleTime))
                yield return new OsuModDoubleTime();

            if (mods.HasFlagFast(LegacyMods.Easy))
                yield return new OsuModEasy();

            if (mods.HasFlagFast(LegacyMods.Flashlight))
                yield return new OsuModFlashlight();

            if (mods.HasFlagFast(LegacyMods.HalfTime))
                yield return new OsuModHalfTime();

            if (mods.HasFlagFast(LegacyMods.HardRock))
                yield return new OsuModHardRock();

            if (mods.HasFlagFast(LegacyMods.Hidden))
                yield return new OsuModHidden();

            if (mods.HasFlagFast(LegacyMods.NoFail))
                yield return new OsuModNoFail();

            if (mods.HasFlagFast(LegacyMods.Relax))
                yield return new OsuModRelax();

            if (mods.HasFlagFast(LegacyMods.SpunOut))
                yield return new OsuModSpunOut();

            if (mods.HasFlagFast(LegacyMods.TouchDevice))
                yield return new OsuModTouchDevice();

            if (mods.HasFlagFast(LegacyMods.ScoreV2))
                yield return new ModScoreV2();
        }

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap)
            => new OsuDifficultyCalculator(RulesetInfo, beatmap);

        public override PerformanceCalculator CreatePerformanceCalculator() => new OsuPerformanceCalculator();

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap)
            => new OsuBeatmapConverter(beatmap, this);

        public override IBeatmapProcessor CreateBeatmapProcessor(IBeatmap beatmap)
            => new OsuBeatmapProcessor(beatmap);

        public int LegacyID => 0;
    }
}
