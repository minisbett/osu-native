using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

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

        string objectName = structSymbol.Name.StartsWith("Native") ? structSymbol.Name.Substring(6) : structSymbol.Name;

        List<string> nativeMethods = [];
        foreach (IMethodSymbol method in structSymbol.GetMembers().OfType<IMethodSymbol>())
        {
          if (!method.GetAttributes().Any(x => x.AttributeClass?.Name == "OsuNativeFunctionAttribute"))
            continue;

          nativeMethods.Add(GetNativeMethodSource(model, objectName, method));
        }

        string code = GetSource(structSymbol, objectName, nativeMethods);

        code = CSharpSyntaxTree.ParseText(code).GetRoot().NormalizeWhitespace().ToFullString();
        ctx.AddSource($"{structSymbol.Name}.g.cs", code);
      }
    });
  }

  private static string GetSource(INamedTypeSymbol @struct, string objectName, IEnumerable<string> nativeMethods)
    => $$"""
       using System.Runtime.InteropServices;
       using System.Runtime.CompilerServices;

       namespace {{@struct.ContainingNamespace}};
           
       internal unsafe struct {{@struct.Name}}
       {
           [CompilerGenerated]
           private static {{@struct.Name}} Resolve(this {{@struct.Name}} obj) => ObjectContainer<{{@struct.Name}}>.Get(obj.ObjectId);

           {{string.Join("\n", nativeMethods)}}

           [CompilerGenerated]
           [UnmanagedCallersOnly(EntryPoint = "{{objectName}}_Destroy", CallConvs = [typeof(CallConvCdecl)])]
           private static ErrorCode Destroy({{@struct.Name}} obj)
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
       }
       """;

  private static string GetNativeMethodSource(SemanticModel model, string objectName, IMethodSymbol method)
  {
    string parameters = string.Join(", ", method.Parameters.Select(p => $"{p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {p.Name}"));
    var methodDeclaration = method.DeclaringSyntaxReferences[0].GetSyntax() as MethodDeclarationSyntax;

    var typeSyntaxes = methodDeclaration.DescendantNodes().OfType<TypeSyntax>();
    methodDeclaration = methodDeclaration.ReplaceNodes(typeSyntaxes, (node, _) =>
    {
      if (model.GetSymbolInfo(node).Symbol is not INamedTypeSymbol typeSymbol)
        return node;

      string qualifiedName = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

      if (typeSymbol.IsGenericType)
      {
        var genericArguments = node.DescendantNodes().OfType<GenericNameSyntax>()
            .Select(genNode =>
            {
              var genericArgs = genNode.TypeArgumentList.Arguments.Select(argNode =>
              {
                var argSymbolInfo = model.GetSymbolInfo(argNode);
                return argSymbolInfo.Symbol is INamedTypeSymbol argTypeSymbol
                    ? SyntaxFactory.ParseTypeName(argTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
                    : argNode;
              }).ToList();

              return genNode.WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(genericArgs)));
            }).ToList();

        return SyntaxFactory.ParseTypeName(qualifiedName)
            .WithTriviaFrom(node);
      }

      return SyntaxFactory.ParseTypeName(qualifiedName).WithTriviaFrom(node);
    });

    return $$"""
           [CompilerGenerated]
           [UnmanagedCallersOnly(EntryPoint = "{{objectName}}_{{method.Name}}", CallConvs = [typeof(CallConvCdecl)])]
           private static ErrorCode {{objectName}}_{{method.Name}}({{parameters}})
           {
             try
             {
               {{methodDeclaration.Body?.Statements.ToFullString() ?? $"return {methodDeclaration.ExpressionBody.Expression.ToFullString()};"}}
             }
             catch (Exception ex)
             {
               return ErrorHandler.Handle(ex);
             }
           }
           """;
  }
}