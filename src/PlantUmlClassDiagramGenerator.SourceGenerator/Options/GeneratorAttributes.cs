﻿using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Options;

internal static class GeneratorAttributes
{
    private static readonly string AutoGeneratedNamespace = "PlantUmlClassDiagramGenerator.SourceGenerator.Attributes";
    private static readonly string PlantUmlDiagramAttributeName = $"{AutoGeneratedNamespace}.PlantUmlDiagramAttribute";
    private static readonly string PlantUmlIgnoreAttributeName = $"{AutoGeneratedNamespace}.PlantUmlIgnoreAttribute";
    private static readonly string PlantUmlAssociationAttributeName = $"{AutoGeneratedNamespace}.PlantUmlAssociationAttribute";
    private static readonly string PlantUmlIgnoreAssociationAttributeName = $"{AutoGeneratedNamespace}.PlantUmlIgnoreAssociationAttribute";
    private static readonly string PlantUmlExtraAssociationTargetsAttributeName = $"{AutoGeneratedNamespace}.PlantUmlExtraAssociationTargetsAttribute";

    public static IImmutableSet<INamedTypeSymbol> GetExtraAssociationTargets(this INamedTypeSymbol symbol)
    {
        return AssemblyAttribute.GetExtraAssociationTargets(symbol)
            .Concat(GetExtraAssociationTargetsFor(symbol))
            .ToImmutableHashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
    }

    public static bool HasPlantUmlDiagramAttribute(this INamedTypeSymbol symbol)
    {
        return AssemblyAttribute.HasPlantUmlDiagramAttribute(symbol)
            || HasPlantUmlDiagramAttributeFor(symbol);
    }

    public static bool HasPlantUmlIgnoreAttribute(this ISymbol symbol)
        => symbol.GetAttributes().Any(attr => attr.AttributeClass?.ToString() == PlantUmlIgnoreAttributeName);

    public static bool HasPlantUmlAssociationAttribute(this ISymbol symbol)
        => symbol.GetAttributes().Any(attr => attr.AttributeClass?.ToString() == PlantUmlAssociationAttributeName);

    public static bool HasPlantUmlIgnoreAssociationAttribute(this ISymbol symbol)
        => symbol.GetAttributes().Any(attr => attr.AttributeClass?.ToString() == PlantUmlIgnoreAssociationAttributeName);

    public static object? GetPlantUmlDiagramAttributeArg(this ISymbol symbol, string argName)
    {
        var attribute = symbol.GetAttributes().FirstOrDefault(attr => attr.AttributeClass?.ToString() == PlantUmlDiagramAttributeName);
        return attribute?.NamedArguments.FirstOrDefault(arg => arg.Key == argName).Value.Value;
    }

    public static object? GetPlantUmlAssociationAttributeArg(this ISymbol symbol, string argName)
    {
        var attribute = symbol.GetAttributes().FirstOrDefault(attr => attr.AttributeClass?.ToString() == PlantUmlAssociationAttributeName);
        return attribute?.NamedArguments.FirstOrDefault(arg => arg.Key == argName).Value.Value;
    }

    public static object? GetPlantUmlAssociationAttributeArg(this ISymbol symbol, int index)
    {
        var attribute = symbol.GetAttributes().FirstOrDefault(attr => attr.AttributeClass?.ToString() == PlantUmlAssociationAttributeName);
        if (attribute?.ConstructorArguments.Length <= index)
        {
            return null;
        }
        return attribute?.ConstructorArguments[index].Value;
    }

    public static bool IsAutoGeneratedSymbol(this ISymbol symbol)
    {
        return symbol.ContainingNamespace.ToString() == AutoGeneratedNamespace
            || symbol.ContainingNamespace.ToString().Contains("<");
    }

    public static bool DeclaredTypeFilter(INamedTypeSymbol symbol)
    {
        return !symbol.IsAutoGeneratedSymbol()
            && !symbol.HasPlantUmlIgnoreAttribute()
            && symbol.HasPlantUmlDiagramAttribute();
    }

    public static bool MemberTypeFilter(ISymbol memberSymbol, INamedTypeSymbol typeSymbol)
    {
        var i = (Accessibilities)(typeSymbol.GetPlantUmlDiagramAttributeArg("IncludeMemberAccessibilities") ?? Accessibilities.NotSet);
        var e = (Accessibilities)(typeSymbol.GetPlantUmlDiagramAttributeArg("ExcludeMemberAccessibilities") ?? Accessibilities.NotSet);
        var includes = i.HasFlag(Accessibilities.NotSet)
            ? AssemblyAttribute.GetIncludeMemberAttributes(typeSymbol.ContainingAssembly)
            : i;
        var excludes = e.HasFlag(Accessibilities.NotSet)
            ? AssemblyAttribute.GetExcludeMemberAttributes(typeSymbol.ContainingAssembly)
            : e;
        return !memberSymbol.HasPlantUmlIgnoreAttribute()
            && HasAccessibility(memberSymbol, includes, excludes);
    }

    private static ImmutableArray<INamedTypeSymbol> GetExtraAssociationTargetsFor(ISymbol symbol)
    {
        var attribute = symbol.GetAttributes()
             .FirstOrDefault(attr => attr.AttributeClass?.ToString() == PlantUmlExtraAssociationTargetsAttributeName);
        if (attribute == null)
        {
            return Enumerable.Empty<INamedTypeSymbol>()
                .ToImmutableArray();
        }
        return attribute.ConstructorArguments
            .SelectMany(arg => arg.Values.Select(arg => arg.Value))
            .OfType<INamedTypeSymbol>()
            .Select(symbol => symbol.IsGenericType ? symbol.OriginalDefinition : symbol)
            .ToImmutableArray();
    }

    private static bool HasPlantUmlDiagramAttributeFor(ISymbol symbol)
        => symbol.GetAttributes().Any(attr => attr.AttributeClass?.ToString() == PlantUmlDiagramAttributeName);

    private static bool HasAccessibility(ISymbol symbol, Accessibilities includes, Accessibilities excludes)
    {
        return symbol.DeclaredAccessibility switch
        {
            Accessibility.Public
                => includes.HasFlag(Accessibilities.Public)
                    && !excludes.HasFlag(Accessibilities.Public),
            Accessibility.Protected
                => includes.HasFlag(Accessibilities.Protected)
                    && !excludes.HasFlag(Accessibilities.Protected),
            Accessibility.Internal or Accessibility.Friend
                => includes.HasFlag(Accessibilities.Internal)
                    && !excludes.HasFlag(Accessibilities.Internal),
            Accessibility.ProtectedOrInternal or Accessibility.ProtectedOrFriend
                => includes.HasFlag(Accessibilities.ProtectedInternal)
                    && !excludes.HasFlag(Accessibilities.ProtectedInternal),
            Accessibility.ProtectedAndInternal or Accessibility.ProtectedAndFriend
                => includes.HasFlag(Accessibilities.PrivateProtected)
                    && !excludes.HasFlag(Accessibilities.PrivateProtected),
            Accessibility.Private
                => includes.HasFlag(Accessibilities.Private)
                    && !excludes.HasFlag(Accessibilities.Private),
            _ => false
        };
    }

    public static AssociationTypes DisableAssociationTypes(this INamedTypeSymbol symbol)
    {
        var t = (AssociationTypes)(symbol.GetPlantUmlDiagramAttributeArg("DisableAssociationTypes") ?? AssociationTypes.NotSet);
        return t.HasFlag(AssociationTypes.NotSet)
            ? AssemblyAttribute.GetDisableAssociationTypesAttributes(symbol.ContainingAssembly)
            : t;
    }


    private static class AssemblyAttribute
    {
        private static readonly IDictionary<IAssemblySymbol, ImmutableArray<INamedTypeSymbol>> extraAssociationTargets
            = new Dictionary<IAssemblySymbol, ImmutableArray<INamedTypeSymbol>>(SymbolEqualityComparer.Default);

        private static readonly IDictionary<IAssemblySymbol, bool> hasPlantUmlDiagramAttribute
            = new Dictionary<IAssemblySymbol, bool>(SymbolEqualityComparer.Default);

        private static readonly IDictionary<IAssemblySymbol, Accessibilities> includeMemberAccessibilities
            = new Dictionary<IAssemblySymbol, Accessibilities>(SymbolEqualityComparer.Default);

        private static readonly IDictionary<IAssemblySymbol, Accessibilities> excludeMemberAccessibilities
                = new Dictionary<IAssemblySymbol, Accessibilities>(SymbolEqualityComparer.Default);

        private static readonly IDictionary<IAssemblySymbol, AssociationTypes> disableAssociationTypes
                = new Dictionary<IAssemblySymbol, AssociationTypes>(SymbolEqualityComparer.Default);

        public static ImmutableArray<INamedTypeSymbol> GetExtraAssociationTargets(INamedTypeSymbol symbol)
        {
            if (extraAssociationTargets.TryGetValue(symbol.ContainingAssembly, out var value))
            {
                return value;
            }
            else
            {
                var targets = GetExtraAssociationTargetsFor(symbol.ContainingAssembly);
                extraAssociationTargets.Add(
                    symbol.ContainingAssembly,
                    targets);
                return targets;
            }
        }

        public static bool HasPlantUmlDiagramAttribute(INamedTypeSymbol symbol)
        {
            if (hasPlantUmlDiagramAttribute.TryGetValue(symbol.ContainingAssembly, out var value))
            {
                return value;
            }
            else
            {
                var hasAttribute = HasPlantUmlDiagramAttributeFor(symbol.ContainingAssembly);
                hasPlantUmlDiagramAttribute.Add(
                    symbol.ContainingAssembly,
                    hasAttribute);
                return hasAttribute;
            }
        }

        public static Accessibilities GetIncludeMemberAttributes(IAssemblySymbol assemblySymbol)
        {
            if (includeMemberAccessibilities.TryGetValue(assemblySymbol, out var value))
            {
                return value;
            }
            else
            {
                var accessibilities = (Accessibilities)(assemblySymbol.GetPlantUmlDiagramAttributeArg("IncludeMemberAccessibilities")
                    ?? Accessibilities.NotSet);
                if (accessibilities.HasFlag(Accessibilities.NotSet))
                {
                    accessibilities = Accessibilities.All;
                }
                includeMemberAccessibilities.Add(
                    assemblySymbol,
                    accessibilities);
                return accessibilities;
            }
        }

        public static Accessibilities GetExcludeMemberAttributes(IAssemblySymbol assemblySymbol)
        {
            if (excludeMemberAccessibilities.TryGetValue(assemblySymbol, out var value))
            {
                return value;
            }
            else
            {
                var accessibilities = (Accessibilities)(assemblySymbol.GetPlantUmlDiagramAttributeArg("ExcludeMemberAccessibilities")
                    ?? Accessibilities.NotSet);
                if (accessibilities.HasFlag(Accessibilities.NotSet))
                {
                    accessibilities = Accessibilities.None;
                }
                excludeMemberAccessibilities.Add(
                    assemblySymbol,
                    accessibilities);
                return accessibilities;
            }
        }
        public static AssociationTypes GetDisableAssociationTypesAttributes(IAssemblySymbol assemblySymbol)
        {
            if (disableAssociationTypes.TryGetValue(assemblySymbol, out var value))
            {
                return value;
            }
            else
            {
                var associationTypes = (AssociationTypes)(assemblySymbol.GetPlantUmlDiagramAttributeArg("DisableAssociationTypes") 
                    ?? AssociationTypes.NotSet);
                if (associationTypes.HasFlag(AssociationTypes.NotSet))
                {
                    associationTypes = AssociationTypes.None;
                }
                disableAssociationTypes.Add(
                    assemblySymbol,
                    associationTypes);
                return associationTypes;
            }
        }
    }
}