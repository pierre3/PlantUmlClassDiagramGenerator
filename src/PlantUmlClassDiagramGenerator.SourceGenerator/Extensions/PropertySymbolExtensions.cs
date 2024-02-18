using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Extensions;

public static class PropertySymbolExtensions
{

    public static string GetTypeString(this IPropertySymbol symbol)
    {
        var fieldTag = symbol.Type.Name == nameof(ValueTuple) ? "{field} " : "";
        var typeName = symbol.Type.GetTypeName();
        return $"{fieldTag}{typeName}";
    }

    public static string GetAccessorString(this IPropertySymbol symbol)
    {
        var getter = symbol.GetMethod is null
            ? ""
            : $"<<{symbol.GetMethod.DeclaredAccessibility.GetAccessorAccessibilityString(symbol.DeclaredAccessibility)}get>>";
        var setter = symbol.SetMethod is null
            ? ""
            : $"<<{symbol.SetMethod.DeclaredAccessibility.GetAccessorAccessibilityString(symbol.DeclaredAccessibility)}set>>";
        return string.Join(" ", new[] { getter, setter }.Where(s => !string.IsNullOrEmpty(s)));
    }

    public static string GetModifiersString(this IPropertySymbol symbol)
    {
        var modifiers = string.Join(" ",
            new[]
            {
                symbol.ContainingType.TypeKind is not TypeKind.Interface 
                    && symbol.IsAbstract ? "{abstract}" : "",
                symbol.IsStatic ? "{static}" : "",
                symbol.IsSealed ? "<<sealed>>" : "",
                symbol.IsReadOnly ? "<<readonly>>" : "",
                symbol.IsRequired ? "<<required>>" : "",
                symbol.IsOverride ? "<<override>>" : "",
                symbol.IsVirtual ? "<<virtual>>" : "",
            }.Where(s => !string.IsNullOrEmpty(s)));
        if (modifiers != string.Empty)
        {
            modifiers += " ";
        }
        return modifiers;
    }

    public static bool HasPropertyInitializer(this IPropertySymbol symbol)
    {
        return symbol.DeclaringSyntaxReferences
            .Select(syntaxRef => syntaxRef.GetSyntax())
            .OfType<PropertyDeclarationSyntax>()
            .Any(syntax => syntax.Initializer is not null);
    }
}
