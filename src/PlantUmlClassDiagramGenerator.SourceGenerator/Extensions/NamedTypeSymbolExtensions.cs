using Microsoft.CodeAnalysis;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Extensions;

public static class NamedTypeSymbolExtensions
{
    public static string GetTypeKindString(this INamedTypeSymbol symbol)
    {
        if (symbol.TypeKind == TypeKind.Class && symbol.IsAbstract)
        {
            return "abstract class";
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

    public static string GetTypeArgumentsString(this INamedTypeSymbol symbol)
    {
        return symbol.IsGenericType && symbol.TypeArguments.Any()
            ? $"<{string.Join(", ", symbol.TypeArguments.Select(arg => arg.Name))}>"
            : "";
    }
}
