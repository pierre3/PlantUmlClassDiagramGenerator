using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Extensions;

public static class FieldSymbolExtensions
{
    public static string GetTypeString(this IFieldSymbol symbol)
    {
        var fieldTag = symbol.Type.Name == nameof(ValueTuple) ? "{field} " : "";
        var typeName = symbol.Type.GetTypeName();
        return $"{fieldTag}{typeName}";
    }

    public static string GetModifiersString(this IFieldSymbol symbol)
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

    public static bool HasFieldInitializer(this IFieldSymbol symbol)
    {
        return symbol.DeclaringSyntaxReferences
            .Select(syntaxRef => syntaxRef.GetSyntax())
            .OfType<PropertyDeclarationSyntax>()
            .Any(syntax => syntax.Initializer is not null);
    }
}



