using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using DynamicDependency = (osu.Native.Analyzers.DynamicallyAccessedMemberTypes MemberTypes, string TypeName, string AssemblyName);

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

            string attributes = string.Join("\n", dependencies.Select(x =>
                $"[DynamicDependency(DynamicallyAccessedMemberTypes.{x.MemberTypes}, \"{x.TypeName}\", \"{x.AssemblyName}\")]"));

            string source =
                $$"""
                using System.Reflection;
                using System.Runtime.CompilerServices;
                using System.Diagnostics.CodeAnalysis;

                namespace osu.Native.Compiler;

                internal static class DynamicDependencies
                {
                    [ModuleInitializer]
                    {{attributes}}
                    public static void Initialize()
                    {
                        Assembly.SetEntryAssembly(typeof(OsuNativeMarker).Assembly);
                    }
                }
                """;

            source = CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace().ToFullString();
            spc.AddSource("DynamicDependencies.g.cs", source);
        });
    }

    private static IEnumerable<DynamicDependency> GetStaticDependencies()
    {
        yield return (DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, "osu.Game.Rulesets.Osu.OsuRuleset", "osu.Game.Rulesets.Osu");
        yield return (DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, "osu.Game.Rulesets.Taiko.TaikoRuleset", "osu.Game.Rulesets.Taiko");
        yield return (DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, "osu.Game.Rulesets.Catch.CatchRuleset", "osu.Game.Rulesets.Catch");
        yield return (DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, "osu.Game.Rulesets.Mania.ManiaRuleset", "osu.Game.Rulesets.Mania");
    }

    private static IEnumerable<DynamicDependency> GetModDependencies(Compilation compilation)
    {
        (string Assembly, string Namespace)[] modNamespaces = [
            ("osu.Game", "osu.Game.Rulesets.Mods"),
            ("osu.Game.Rulesets.Osu", "osu.Game.Rulesets.Osu.Mods"),
            ("osu.Game.Rulesets.Taiko", "osu.Game.Rulesets.Taiko.Mods"),
            ("osu.Game.Rulesets.Catch", "osu.Game.Rulesets.Catch.Mods"),
            ("osu.Game.Rulesets.Mania", "osu.Game.Rulesets.Mania.Mods")
        ];

        foreach ((string assembly, string ns) in modNamespaces)
            foreach (INamedTypeSymbol type in GetTypesInNamespace(compilation, assembly, ns))
            {
                string name = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat
                    .WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted) // global::Foo<T> -> Foo<T>
                    .WithGenericsOptions(SymbolDisplayGenericsOptions.None)); // Foo<T> -> Foo

                if (type.IsGenericType)
                    name += $"`{type.TypeParameters.Length}"; // Foo -> Foo`1

                yield return (DynamicallyAccessedMemberTypes.PublicProperties, name, assembly);
            }
    }

    private static ImmutableArray<INamedTypeSymbol> GetTypesInNamespace(Compilation compilation, string assemblyName, string namespaceName)
    {
        IAssemblySymbol assembly = compilation.References.Select(compilation.GetAssemblyOrModuleSymbol)
           .OfType<IAssemblySymbol>()
           .First(x => x.Name == assemblyName);

        INamespaceSymbol ns = assembly.GlobalNamespace;
        foreach (string part in namespaceName.Split('.'))
        {
            ns = ns.GetNamespaceMembers().FirstOrDefault(n => n.Name == part);
            if (ns is null) // Namespace not found after walking down the path
                return [];
        }

        return ns.GetTypeMembers();
    }
}

internal enum DynamicallyAccessedMemberTypes
{
    PublicProperties,
    PublicParameterlessConstructor
}