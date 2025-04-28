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
        predicate: static (s, _) => s is ClassDeclarationSyntax,
        transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node);


    var source = context.CompilationProvider.Combine(structs.Collect());

    context.RegisterSourceOutput(source, static (ctx, source) =>
    {
      var (compilation, classDelcarations) = source;
      var iOsuNativeObjectSymbol = compilation.GetTypeByMetadataName("osu.Native.Objects.IOsuNativeObject`1");
      var osuNativeFunctionSymbol = compilation.GetTypeByMetadataName("osu.Native.Objects.OsuNativeFunctionAttribute");

      foreach (var classDeclaration in classDelcarations)
      {
        var model = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
        var @class = model.GetDeclaredSymbol(classDeclaration);

        var managedObjectSymbol = @class.AllInterfaces.FirstOrDefault(x => x.OriginalDefinition.Equals(iOsuNativeObjectSymbol, SymbolEqualityComparer.Default))?.TypeArguments.FirstOrDefault();
        if (managedObjectSymbol is null)
          continue;

        List<string> members = [];

        foreach (IMethodSymbol method in @class.GetMembers().OfType<IMethodSymbol>())
        {
          if (!method.GetAttributes().Any(x => x.AttributeClass.Equals(osuNativeFunctionSymbol, SymbolEqualityComparer.Default)))
            continue;

          members.Add(GetNativeMethodSource(model, @class, method));
        }

        string code = GetSource(@class, managedObjectSymbol, members);

        code = CSharpSyntaxTree.ParseText(code).GetRoot().NormalizeWhitespace().ToFullString();
        ctx.AddSource($"{@class.Name}.g.cs", code);
      }
    });
  }

  private static string GetSource(INamedTypeSymbol @class, ITypeSymbol managedObject, IEnumerable<string> members)
  {
    string nativeObjectName = @class.Name.EndsWith("Object") ? @class.Name.Substring(0, @class.Name.Length - 6) : @class.Name;
    string managedObjectName = managedObject.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

    return $$"""
           using System.Runtime.InteropServices;
           using System.Runtime.CompilerServices;

           namespace {{@class.ContainingNamespace}};

           internal unsafe partial class {{@class.Name}}
           {
               {{string.Join("\n\n", members)}}

               [CompilerGenerated]
               [UnmanagedCallersOnly(EntryPoint = "{{nativeObjectName}}_Destroy", CallConvs = [typeof(CallConvCdecl)])]
               private static ErrorCode Destroy(Native{{nativeObjectName}} obj)
               {
                   try
                   {
                       if (ObjectContainer<{{managedObjectName}}>.Get(obj.ObjectId) is IDisposable disposable)
                           disposable.Dispose();

                       ObjectContainer<{{managedObjectName}}>.Remove(obj.ObjectId);
                       return ErrorCode.Success;
                   }
                   catch (Exception ex)
                   {
                       return ErrorHandler.Handle(ex);
                   }
               }
           }
           
           internal unsafe readonly struct Native{{nativeObjectName}}
           {
               public required readonly int ObjectId { get; init; }

               [CompilerGenerated]
               public {{managedObjectName}} Resolve() => ObjectContainer<{{managedObjectName}}>.Get(ObjectId);
           }
           """;
  }

  private static string GetNativeMethodSource(SemanticModel model, INamedTypeSymbol @class, IMethodSymbol method)
  {
    string parameters = string.Join(", ", method.Parameters.Select(p => $"{p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {p.Name}"));
    var methodDeclaration = method.DeclaringSyntaxReferences[0].GetSyntax() as MethodDeclarationSyntax;

    // Replace all type names in the method declaration with their fully qualified names.
    var typeSyntaxes = methodDeclaration.DescendantNodes().OfType<TypeSyntax>();
    methodDeclaration = methodDeclaration.ReplaceNodes(typeSyntaxes, (node, _) =>
    {
      if (model.GetSymbolInfo(node).Symbol is not INamedTypeSymbol typeSymbol)
        return node;

      string qualifiedName = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
      return SyntaxFactory.ParseTypeName(qualifiedName).WithTriviaFrom(node);
    });

    return $$"""
           [CompilerGenerated]
           [UnmanagedCallersOnly(EntryPoint = "{{@class.Name}}_{{method.Name}}", CallConvs = [typeof(CallConvCdecl)])]
           private static ErrorCode {{@class.Name}}_{{method.Name}}({{parameters}})
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