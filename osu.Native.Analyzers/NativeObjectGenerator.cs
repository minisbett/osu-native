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

        IncrementalValueProvider<(Compilation Compilation, ImmutableArray<ClassDeclarationSyntax> Classes)> source =
            context.CompilationProvider.Combine(classes.Collect());

        context.RegisterSourceOutput(source, static (ctx, source) =>
        {
            INamedTypeSymbol iOsuNativeObjectSymbol = source.Compilation.GetTypeByMetadataName("osu.Native.Compiler.IOsuNativeObject`1");
            INamedTypeSymbol osuNativeFunctionSymbol = source.Compilation.GetTypeByMetadataName("osu.Native.Compiler.OsuNativeFunctionAttribute");
            INamedTypeSymbol osuNativeEnumeratorSymbol = source.Compilation.GetTypeByMetadataName("osu.Native.Compiler.OsuNativeEnumeratorAttribute`1");
            foreach (ClassDeclarationSyntax declaration in source.Classes)
            {
                SemanticModel model = source.Compilation.GetSemanticModel(declaration.SyntaxTree);
                INamedTypeSymbol classSymbol = model.GetDeclaredSymbol(declaration);

                // Make sure the class inherits IOsuNativeObject<T>.
                ITypeSymbol managedObjectSymbol = classSymbol.AllInterfaces.FirstOrDefault(
                    x => x.OriginalDefinition.Equals(iOsuNativeObjectSymbol, SymbolEqualityComparer.Default))?.TypeArguments.FirstOrDefault();
                if (managedObjectSymbol is null)
                    continue;

                string objectName = classSymbol.Name.EndsWith("Object") ? classSymbol.Name.Substring(0, classSymbol.Name.Length - 6) : classSymbol.Name;
                List<string> members = [];

                // Get all methods marked with [OsuNativeFunction] (and optionally additionally [OsuNativeEnumerator<T>]).
                foreach (IMethodSymbol method in classSymbol.GetMembers().OfType<IMethodSymbol>())
                    if (method.GetAttributes().Any(x => x.AttributeClass.Equals(osuNativeFunctionSymbol, SymbolEqualityComparer.Default)))
                    {
                        members.Add(GetNativeFunctionSource(method, objectName));

                        AttributeData enumeratorAttribute = method.GetAttributes().FirstOrDefault(
                            x => x.AttributeClass.OriginalDefinition.Equals(osuNativeEnumeratorSymbol, SymbolEqualityComparer.Default));
                        if (enumeratorAttribute is not null)
                            members.Add(GetNativeEnumeratorSource(method, objectName, enumeratorAttribute.AttributeClass.TypeArguments[0]));
                    }

                string code = GetPartialClassSource(classSymbol, objectName, [.. members], managedObjectSymbol);
                code = CSharpSyntaxTree.ParseText(code).GetRoot().NormalizeWhitespace().ToFullString();
                ctx.AddSource($"{classSymbol.Name}.g.cs", code);
            }
        });
    }

    private static string GetPartialClassSource(INamedTypeSymbol classSymbol, string objectName, string[] members, ITypeSymbol managedObjectSymbol)
    {
        return $$"""
               using System.Runtime.InteropServices;
               using System.Runtime.CompilerServices;
               
               namespace {{classSymbol.ContainingNamespace}};
               
               [CompilerGenerated]
               internal unsafe partial class {{classSymbol.Name}}
               {
                   {{string.Join("\n\n", members)}}
               
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

    private static string GetNativeEnumeratorSource(IMethodSymbol method, string objName, ITypeSymbol enumType)
    {
        return $$"""
               [CompilerGenerated]
               [UnmanagedCallersOnly(EntryPoint = "{{objName}}_{{method.Name}}_Next", CallConvs = [typeof(CallConvCdecl)])]
               public static ErrorCode {{objName}}_{{method.Name}}_Next(ManagedObjectHandle<IEnumerator<{{enumType}}>> enumeratorHandle, {{enumType}}* obj)
               {
                   ErrorHandler.SetLastMessage(null);

                   try
                   {
                       IEnumerator<{{enumType}}> enumerator = handle.Resolve();

                       if (!enumerator.MoveNext())
                           return ErrorCode.EndOfEnumeration;

                       *obj = enumerator.Current;

                       return ErrorCode.Success;
                   }
                   catch (Exception ex)
                   {
                       return ErrorHandler.HandleException(ex);
                   }
               }
               
               [CompilerGenerated]
               [UnmanagedCallersOnly(EntryPoint = "{{objName}}_{{method.Name}}_Destroy", CallConvs = [typeof(CallConvCdecl)])]
               public static ErrorCode {{objName}}_{{method.Name}}_Destroy(ManagedObjectHandle<IEnumerator<{{enumType}}>> enumeratorHandle)
               {
                   ErrorHandler.SetLastMessage(null);
               
                   try
                   {
                       ManagedObjectStore<IEnumerator<{{enumType}}>>.Remove(handle);
                       return ErrorCode.Success;
                   }
                   catch (Exception ex)
                   {
                       return ErrorHandler.HandleException(ex);
                   }
               }
               """;
    }
}