using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace osu.Native.Analyzers;

[Generator]
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NativeObjectAnalyzer : DiagnosticAnalyzer
{
#pragma warning disable RS2008
  private static readonly DiagnosticDescriptor RuleOSU001 = new DiagnosticDescriptor("OSU001", "Invalid NativeObject layout",
    "The 'ObjectId' property of a native object must be declared as the first property or field", "Usage", DiagnosticSeverity.Error, true,
    "Ensures the 'ObjectId' property is always declared first in the struct.");

  private static readonly DiagnosticDescriptor RuleOSU002 = new DiagnosticDescriptor("OSU002", "API functions must return an error code",
    "The return type of a native function must be osu.Native.ErrorCode", "Usage", DiagnosticSeverity.Error, true,
    "Ensures all native functions return an error code.");

  public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [RuleOSU001, RuleOSU002];

  public override void Initialize(AnalysisContext context)
  {
    context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
    context.EnableConcurrentExecution();
    context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.StructDeclaration);
  }

  private void Analyze(SyntaxNodeAnalysisContext context)
  {
    var structDeclaration = (StructDeclarationSyntax)context.Node;
    var structSymbol = context.SemanticModel.GetDeclaredSymbol(structDeclaration);
    var iNativeObjectSymbol = context.Compilation.GetTypeByMetadataName("osu.Native.Objects.INativeObject`1");

    if (!structSymbol.AllInterfaces.Any(x => x.OriginalDefinition.Equals(iNativeObjectSymbol, SymbolEqualityComparer.Default)))
      return;

    // RuleOSU001: The 'ObjectId' property of a native object must be declared as the first property or field
    var idProperty = structSymbol.GetMembers("ObjectId").FirstOrDefault();
    var members = structDeclaration.Members.Where(x => x is PropertyDeclarationSyntax or FieldDeclarationSyntax && !x.Modifiers.Any(SyntaxKind.StaticKeyword));
    if (idProperty is not null && members.First() != idProperty.DeclaringSyntaxReferences.First().GetSyntax())
      context.ReportDiagnostic(Diagnostic.Create(RuleOSU001, idProperty.Locations[0]));

    // RuleOSU002: The return type of a native function must be osu.Native.ErrorCode
    foreach (IMethodSymbol method in structSymbol.GetMembers().OfType<IMethodSymbol>())
    {
      if (!method.GetAttributes().Any(x => x.AttributeClass?.Name == "OsuNativeFunctionAttribute"))
        continue;

      if (method.ReturnType.Name == "ErrorCode")
        continue;

      context.ReportDiagnostic(Diagnostic.Create(RuleOSU002, method.Locations[0]));
    }
  }
}