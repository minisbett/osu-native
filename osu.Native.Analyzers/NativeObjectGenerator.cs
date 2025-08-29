using System.Collections.Generic;
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

        IncrementalValueProvider<(Compilation Left, System.Collections.Immutable.ImmutableArray<ClassDeclarationSyntax> Right)> source = context.CompilationProvider.Combine(classes.Collect());

        context.RegisterSourceOutput(source, static (ctx, source) =>
        {
            INamedTypeSymbol iOsuNativeObjectSymbol = source.Left.GetTypeByMetadataName("osu.Native.Compiler.IOsuNativeObject`1");
            INamedTypeSymbol osuNativeFunctionSymbol = source.Left.GetTypeByMetadataName("osu.Native.Compiler.OsuNativeFunctionAttribute");
            INamedTypeSymbol osuNativeFieldSymbol = source.Left.GetTypeByMetadataName("osu.Native.Compiler.OsuNativeFieldAttribute");

            foreach (ClassDeclarationSyntax declaration in source.Right)
            {
                SemanticModel model = source.Left.GetSemanticModel(declaration.SyntaxTree);
                INamedTypeSymbol @class = model.GetDeclaredSymbol(declaration);

                ITypeSymbol managedObjectSymbol = @class.AllInterfaces.FirstOrDefault(x => x.OriginalDefinition.Equals(iOsuNativeObjectSymbol, SymbolEqualityComparer.Default))?.TypeArguments.FirstOrDefault();
                if (managedObjectSymbol is null)
                    continue;

                List<IMethodSymbol> nativeMethods = [];
                foreach (IMethodSymbol method in @class.GetMembers().OfType<IMethodSymbol>())
                    if (method.IsStatic && method.GetAttributes().Any(x => x.AttributeClass.Equals(osuNativeFunctionSymbol, SymbolEqualityComparer.Default)))
                        nativeMethods.Add(method);

                List<IFieldSymbol> nativeFields = [];
                foreach (IFieldSymbol field in @class.GetMembers().OfType<IFieldSymbol>())
                    if (field.GetAttributes().Any(x => x.AttributeClass.Equals(osuNativeFieldSymbol, SymbolEqualityComparer.Default)))
                        nativeFields.Add(field);

                string code = GetNativeObjectSource(@class, managedObjectSymbol, nativeMethods, nativeFields);

                code = CSharpSyntaxTree.ParseText(code).GetRoot().NormalizeWhitespace().ToFullString();
                ctx.AddSource($"{@class.Name}.g.cs", code);
            }
        });
    }

    private static string GetNativeObjectSource(INamedTypeSymbol classSymbol, ITypeSymbol managedObject, IEnumerable<IMethodSymbol> nativeMethods, IEnumerable<IFieldSymbol> nativeFields)
    {
        string objectName = classSymbol.Name.EndsWith("Object") ? classSymbol.Name.Substring(0, classSymbol.Name.Length - 6) : classSymbol.Name;

        string fieldSource = string.Join("\n\n", nativeFields.Select(GetNativeFieldSource));
        string methodSource = string.Join("\n\n", nativeMethods.Select(m => GetNativeMethodSource(classSymbol, m)));

        return $$"""
           using System.Runtime.InteropServices;
           using System.Runtime.CompilerServices;

           namespace {{classSymbol.ContainingNamespace}};
           
           [CompilerGenerated]
           internal unsafe partial class {{classSymbol.Name}}
           {
               {{methodSource}}

               [CompilerGenerated]
               [UnmanagedCallersOnly(EntryPoint = "{{objectName}}_Destroy", CallConvs = [typeof(CallConvCdecl)])]
               private static ErrorCode {{objectName}}_Destroy(ManagedObjectHandle<{{managedObject}}> handle)
               {
                   ErrorHandler.SetLastMessage(null);
                   
                   try
                   {
                       ManagedObjectStore<{{managedObject}}>.Remove(handle);

                       return ErrorCode.Success;
                   }
                   catch (Exception ex)
                   {
                       return ErrorHandler.HandleException(ex);
                   }
               }
           }
           
           [CompilerGenerated]
           internal unsafe struct Native{{objectName}}
           {
               [CompilerGenerated]
               public required ManagedObjectHandle<{{managedObject}}> Handle;

               {{fieldSource}}
           }
           """;
    }

    private static string GetNativeMethodSource(INamedTypeSymbol classSymbol, IMethodSymbol method)
    {
        string objectName = classSymbol.Name.EndsWith("Object") ? classSymbol.Name.Substring(0, classSymbol.Name.Length - 6) : classSymbol.Name;
        string parameters = string.Join(", ", method.Parameters.Select(p => $"{p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {p.Name}"));

        return $$"""
           [CompilerGenerated]
           [UnmanagedCallersOnly(EntryPoint = "{{objectName}}_{{method.Name}}", CallConvs = [typeof(CallConvCdecl)])]
           private static ErrorCode {{objectName}}_{{method.Name}}({{parameters}})
           {
             ErrorHandler.SetLastMessage(null);

             try
             {
               return {{classSymbol}}.{{method.Name}}({{string.Join(", ", method.Parameters.Select(p => p.Name))}});
             }
             catch (Exception ex)
             {
               return ErrorHandler.HandleException(ex);
             }
           }
           """;
    }

    private static string GetNativeFieldSource(IFieldSymbol field)
    {
        string fieldName = field.Name.TrimStart('_');
        fieldName = char.ToUpper(fieldName[0]) + fieldName.Substring(1);

        return $$"""
           [CompilerGenerated]
           public required {{field.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}} {{fieldName}};
           """;
    }
}