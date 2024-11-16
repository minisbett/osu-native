// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Extensions.EnumExtensions;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Legacy;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mania.Beatmaps;
using osu.Game.Rulesets.Mania.Difficulty;
using osu.Game.Rulesets.Mania.Mods;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu.Difficulty;

// ReSharper disable once CheckNamespace
namespace osu.Game.Rulesets.Mania
{
    public class ManiaRuleset : Ruleset, ILegacyRuleset
    {
        public const int MAX_STAGE_KEYS = 10;
        public const string SHORT_NAME = "mania";

        public override string Description => "osu!mania";

        public override string ShortName => SHORT_NAME;

        public ManiaRuleset()
        {
            RulesetInfo.CreateInstanceFunc = () => new ManiaRuleset();
        }

        public override Mod CreateMod<T>()
        {
            if (typeof(T) == typeof(ManiaModDoubleTime))
                return new ManiaModDoubleTime();
            if (typeof(T) == typeof(ManiaModEasy))
                return new ManiaModEasy();
            if (typeof(T) == typeof(ManiaModHalfTime))
                return new ManiaModHalfTime();
            if (typeof(T) == typeof(ManiaModHardRock))
                return new ManiaModHardRock();
            if (typeof(T) == typeof(ManiaModKey1))
                return new ManiaModKey1();
            if (typeof(T) == typeof(ManiaModKey2))
                return new ManiaModKey2();
            if (typeof(T) == typeof(ManiaModKey3))
                return new ManiaModKey3();
            if (typeof(T) == typeof(ManiaModKey4))
                return new ManiaModKey4();
            if (typeof(T) == typeof(ManiaModKey5))
                return new ManiaModKey5();
            if (typeof(T) == typeof(ManiaModKey6))
                return new ManiaModKey6();
            if (typeof(T) == typeof(ManiaModKey7))
                return new ManiaModKey7();
            if (typeof(T) == typeof(ManiaModKey8))
                return new ManiaModKey8();
            if (typeof(T) == typeof(ManiaModKey9))
                return new ManiaModKey9();
            if (typeof(T) == typeof(ManiaModKey10))
                return new ManiaModKey10();
            if (typeof(T) == typeof(ManiaModDualStages))
                return new ManiaModDualStages();

            throw new System.NotImplementedException();
        }

        public override IEnumerable<Mod> ConvertFromLegacyMods(LegacyMods mods)
        {
            if (mods.HasFlagFast(LegacyMods.Nightcore))
                yield return new ManiaModDoubleTime();
            else if (mods.HasFlagFast(LegacyMods.DoubleTime))
                yield return new ManiaModDoubleTime();

            if (mods.HasFlagFast(LegacyMods.Easy))
                yield return new ManiaModEasy();

            if (mods.HasFlagFast(LegacyMods.HalfTime))
                yield return new ManiaModHalfTime();

            if (mods.HasFlagFast(LegacyMods.HardRock))
                yield return new ManiaModHardRock();

            if (mods.HasFlagFast(LegacyMods.Key1))
                yield return new ManiaModKey1();

            if (mods.HasFlagFast(LegacyMods.Key2))
                yield return new ManiaModKey2();

            if (mods.HasFlagFast(LegacyMods.Key3))
                yield return new ManiaModKey3();

            if (mods.HasFlagFast(LegacyMods.Key4))
                yield return new ManiaModKey4();

            if (mods.HasFlagFast(LegacyMods.Key5))
                yield return new ManiaModKey5();

            if (mods.HasFlagFast(LegacyMods.Key6))
                yield return new ManiaModKey6();

            if (mods.HasFlagFast(LegacyMods.Key7))
                yield return new ManiaModKey7();

            if (mods.HasFlagFast(LegacyMods.Key8))
                yield return new ManiaModKey8();

            if (mods.HasFlagFast(LegacyMods.Key9))
                yield return new ManiaModKey9();

            if (mods.HasFlagFast(LegacyMods.KeyCoop))
                yield return new ManiaModDualStages();

            if (mods.HasFlagFast(LegacyMods.ScoreV2))
                yield return new ModScoreV2();
        }

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap)
            => new ManiaDifficultyCalculator(RulesetInfo, beatmap);

        public override PerformanceCalculator CreatePerformanceCalculator() => new ManiaPerformanceCalculator();

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap)
            => new ManiaBeatmapConverter(beatmap, this);

        public override IBeatmapProcessor? CreateBeatmapProcessor(IBeatmap beatmap) => null;

        public int LegacyID => 3;
    }
}
