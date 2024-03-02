using Microsoft.CodeAnalysis;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Extensions;

public static class IEventSymbolExtensions
{

    public static string GetTypeString(this IEventSymbol symbol)
    {
        return symbol.Type.GetTypeName();   
    }

    public static string GetModifiersString(this IEventSymbol symbol)
    {
        var modifiers = string.Join(" ",
            new[]
            {
                symbol.ContainingType.TypeKind is not TypeKind.Interface
                    && symbol.IsAbstract ? "{abstract}" : "",
                symbol.IsStatic ? "{static}" : "",
                symbol.IsSealed ? "<<sealed>>" : "",
                symbol.IsOverride ? "<<override>>" : "",
                symbol.IsVirtual ? "<<virtual>>" : "",
            }.Where(s => !string.IsNullOrEmpty(s)));
        if (modifiers != string.Empty)
        {
            modifiers += " ";
        }
        return modifiers;
    }
}
