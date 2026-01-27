using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
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
    private static readonly DiagnosticDescriptor RuleOSU002 = new("OSU001", "Native functions must be static",
        "Native functions must be static", "Usage", DiagnosticSeverity.Error, true);

    private static readonly DiagnosticDescriptor RuleOSU003 = new("OSU002", "Native functions return an ErrorCode",
        "Native functions must return an ErrorCode", "Usage", DiagnosticSeverity.Error, true);

    private static readonly DiagnosticDescriptor RuleOSU001 = new("OSU003", "Native functions may only exist in a native object",
        "Native functions may only exist in a native object (IOsuNativeObject<T>)", "Usage", DiagnosticSeverity.Error, true);

    private static readonly DiagnosticDescriptor RuleOSU004 = new("OSU003", "Strings should be UTF-8",
        "Strings should be handled with UTF-8 encoding", "Usage", DiagnosticSeverity.Warning, true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [RuleOSU002, RuleOSU003, RuleOSU001, RuleOSU004];

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
        INamedTypeSymbol iOsuNativeObjectSymbol = context.Compilation.GetTypeByMetadataName("osu.Native.Compiler.IOsuNativeObject`1");
        INamedTypeSymbol errorCodeSymbol = context.Compilation.GetTypeByMetadataName("osu.Native.ErrorCode");
        INamedTypeSymbol osuNativeFunctionSymbol = context.Compilation.GetTypeByMetadataName("osu.Native.Compiler.OsuNativeFunctionAttribute");

        foreach (IMethodSymbol method in classSymbol.GetMembers().OfType<IMethodSymbol>())
        {
            if (!method.GetAttributes().Any(x => x.AttributeClass.Equals(osuNativeFunctionSymbol, SymbolEqualityComparer.Default)))
                continue;

            // RuleOSU001: Native functions may only exist in a native object
            if (!classSymbol.AllInterfaces.Any(x => x.OriginalDefinition.Equals(iOsuNativeObjectSymbol, SymbolEqualityComparer.Default)))
                context.ReportDiagnostic(Diagnostic.Create(RuleOSU001, method.Locations[0]));

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
    }
}