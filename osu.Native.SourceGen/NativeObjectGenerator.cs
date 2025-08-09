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
    var classes = context.SyntaxProvider.CreateSyntaxProvider(
        predicate: static (s, _) => s is ClassDeclarationSyntax,
        transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node);


    var source = context.CompilationProvider.Combine(classes.Collect());

    context.RegisterSourceOutput(source, static (ctx, source) =>
    {
      var iOsuNativeObjectSymbol = source.Left.GetTypeByMetadataName("osu.Native.Compiler.IOsuNativeObject`1");
      var osuNativeFunctionSymbol = source.Left.GetTypeByMetadataName("osu.Native.Compiler.OsuNativeFunctionAttribute");
      var osuNativeFieldSymbol = source.Left.GetTypeByMetadataName("osu.Native.Compiler.OsuNativeFieldAttribute");

      foreach (var declaration in source.Right)
      {
        var model = source.Left.GetSemanticModel(declaration.SyntaxTree);
        var @class = model.GetDeclaredSymbol(declaration);

        var managedObjectSymbol = @class.AllInterfaces.FirstOrDefault(x => x.OriginalDefinition.Equals(iOsuNativeObjectSymbol, SymbolEqualityComparer.Default))?.TypeArguments.FirstOrDefault();
        if (managedObjectSymbol is null)
          continue;

        List<string> nativeMethods = [];
        foreach (IMethodSymbol method in @class.GetMembers().OfType<IMethodSymbol>())
          if (method.IsStatic && method.GetAttributes().Any(x => x.AttributeClass.Equals(osuNativeFunctionSymbol, SymbolEqualityComparer.Default)))
            nativeMethods.Add(GetNativeMethodSource(model, @class, method));

        List<string> nativeFields = [];
        foreach (IFieldSymbol field in @class.GetMembers().OfType<IFieldSymbol>())
          if (field.GetAttributes().Any(x => x.AttributeClass.Equals(osuNativeFieldSymbol, SymbolEqualityComparer.Default)))
            nativeFields.Add(GetNativeFieldSource(field));

        string code = GetSource(@class, managedObjectSymbol, nativeMethods, nativeFields);

        code = CSharpSyntaxTree.ParseText(code).GetRoot().NormalizeWhitespace().ToFullString();
        ctx.AddSource($"{@class.Name}.g.cs", code);
      }
    });
  }

  private static string GetSource(INamedTypeSymbol @class, ITypeSymbol managedObject, IEnumerable<string> nativeMethods, IEnumerable<string> nativeFields)
  {
    string nativeObjectName = @class.Name.EndsWith("Object") ? @class.Name.Substring(0, @class.Name.Length - 6) : @class.Name;
    
    return $$"""
           using System.Runtime.InteropServices;
           using System.Runtime.CompilerServices;

           namespace {{@class.ContainingNamespace}};
           
           [CompilerGenerated]
           internal unsafe partial class {{@class.Name}}
           {
               {{string.Join("\n\n", nativeMethods)}}

               [CompilerGenerated]
               [UnmanagedCallersOnly(EntryPoint = "{{nativeObjectName}}_Destroy", CallConvs = [typeof(CallConvCdecl)])]
               private static ErrorCode {{nativeObjectName}}_Destroy(Native{{nativeObjectName}} obj)
               {
                   ErrorHandler.SetLastMessage(null);
                   
                   try
                   {
                       {{managedObject}} managedObj = obj.Resolve();
                       if (managedObj is IDisposable disposable)
                           disposable.Dispose();

                       ObjectContainer<{{managedObject}}>.Remove(obj.ObjectId);
                       return ErrorCode.Success;
                   }
                   catch (Exception ex)
                   {
                       return ErrorHandler.HandleException(ex);
                   }
               }
           }
           
           [CompilerGenerated]
           internal unsafe struct Native{{nativeObjectName}}
           {
               [CompilerGenerated]
               public required int ObjectId { get; init; }

               {{string.Join("\n\n", nativeFields)}}

               [CompilerGenerated]
               public {{managedObject}} Resolve() => ObjectContainer<{{managedObject}}>.Get(ObjectId);
           }
           """;
  }

  private static string GetNativeMethodSource(SemanticModel model, INamedTypeSymbol @class, IMethodSymbol method)
  {
    string nativeObjectName = @class.Name.EndsWith("Object") ? @class.Name.Substring(0, @class.Name.Length - 6) : @class.Name;
    string parameters = string.Join(", ", method.Parameters.Select(p => $"{p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {p.Name}"));
    string methodCall = $"{@class}.{method.Name}({string.Join(", ", method.Parameters.Select(p => p.Name))})";

    return $$"""
           [CompilerGenerated]
           [UnmanagedCallersOnly(EntryPoint = "{{nativeObjectName}}_{{method.Name}}", CallConvs = [typeof(CallConvCdecl)])]
           private static ErrorCode {{nativeObjectName}}_{{method.Name}}({{parameters}})
           {
             ErrorHandler.SetLastMessage(null);

             try
             {
               return {{methodCall}};
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
           public required {{field.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}} {{fieldName}} { get; init; }
           """;
  }
}