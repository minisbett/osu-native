using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace osu.Native.Analyzers;

[Generator]
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NativeObjectAnalyzer : DiagnosticAnalyzer
{
#pragma warning disable RS2008
  private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor("OSU001", "Invalid NativeObject layout",
    "The 'Id' property of a native object must be declared as the first property or field", "Usage", DiagnosticSeverity.Error, true,
    "Ensures the 'Id' property is always declared first in the struct.");

  public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

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

    var idProperty = structSymbol.GetMembers("ObjectId").FirstOrDefault();
    var members = structDeclaration.Members.Where(x => x is PropertyDeclarationSyntax or FieldDeclarationSyntax && !x.Modifiers.Any(SyntaxKind.StaticKeyword));
    if (idProperty is not null && members.First() != idProperty.DeclaringSyntaxReferences.First().GetSyntax())
      context.ReportDiagnostic(Diagnostic.Create(Rule, idProperty.Locations[0]));
  }
}