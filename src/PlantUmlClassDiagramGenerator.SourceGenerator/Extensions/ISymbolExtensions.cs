﻿using Microsoft.CodeAnalysis;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Extensions;

public static class ISymbolExtensions
{
    public static readonly string AutoGeneratedNamespace = "PlantUmlClassDiagramGenerator.SourceGenerator.Attributes";
    public static readonly string PlantUmlDiagramAttributeName = $"{AutoGeneratedNamespace}.PlantUmlDiagramAttribute";
    public static readonly string PlantUmlIgnoreAttributeName = $"{AutoGeneratedNamespace}.PlantUmlIgnoreAttribute";

    public static bool HasPlantUmlDiagramAttribute(this ISymbol symbol)
        => symbol.GetAttributes().Any(attr => attr.AttributeClass?.ToString() == PlantUmlDiagramAttributeName);

    public static bool HasPlantUmlIgnoreAttribute(this ISymbol symbol)
        => symbol.GetAttributes().Any(attr => attr.AttributeClass?.ToString() == PlantUmlIgnoreAttributeName);

    public static bool NamespaceStartsWith(this ISymbol symbol, string value)
        => symbol.ContainingNamespace.ToString().StartsWith(value);

    public static object? GetPlantUmlDiagramAttributeArg(this ISymbol symbol, string argName)
    {
        var attribute = symbol.GetAttributes().FirstOrDefault(attr => attr.AttributeClass?.ToString() == PlantUmlDiagramAttributeName);
        return attribute?.NamedArguments.FirstOrDefault(arg => arg.Key == argName).Value.Value;
    }

    public static bool IsAutoGeneratedSymbol(this ISymbol symbol)
    {
        return symbol.ContainingNamespace.ToString() == AutoGeneratedNamespace
            || symbol.ContainingNamespace.ToString().Contains("<");
    }
}
