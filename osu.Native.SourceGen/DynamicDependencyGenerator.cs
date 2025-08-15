using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Linq;
using DynamicDependency = (osu.Native.Analyzers.DynamicallyAccessedMemberTypes MemberTypes, string TypeName);

namespace osu.Native.Analyzers;

[Generator]
public class DynamicDependencyGenerator : IIncrementalGenerator
{
  public void Initialize(IncrementalGeneratorInitializationContext context)
  {
    context.RegisterSourceOutput(context.CompilationProvider, static (spc, src) =>
    {
      List<DynamicDependency> dependencies = [
        ..GetStaticDependencies(),
        ..GetModDependencies(src)
      ];

      string source =
      $$"""
      using System.Runtime.CompilerServices;
      using System.Diagnostics.CodeAnalysis;

      public class DynamicDependencies
      {
        [ModuleInitializer]
        {{string.Join("\n", dependencies.Select(x => $"[DynamicDependency(DynamicallyAccessedMemberTypes.{x.MemberTypes}, typeof({x.TypeName})]"))}}
        public static void Initialize()
        {
          Assembly.SetEntryAssembly(typeof(OsuNative).Assembly);
        }
      }
      """;

      source = CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace().ToFullString();
      spc.AddSource("DynamicDependencies.g.cs", source);
    });
  }

  private static IEnumerable<DynamicDependency> GetStaticDependencies()
  {
    yield return (DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, "osu.Game.Rulesets.Osu.OsuRuleset");
    yield return (DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, "osu.Game.Rulesets.Taiko.TaikoRuleset");
    yield return (DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, "osu.Game.Rulesets.Catch.CatchRuleset");
    yield return (DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, "osu.Game.Rulesets.Mania.ManiaRuleset");
  }

  private static IEnumerable<DynamicDependency> GetModDependencies(Compilation compilation)
  {
    string[] modTypes = [
      ..GetTypeNamesInNamespace(compilation, "osu.Game", "osu.Game.Rulesets.Mods"),
      ..GetTypeNamesInNamespace(compilation, "osu.Game.Rulesets.Osu", "osu.Game.Rulesets.Osu.Mods"),
      ..GetTypeNamesInNamespace(compilation, "osu.Game.Rulesets.Taiko", "osu.Game.Rulesets.Taiko.Mods"),
      ..GetTypeNamesInNamespace(compilation, "osu.Game.Rulesets.Catch", "osu.Game.Rulesets.Catch.Mods"),
      ..GetTypeNamesInNamespace(compilation, "osu.Game.Rulesets.Mania", "osu.Game.Rulesets.Mania.Mods")
    ];

    return modTypes.Select(x => (DynamicallyAccessedMemberTypes.PublicProperties, x));
  }

  private static IEnumerable<string> GetTypeNamesInNamespace(Compilation compilation, string assemblyName, string namespaceName)
  {
    IAssemblySymbol assembly = compilation.References.Select(compilation.GetAssemblyOrModuleSymbol)
     .OfType<IAssemblySymbol>()
     .First(x => x.Name == assemblyName);

    INamespaceSymbol ns = assembly.GlobalNamespace;
    foreach (string part in namespaceName.Split('.'))
    {
      ns = ns.GetNamespaceMembers().FirstOrDefault(n => n.Name == part);
      if (ns is null) // Namespace not found after walking down the path
        yield break;
    }

    foreach (INamedTypeSymbol type in ns.GetTypeMembers())
      yield return (type.IsGenericType ? type.ConstructUnboundGenericType() : type).ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
  }
}

internal enum DynamicallyAccessedMemberTypes
{
  PublicProperties,
  PublicParameterlessConstructor
}