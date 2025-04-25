using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;

namespace osu.Native.Analyzers;

[Generator]
public class NativeObjectGenerator : IIncrementalGenerator
{
  public void Initialize(IncrementalGeneratorInitializationContext context)
  {
    var structs = context.SyntaxProvider.CreateSyntaxProvider(
        predicate: static (s, _) => s is StructDeclarationSyntax,
        transform: static (ctx, _) => (StructDeclarationSyntax)ctx.Node);


    var source = context.CompilationProvider.Combine(structs.Collect());

    context.RegisterSourceOutput(source, static (ctx, source) =>
    {
      var (compilation, structs) = source;
      var iNativeObjectSymbol = compilation.GetTypeByMetadataName("osu.Native.Objects.INativeObject`1");

      foreach (var structDeclaration in structs)
      {
        var model = compilation.GetSemanticModel(structDeclaration.SyntaxTree);
        var structSymbol = model.GetDeclaredSymbol(structDeclaration);
        if (!structSymbol.AllInterfaces.Any(x => x.OriginalDefinition.Equals(iNativeObjectSymbol, SymbolEqualityComparer.Default)))
          continue;

        string destroyMethod = GetDestroyMethodSource(structSymbol.Name);
        string code = GetSource(structSymbol, [destroyMethod]);

        code = CSharpSyntaxTree.ParseText(code).GetRoot().NormalizeWhitespace().ToFullString();
        ctx.AddSource($"{structSymbol.Name}.g.cs", code);
      }
    });
  }

  private static string GetSource(INamedTypeSymbol @struct, IEnumerable<string> members)
    => $$"""
       using System.Runtime.InteropServices;
       using System.Runtime.CompilerServices;

       namespace {{@struct.ContainingNamespace}};

       internal partial struct {{@struct.Name}}
       {
       {{string.Join("\n", members)}}
       }
       """;

  private static string GetDestroyMethodSource(string structName)
  {
    string objectName = structName.StartsWith("Native") ? structName.Substring(6) : structName;

    return $$"""
           [UnmanagedCallersOnly(EntryPoint = "{{objectName}}_Destroy", CallConvs = [typeof(CallConvCdecl)])]
           private static ErrorCode Destroy({{structName}} obj)
           {
             try
             {
               obj.Destroy();
               return ErrorCode.Success;
             }
             catch (Exception ex)
             {
               return ErrorHandler.Handle(ex);
             }
           }
           """;
  }
}