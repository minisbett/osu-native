using System.Runtime.InteropServices;
using System.Text;
using osu.Game.Online.API;
using osu.Native.Structures;
using osu.Native.Structures.Difficulty;
using osu.Native.Structures.Performance;

namespace osu.Native.Tests;

internal sealed unsafe class NativeTestUtils : IDisposable
{
    private readonly int _ruleset;
    private readonly Stack<Action> _cleanup = new();

    public NativeTestUtils(int ruleset)
    {
        if (ruleset is < 0 or > 3)
            throw new ArgumentOutOfRangeException(nameof(ruleset));

        _ruleset = ruleset;
    }

    public static string? GetLastError()
        => Marshal.PtrToStringUTF8(NativeApi.ErrorHandler_GetLastMessage());

    public static void AssertSuccess(ErrorCode error)
        => Assert.That(error, Is.EqualTo(ErrorCode.Success), GetLastError());

    private void RegisterCleanup(uint handle, Func<uint, ErrorCode> destroy)
        => _cleanup.Push(() => AssertSuccess(destroy(handle)));

    public NativeRuleset CreateRuleset(int id)
    {
        AssertSuccess(NativeApi.Ruleset_CreateFromId(id, out NativeRuleset result));
        RegisterCleanup(result.Handle.Id, NativeApi.Ruleset_Destroy);
        return result;
    }

    public NativeBeatmap CreateBeatmap(string filename)
    {
        string text = Encoding.UTF8.GetString(TestUtils.GetResource(filename));
        AssertSuccess(NativeApi.Beatmap_CreateFromText(text, out NativeBeatmap beatmap));
        RegisterCleanup(beatmap.Handle.Id, NativeApi.Beatmap_Destroy);
        return beatmap;
    }

    public NativeModsCollection CreateMods(List<APIMod> mods)
    {
        AssertSuccess(NativeApi.ModsCollection_Create(out NativeModsCollection collection));
        RegisterCleanup(collection.Handle.Id, NativeApi.ModsCollection_Destroy);

        foreach (APIMod mod in mods)
        {
            NativeMod nativeMod = CreateMod(mod);
            AssertSuccess(NativeApi.ModsCollection_Add(collection.Handle.Id, nativeMod.Handle.Id));
        }

        return collection;
    }

    private NativeMod CreateMod(APIMod mod)
    {
        AssertSuccess(NativeApi.Mod_Create(mod.Acronym, out NativeMod result));
        RegisterCleanup(result.Handle.Id, NativeApi.Mod_Destroy);

        foreach ((string key, object value) in mod.Settings)
        {
            ErrorCode error = value switch
            {
                bool boolean => NativeApi.Mod_SetSettingBool(result.Handle.Id, key, boolean),
                int or long => NativeApi.Mod_SetSettingInteger(result.Handle.Id, key, Convert.ToInt32(value)),
                float or double => NativeApi.Mod_SetSettingFloat(result.Handle.Id, key, Convert.ToSingle(value)),
                _ => throw new ArgumentException($"Unsupported mod setting: {key} ({value.GetType()}).")
            };
            AssertSuccess(error);
        }

        return result;
    }

    public ErrorCode CreatePerformance(uint* calculator)
    {
        ErrorCode error = _ruleset switch
        {
            0 => NativeApi.OsuPerformanceCalculator_Create((NativeOsuPerformanceCalculator*)calculator),
            1 => NativeApi.TaikoPerformanceCalculator_Create((NativeTaikoPerformanceCalculator*)calculator),
            2 => NativeApi.CatchPerformanceCalculator_Create((NativeCatchPerformanceCalculator*)calculator),
            3 => NativeApi.ManiaPerformanceCalculator_Create((NativeManiaPerformanceCalculator*)calculator),
            _ => throw new ArgumentOutOfRangeException(nameof(_ruleset))
        };
        if (error == ErrorCode.Success)
        {
            Func<uint, ErrorCode> destroy = _ruleset switch
            {
                0 => NativeApi.OsuPerformanceCalculator_Destroy,
                1 => NativeApi.TaikoPerformanceCalculator_Destroy,
                2 => NativeApi.CatchPerformanceCalculator_Destroy,
                3 => NativeApi.ManiaPerformanceCalculator_Destroy,
                _ => throw new ArgumentOutOfRangeException(nameof(_ruleset))
            };
            RegisterCleanup(*calculator, destroy);
        }

        return error;
    }

    public ErrorCode CreateDifficulty(uint rulesetHandle, uint beatmap, uint* calculator)
    {
        ErrorCode error = _ruleset switch
        {
            0 => NativeApi.OsuDifficultyCalculator_Create(rulesetHandle, beatmap, (NativeOsuDifficultyCalculator*)calculator),
            1 => NativeApi.TaikoDifficultyCalculator_Create(rulesetHandle, beatmap, (NativeTaikoDifficultyCalculator*)calculator),
            2 => NativeApi.CatchDifficultyCalculator_Create(rulesetHandle, beatmap, (NativeCatchDifficultyCalculator*)calculator),
            3 => NativeApi.ManiaDifficultyCalculator_Create(rulesetHandle, beatmap, (NativeManiaDifficultyCalculator*)calculator),
            _ => throw new ArgumentOutOfRangeException(nameof(_ruleset))
        };
        if (error == ErrorCode.Success)
        {
            Func<uint, ErrorCode> destroy = _ruleset switch
            {
                0 => NativeApi.OsuDifficultyCalculator_Destroy,
                1 => NativeApi.TaikoDifficultyCalculator_Destroy,
                2 => NativeApi.CatchDifficultyCalculator_Destroy,
                3 => NativeApi.ManiaDifficultyCalculator_Destroy,
                _ => throw new ArgumentOutOfRangeException(nameof(_ruleset))
            };
            RegisterCleanup(*calculator, destroy);
        }

        return error;
    }

    public ErrorCode CalculateDifficulty(uint calculator, uint mods, void* attributes)
        => _ruleset switch
        {
            0 => NativeApi.OsuDifficultyCalculator_Calculate(calculator, mods, (NativeOsuDifficultyAttributes*)attributes),
            1 => NativeApi.TaikoDifficultyCalculator_Calculate(calculator, mods, (NativeTaikoDifficultyAttributes*)attributes),
            2 => NativeApi.CatchDifficultyCalculator_Calculate(calculator, mods, (NativeCatchDifficultyAttributes*)attributes),
            3 => NativeApi.ManiaDifficultyCalculator_Calculate(calculator, mods, (NativeManiaDifficultyAttributes*)attributes),
            _ => throw new ArgumentOutOfRangeException(nameof(_ruleset))
        };

    public ErrorCode CalculateTimed(uint calculator, uint mods, void* attributes, int* size)
        => _ruleset switch
        {
            0 => NativeApi.OsuDifficultyCalculator_CalculateTimed(calculator, mods, (NativeTimedOsuDifficultyAttributes*)attributes, size),
            1 => NativeApi.TaikoDifficultyCalculator_CalculateTimed(calculator, mods, (NativeTimedTaikoDifficultyAttributes*)attributes, size),
            2 => NativeApi.CatchDifficultyCalculator_CalculateTimed(calculator, mods, (NativeTimedCatchDifficultyAttributes*)attributes, size),
            3 => NativeApi.ManiaDifficultyCalculator_CalculateTimed(calculator, mods, (NativeTimedManiaDifficultyAttributes*)attributes, size),
            _ => throw new ArgumentOutOfRangeException(nameof(_ruleset))
        };

    public ErrorCode CalculatePerformance(uint calculator, NativeScoreInfo score, void* difficulty, void* attributes)
        => _ruleset switch
        {
            0 => NativeApi.OsuPerformanceCalculator_Calculate(calculator, score, *(NativeOsuDifficultyAttributes*)difficulty, (NativeOsuPerformanceAttributes*)attributes),
            1 => NativeApi.TaikoPerformanceCalculator_Calculate(calculator, score, *(NativeTaikoDifficultyAttributes*)difficulty, (NativeTaikoPerformanceAttributes*)attributes),
            2 => NativeApi.CatchPerformanceCalculator_Calculate(calculator, score, *(NativeCatchDifficultyAttributes*)difficulty, (NativeCatchPerformanceAttributes*)attributes),
            3 => NativeApi.ManiaPerformanceCalculator_Calculate(calculator, score, *(NativeManiaDifficultyAttributes*)difficulty, (NativeManiaPerformanceAttributes*)attributes),
            _ => throw new ArgumentOutOfRangeException(nameof(_ruleset))
        };

    public void Dispose()
    {
        Assert.Multiple(() =>
        {
            while (_cleanup.TryPop(out Action? cleanup))
                cleanup();
        });
    }
}