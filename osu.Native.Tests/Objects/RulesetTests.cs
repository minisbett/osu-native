using System.Runtime.InteropServices.Marshalling;
using System.Text;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Taiko;
using osu.Native.Objects;
using osu.Native.Structures;

namespace osu.Native.Tests.Objects;

[TestFixture]
internal unsafe class RulesetTests
{
    [TestCase(0, typeof(OsuRuleset))]
    [TestCase(1, typeof(TaikoRuleset))]
    [TestCase(2, typeof(CatchRuleset))]
    [TestCase(3, typeof(ManiaRuleset))]
    public void Create_LegacyRulesetsFromId_Success(int id, Type rulesetType)
    {
        NativeRuleset nativeRuleset;
        ErrorCode errorCode = RulesetObject.CreateFromId(id, &nativeRuleset);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));

        Ruleset ruleset = nativeRuleset.Handle.Resolve();

        Assert.That(ruleset, Is.TypeOf(rulesetType));
    }

    [TestCase("osu", typeof(OsuRuleset))]
    [TestCase("taiko", typeof(TaikoRuleset))]
    [TestCase("fruits", typeof(CatchRuleset))]
    [TestCase("mania", typeof(ManiaRuleset))]
    public void Create_LegacyRulesetsFromShortName_Success(string shortName, Type rulesetType)
    {
        NativeRuleset nativeRuleset;
        ErrorCode errorCode = RulesetObject.CreateFromShortName(Utf8StringMarshaller.ConvertToUnmanaged(shortName), &nativeRuleset);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.Success));

        Ruleset ruleset = nativeRuleset.Handle.Resolve();

        Assert.That(ruleset, Is.TypeOf(rulesetType));
    }

    [Test]
    public void Create_UnknownRulesetFromId_ReturnsRulesetUnavailable()
    {
        NativeRuleset nativeRuleset;
        ErrorCode errorCode = RulesetObject.CreateFromId(-1, &nativeRuleset);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.RulesetUnavailable));
    }

    [Test]
    public void Create_UnknownRulesetFromShortName_ReturnsRulesetUnavailable()
    {
        NativeRuleset nativeRuleset;
        ErrorCode errorCode = RulesetObject.CreateFromShortName(Utf8StringMarshaller.ConvertToUnmanaged("foo"), &nativeRuleset);

        Assert.That(errorCode, Is.EqualTo(ErrorCode.RulesetUnavailable));
    }

    [TestCase("osu")]
    [TestCase("taiko")]
    [TestCase("fruits")]
    [TestCase("mania")]
    public void GetShortName_LegacyRulesets_ReturnsCorrectShortName(string shortName)
    {
        NativeRuleset nativeRuleset;
        RulesetObject.CreateFromShortName(Utf8StringMarshaller.ConvertToUnmanaged(shortName), &nativeRuleset);

        int size = 0;
        RulesetObject.GetShortName(nativeRuleset.Handle, null, &size);

        byte[] buffer = new byte[size];
        ErrorCode errorCode;
        fixed (byte* bufferPtr = buffer)
            errorCode = RulesetObject.GetShortName(nativeRuleset.Handle, bufferPtr, &size);

        Assert.That(Encoding.UTF8.GetString(buffer).TrimEnd(char.MinValue), Is.EqualTo(shortName));
    }
}