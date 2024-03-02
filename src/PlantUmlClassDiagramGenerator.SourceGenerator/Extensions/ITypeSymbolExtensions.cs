using Microsoft.CodeAnalysis;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Extensions;

public static class ITypeSymbolExtensions
{
    public static string GetMetadataName(this ITypeSymbol typeSymbol, string punctuation = "::")
    {
        var parts = typeSymbol.ToDisplayParts(
            new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypes));
        var containingTypes = parts
            .Take(parts.Length -1)
            .Where(p => p.Kind != SymbolDisplayPartKind.Punctuation)
            .Select(p => p.ToString());
        return string.Join(punctuation, [.. containingTypes, typeSymbol.MetadataName]);
    }

    public static string GetTypeName(this ITypeSymbol symbol)
    {
        return symbol.ToDisplayString(
            symbol.NullableAnnotation == NullableAnnotation.Annotated
                ? NullableFlowState.MaybeNull
                : NullableFlowState.None,
            SymbolDisplayFormat.MinimallyQualifiedFormat);
    }

    public static string GetOutputFilePath(this ITypeSymbol symbol, string basePath)
    {
        return Path.Combine([basePath,
            symbol.ContainingAssembly.Name,
            .. symbol.ContainingNamespace.ToString().Replace("<", "").Replace(">", "").Split('.'),
            symbol.GetMetadataName(".") + ".puml"]);
    }
}



