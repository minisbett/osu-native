using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Difficulty.Preprocessing;
using osu.Game.Rulesets.Difficulty.Skills;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;

namespace osu.Native.Objects.Difficulty;

/// <summary>
/// A temporary class assisting with circumventing crrent limitations of the osu!(lazer) codebase in regards to calculation difficulty attributes lazily.
/// </summary>
internal static class LazyDifficultyCalculationHelper
{
    private static readonly MethodInfo _preProcess;
    private static readonly MethodInfo _createSkills;
    private static readonly MethodInfo _getDifficultyHitObjects;
    private static readonly MethodInfo _createDifficultyAttributes;
    private static readonly FieldInfo _playableMods;
    private static readonly FieldInfo _clockRate;
    private static readonly PropertyInfo _beatmap;
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields)]
    private static readonly Type _progressiveBeatmap;
    private static readonly FieldInfo _progressiveBeatmapHitObjects;

    static LazyDifficultyCalculationHelper()
    {
        _preProcess = typeof(DifficultyCalculator).GetMethod("preProcess", BindingFlags.NonPublic | BindingFlags.Instance)!;
        _createSkills = typeof(DifficultyCalculator).GetMethod("CreateSkills", BindingFlags.NonPublic | BindingFlags.Instance)!;
        _getDifficultyHitObjects = typeof(DifficultyCalculator).GetMethod("getDifficultyHitObjects", BindingFlags.NonPublic | BindingFlags.Instance)!;
        _createDifficultyAttributes = typeof(DifficultyCalculator).GetMethod("CreateDifficultyAttributes", BindingFlags.NonPublic | BindingFlags.Instance)!;
        _playableMods = typeof(DifficultyCalculator).GetField("playableMods", BindingFlags.NonPublic | BindingFlags.Instance)!;
        _clockRate = typeof(DifficultyCalculator).GetField("clockRate", BindingFlags.NonPublic | BindingFlags.Instance)!;
        _beatmap = typeof(DifficultyCalculator).GetProperty("Beatmap", BindingFlags.NonPublic | BindingFlags.Instance)!;
        _progressiveBeatmap = typeof(DifficultyCalculator).GetNestedType("ProgressiveCalculationBeatmap", BindingFlags.NonPublic | BindingFlags.Instance)!;
        _progressiveBeatmapHitObjects = _progressiveBeatmap.GetField("HitObjects")!;
    }

    [DynamicDependency(DynamicallyAccessedMemberTypes.AllConstructors, "osu.Game.Rulesets.Difficulty.DifficultyCalculator+ProgressiveCalculationBeatmap", "osu.Game")]
    public static IEnumerable<TimedDifficultyAttributes> CalculateTimedLazy(DifficultyCalculator calc, IEnumerable<Mod> mods)
    {
        _preProcess.Invoke(calc, [mods, CancellationToken.None]);
        IBeatmap beatmap = (IBeatmap)_beatmap.GetValue(calc)!;

        if (!beatmap.HitObjects.Any())
            return [];

        Mod[] playableMods = (Mod[])_playableMods.GetValue(calc)!;
        double clockRate = (double)_clockRate.GetValue(calc)!;
        Skill[] skills = (Skill[])_createSkills.Invoke(calc, [beatmap, mods, clockRate])!;
        object progressiveBeatmap = Activator.CreateInstance(_progressiveBeatmap, [beatmap])!;
        DifficultyHitObject[] difficultyObjects = [.. (IEnumerable<DifficultyHitObject>)_getDifficultyHitObjects.Invoke(calc, [])!];

        return enumerate();

        IEnumerable<TimedDifficultyAttributes> enumerate()
        {
            int currentIndex = 0;
            List<HitObject> progressiveHitObjects = (List<HitObject>)_progressiveBeatmapHitObjects.GetValue(progressiveBeatmap)!;

            foreach (HitObject obj in beatmap.HitObjects)
            {
                progressiveHitObjects.Add(obj);

                while (currentIndex < difficultyObjects.Length && difficultyObjects[currentIndex].BaseObject.GetEndTime() <= obj.GetEndTime())
                {
                    foreach (Skill skill in skills)
                    {
                        skill.Process(difficultyObjects[currentIndex]);
                    }

                    currentIndex++;
                }

                yield return new TimedDifficultyAttributes(obj.GetEndTime(), (DifficultyAttributes)_createDifficultyAttributes.Invoke(calc, [progressiveBeatmap, playableMods, skills, clockRate])!);
            }
        }
    }
}
