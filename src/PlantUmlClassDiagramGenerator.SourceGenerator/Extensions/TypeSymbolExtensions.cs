using Microsoft.CodeAnalysis;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Extensions;

public static class TypeSymbolExtensions
{
    public static string GetTypeName(this ITypeSymbol symbol)
    {
        return symbol.ToDisplayString(
            symbol.NullableAnnotation == NullableAnnotation.Annotated
                ? NullableFlowState.MaybeNull
                : NullableFlowState.None,
            SymbolDisplayFormat.MinimallyQualifiedFormat);
    }
}



