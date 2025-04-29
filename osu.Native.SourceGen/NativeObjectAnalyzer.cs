using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace osu.Native.Analyzers;

[Generator]
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NativeObjectAnalyzer : DiagnosticAnalyzer
{
#pragma warning disable RS2008
  private static readonly DiagnosticDescriptor RuleOSU001 = new("OSU001", "Native fields must be unmanaged", "Native fields must be of an unmanaged type", 
    "Usage", DiagnosticSeverity.Error, true, "Ensures all native fields are of an unmanaged type.");

  private static readonly DiagnosticDescriptor RuleOSU002 = new("OSU002", "Native functions must be static", "Native functions must be static",
    "Usage", DiagnosticSeverity.Error, true, "Ensures all native functions are static.");

  public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [RuleOSU001, RuleOSU002];

  public override void Initialize(AnalysisContext context)
  {
    context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
    context.EnableConcurrentExecution();
    context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.ClassDeclaration);
  }

  private void Analyze(SyntaxNodeAnalysisContext context)
  {
    var declaration = (ClassDeclarationSyntax)context.Node;
    var @class = context.SemanticModel.GetDeclaredSymbol(declaration);
    var iNativeObjectSymbol = context.Compilation.GetTypeByMetadataName("osu.Native.Compiler.IOsuNativeObject`1");
    var osuNativeFieldSymbol = context.Compilation.GetTypeByMetadataName("osu.Native.Compiler.OsuNativeFieldAttribute");
    var osuNativeFunctionSymbol = context.Compilation.GetTypeByMetadataName("osu.Native.Compiler.OsuNativeFunctionAttribute");

    if (!@class.AllInterfaces.Any(x => x.OriginalDefinition.Equals(iNativeObjectSymbol, SymbolEqualityComparer.Default)))
      return;

    // RuleOSU001: Native fields must be of an unmanaged type
    foreach (IFieldSymbol field in @class.GetMembers().OfType<IFieldSymbol>())
    {
      if (!field.GetAttributes().Any(x => x.AttributeClass.Equals(osuNativeFieldSymbol, SymbolEqualityComparer.Default)))
        continue;

      if (!field.Type.IsUnmanagedType)
        context.ReportDiagnostic(Diagnostic.Create(RuleOSU001, field.Locations[0]));
    }

    // RuleOSU002: Native functions must be static
    foreach (IMethodSymbol method in @class.GetMembers().OfType<IMethodSymbol>())
    {
      if (!method.GetAttributes().Any(x => x.AttributeClass.Equals(osuNativeFunctionSymbol, SymbolEqualityComparer.Default)))
        continue;

      if (!method.IsStatic)
        context.ReportDiagnostic(Diagnostic.Create(RuleOSU002, method.Locations[0]));
    }
  }
}