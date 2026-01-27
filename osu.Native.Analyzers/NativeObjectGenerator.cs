using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace osu.Native.Analyzers;

[Generator]
public class NativeObjectGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ClassDeclarationSyntax> classes = context.SyntaxProvider.CreateSyntaxProvider(
        predicate: static (s, _) => s is ClassDeclarationSyntax,
        transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node);

        IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> source = context.CompilationProvider.Combine(classes.Collect());

        context.RegisterSourceOutput(source, static (ctx, source) =>
        {
            INamedTypeSymbol iOsuNativeObjectSymbol = source.Item1.GetTypeByMetadataName("osu.Native.Compiler.IOsuNativeObject`1");
            INamedTypeSymbol osuNativeFunctionSymbol = source.Item1.GetTypeByMetadataName("osu.Native.Compiler.OsuNativeFunctionAttribute");
            foreach (ClassDeclarationSyntax declaration in source.Item2)
            {
                SemanticModel model = source.Item1.GetSemanticModel(declaration.SyntaxTree);
                INamedTypeSymbol classSymbol = model.GetDeclaredSymbol(declaration);

                // Make sure the class inherits IOsuNativeObject<T>.
                ITypeSymbol managedObjectSymbol = classSymbol.AllInterfaces.FirstOrDefault(
                    x => x.OriginalDefinition.Equals(iOsuNativeObjectSymbol, SymbolEqualityComparer.Default))?.TypeArguments.FirstOrDefault();
                if (managedObjectSymbol is null)
                    continue;

                // Get all methods marked with [OsuNativeFunction].
                List<IMethodSymbol> nativeFunctions = [];
                foreach (IMethodSymbol method in classSymbol.GetMembers().OfType<IMethodSymbol>())
                    if (method.IsStatic && method.GetAttributes().Any(x => x.AttributeClass.Equals(osuNativeFunctionSymbol, SymbolEqualityComparer.Default)))
                        nativeFunctions.Add(method);

                string code = GetPartialClassSource(classSymbol, [..nativeFunctions], managedObjectSymbol);

                code = CSharpSyntaxTree.ParseText(code).GetRoot().NormalizeWhitespace().ToFullString();
                ctx.AddSource($"{classSymbol.Name}.g.cs", code);
            }
        });
    }

    private static string GetPartialClassSource(INamedTypeSymbol classSymbol, IMethodSymbol[] methods, ITypeSymbol managedObjectSymbol)
    {
        string objectName = classSymbol.Name.EndsWith("Object") ? classSymbol.Name.Substring(0, classSymbol.Name.Length - 6) : classSymbol.Name;
        string functionsSource = string.Join("\n\n", methods.Select(x => GetNativeFunctionSource(x, objectName)));

        return $$"""
               using System.Runtime.InteropServices;
               using System.Runtime.CompilerServices;
               
               namespace {{classSymbol.ContainingNamespace}};
               
               [CompilerGenerated]
               internal unsafe partial class {{classSymbol.Name}}
               {
                   {{functionsSource}}
               
                   [CompilerGenerated]
                   [UnmanagedCallersOnly(EntryPoint = "{{objectName}}_Destroy", CallConvs = [typeof(CallConvCdecl)])]
                   private static ErrorCode {{objectName}}_Destroy(ManagedObjectHandle<{{managedObjectSymbol}}> handle)
                   {
                       ErrorHandler.SetLastMessage(null);
                       
                       try
                       {
                            ManagedObjectStore<{{managedObjectSymbol}}>.Remove(handle);
                
                            return ErrorCode.Success;
                       }
                       catch (Exception ex)
                       {
                           return ErrorHandler.HandleException(ex);
                       }
                   }
               }
               """;
    }

    private static string GetNativeFunctionSource(IMethodSymbol method, string objectName)
    {
        string parameters = string.Join(", ", method.Parameters.Select(p => $"{p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {p.Name}"));
        
        return $$"""
               [CompilerGenerated]
               [UnmanagedCallersOnly(EntryPoint = "{{objectName}}_{{method.Name}}", CallConvs = [typeof(CallConvCdecl)])]
               private static ErrorCode {{objectName}}_{{method.Name}}({{parameters}})
               {
                   ErrorHandler.SetLastMessage(null);
        
                   try
                   {
                       return {{method.Name}}({{string.Join(", ", method.Parameters.Select(p => p.Name))}});
                   }
                   catch (Exception ex)
                   {
                       return ErrorHandler.HandleException(ex);
                   }
               }
               """;
    }
}