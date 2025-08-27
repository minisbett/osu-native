using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

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

    private static readonly DiagnosticDescriptor RuleOSU003 = new("OSU003", "Native functions return an ErrorCode", "Native functions must return an ErrorCode",
      "Usage", DiagnosticSeverity.Error, true, "Ensures all native functions return an ErrorCode.");

    private static readonly DiagnosticDescriptor RuleOSU004 = new("OSU004", "Strings should be UTF-8", "Strings should be handled with UTF-8 encoding",
      "Usage", DiagnosticSeverity.Warning, true, "Warns the user if a string is not handled with UTF-8 encoding (byte* instead of char*).");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [RuleOSU001, RuleOSU002, RuleOSU003, RuleOSU004];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.ClassDeclaration);
    }

    private void Analyze(SyntaxNodeAnalysisContext context)
    {
        ClassDeclarationSyntax classDeclaration = (ClassDeclarationSyntax)context.Node;
        INamedTypeSymbol classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);
        INamedTypeSymbol errorCodeSymbol = context.Compilation.GetTypeByMetadataName("osu.Native.ErrorCode");
        INamedTypeSymbol iNativeObjectSymbol = context.Compilation.GetTypeByMetadataName("osu.Native.Compiler.IOsuNativeObject`1");
        INamedTypeSymbol osuNativeFieldSymbol = context.Compilation.GetTypeByMetadataName("osu.Native.Compiler.OsuNativeFieldAttribute");
        INamedTypeSymbol osuNativeFunctionSymbol = context.Compilation.GetTypeByMetadataName("osu.Native.Compiler.OsuNativeFunctionAttribute");

        foreach (IMethodSymbol method in classSymbol.GetMembers().OfType<IMethodSymbol>())
        {
            if (!method.GetAttributes().Any(x => x.AttributeClass.Equals(osuNativeFunctionSymbol, SymbolEqualityComparer.Default)))
                continue;

            // RuleOSU002: Native functions must be static
            if (!method.IsStatic)
                context.ReportDiagnostic(Diagnostic.Create(RuleOSU002, method.Locations[0]));

            // RuleOSU003: Native functions must return an ErrorCode
            if (!method.ReturnType.Equals(errorCodeSymbol, SymbolEqualityComparer.Default))
                context.ReportDiagnostic(Diagnostic.Create(RuleOSU003, method.Locations[0]));

            // RuleOSU004: Strings should be handled with UTF-8 encoding
            foreach (IParameterSymbol parameter in method.Parameters)
                if (parameter.Type is IPointerTypeSymbol pointer && pointer.PointedAtType.SpecialType == SpecialType.System_Char)
                    context.ReportDiagnostic(Diagnostic.Create(RuleOSU004, parameter.Locations[0]));
        }

        if (!classSymbol.AllInterfaces.Any(x => x.OriginalDefinition.Equals(iNativeObjectSymbol, SymbolEqualityComparer.Default)))
            return;

        // RuleOSU001: Native fields must be of an unmanaged type
        foreach (IFieldSymbol field in classSymbol.GetMembers().OfType<IFieldSymbol>())
        {
            if (!field.GetAttributes().Any(x => x.AttributeClass.Equals(osuNativeFieldSymbol, SymbolEqualityComparer.Default)))
                continue;

            if (field.Type.IsUnmanagedType)
                continue;

            context.ReportDiagnostic(Diagnostic.Create(RuleOSU001, field.Locations[0]));
        }
    }
}