using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Extensions;

public static class NamedTypeSymbolExtensions
{
    public static string GetTypeKindString(this INamedTypeSymbol symbol)
    {
        if (symbol.TypeKind == TypeKind.Class && symbol.IsAbstract)
        {
            return "abstract class";
        }
        else if (symbol.TypeKind is TypeKind.Struct or TypeKind.Structure)
        {
            return "struct";
        }
        return symbol.TypeKind.ToString().ToLower();

    }

    public static string GetModifiersString(this INamedTypeSymbol symbol)
    {
        var modifiers = string.Join(" ",
            new[]
            {
                symbol.GetAttributes()
                    .Any(a=>a.AttributeClass?.ContainingNamespace.Name=="System"
                        && a.AttributeClass?.Name == "FlagsAttribute") ? "<<Flags>>" : "",
                symbol.IsStatic ? "<<static>>" : "",
                symbol.IsSealed ? "<<sealed>>" : "",
                symbol.IsFileLocal ? "<<file>>" : "",
                symbol.IsReadOnly ? "<<readonly>>" : "",
                symbol.IsRecord ? "<<record>>": "",
            }.Where(s => !string.IsNullOrEmpty(s)));
        if (modifiers != string.Empty)
        {
            modifiers += " ";
        }
        return modifiers;
    }

    public static string GetTypeParamtersString(this INamedTypeSymbol symbol)
    {
        return symbol.IsGenericType && symbol.TypeArguments.Any()
            ? $"<{string.Join(", ", symbol.TypeParameters.Select(arg => arg.Name))}>"
            : "";
    }

    public static string GetTypeArgumentsString(this INamedTypeSymbol symbol)
    {
        return symbol.IsGenericType && symbol.TypeArguments.Where(arg => arg.Kind != SymbolKind.TypeParameter).Any()
            ? $"<{string.Join(", ", symbol.TypeArguments.Select(arg => arg.Name))}>"
            : "";
    }

    public static bool ContainsObjectCreationInConstructor(this INamedTypeSymbol symbol, ITypeSymbol type)
    {
        return symbol.Constructors
            .SelectMany(x => x.DeclaringSyntaxReferences)
            .Select(syntaxRef => syntaxRef.GetSyntax())
            .OfType<ConstructorDeclarationSyntax>()
            .SelectMany(syntax => syntax.Body?.DescendantNodes())
            .OfType<ObjectCreationExpressionSyntax>()
            .Any(node => node.Type.ToString() == type.Name);
    }
}
