using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PlantUmlClassDiagramGenerator.Attributes;

namespace PlantUmlClassDiagramGenerator.Library.ClassDiagramGenerator;

public partial class ClassDiagramGenerator(
    TextWriter writer,
    string indent,
    Accessibilities ignoreMemberAccessibilities = Accessibilities.None,
    bool createAssociation = true,
    bool attributeRequired = false,
    bool excludeUmlBeginEndTags = false,
    bool addPackageTags = false,
    bool removeSystemCollectionsAssociations = false,
    bool noGetSetForProperties = false,
    bool saveFields = false) : CSharpSyntaxWalker
{
    private readonly HashSet<string> types = [];
    private readonly List<SyntaxNode> additionalTypeDeclarationNodes = [];
    private readonly Accessibilities ignoreMemberAccessibilities = ignoreMemberAccessibilities;
    public readonly RelationshipCollection relationships = new();
    private readonly TextWriter writer = writer;
    private readonly string indent = indent;
    private int nestingDepth = 0;
    private readonly bool createAssociation = createAssociation;
    private readonly bool attributeRequired = attributeRequired;
    private readonly bool excludeUmlBeginEndTags = excludeUmlBeginEndTags;
    private readonly bool addPackageTags = addPackageTags;
    private readonly bool removeSystemCollectionsAssociations = removeSystemCollectionsAssociations;
    private readonly bool noGetSetForProperties = noGetSetForProperties;
    private readonly bool saveFields = saveFields;
    private readonly Dictionary<string, string> escapeDictionary = new()
    {
        {@"(?<before>[^{]){(?<after>{[^{])", "${before}&#123;${after}"},
        {@"(?<before>[^}])}(?<after>[^}])", "${before}&#125;${after}"},
    };

    public void Generate(SyntaxNode root)
    {
        if (!this.excludeUmlBeginEndTags) WriteLine("@startuml");
        GenerateInternal(root);
        if (!this.excludeUmlBeginEndTags) WriteLine("@enduml");
    }

    public void GenerateInternal(SyntaxNode root)
    {
        Visit(root);
        GenerateAdditionalTypeDeclarations();
        if (!this.addPackageTags)
            GenerateRelationships();
    }

    private static PlantUmlAssociationAttribute CreateAssociationAttribute(AttributeSyntax associationAttribute)
    {
        var attributeProps = associationAttribute.ArgumentList.Arguments.Select(arg => new
        {
            Name = arg.NameEquals.Name.ToString(),
            Value = arg.Expression.GetLastToken().ValueText
        }).ToList();
        return new PlantUmlAssociationAttribute()
        {
            Association = attributeProps.FirstOrDefault(prop => prop.Name == nameof(PlantUmlAssociationAttribute.Association))?.Value,
            Name = attributeProps.FirstOrDefault(prop => prop.Name == nameof(PlantUmlAssociationAttribute.Name))?.Value,
            RootLabel = attributeProps.FirstOrDefault(prop => prop.Name == nameof(PlantUmlAssociationAttribute.RootLabel))?.Value,
            LeafLabel = attributeProps.FirstOrDefault(prop => prop.Name == nameof(PlantUmlAssociationAttribute.LeafLabel))?.Value,
            Label = attributeProps.FirstOrDefault(prop => prop.Name == nameof(PlantUmlAssociationAttribute.Label))?.Value
        };
    }



    public override void VisitGenericName(GenericNameSyntax node)
    {
        if (createAssociation)
        {
            additionalTypeDeclarationNodes.Add(node);
        }
    }

    private void WriteLine(string line)
    {
        var space = string.Concat(Enumerable.Repeat(indent, nestingDepth));
        writer.WriteLine(space + line);
    }

    private bool SkipInnerTypeDeclaration(SyntaxNode node)
    {
        if (nestingDepth <= 0) return false;
        if (nestingDepth == 1 && addPackageTags) return false;
        additionalTypeDeclarationNodes.Add(node);
        return true;
    }

    private static string GetTypeModifiersText(SyntaxTokenList modifiers)
    {
        var tokens = modifiers.Select(token =>
        {
            switch (token.Kind())
            {
                case SyntaxKind.PublicKeyword:
                case SyntaxKind.PrivateKeyword:
                case SyntaxKind.ProtectedKeyword:
                case SyntaxKind.InternalKeyword:
                case SyntaxKind.AbstractKeyword:
                    return "";
                default:
                    return $"<<{token.ValueText}>>";
            }
        }).Where(token => token != "");

        var result = string.Join(" ", tokens);
        if (result != string.Empty)
        {
            result += " ";
        };
        return result;
    }

    private bool IsIgnoreMember(SyntaxTokenList modifiers)
    {
        if (ignoreMemberAccessibilities == Accessibilities.None) { return false; }

        var tokenKinds = HasAccessModifier(modifiers)
            ? modifiers.Select(x => x.Kind()).ToArray()
            : [SyntaxKind.PrivateKeyword];

        if (ignoreMemberAccessibilities.HasFlag(Accessibilities.ProtectedInternal)
            && tokenKinds.Contains(SyntaxKind.ProtectedKeyword)
            && tokenKinds.Contains(SyntaxKind.InternalKeyword))
        {
            return true;
        }

        if (ignoreMemberAccessibilities.HasFlag(Accessibilities.Public)
            && tokenKinds.Contains(SyntaxKind.PublicKeyword))
        {
            return true;
        }

        if (ignoreMemberAccessibilities.HasFlag(Accessibilities.Protected)
            && tokenKinds.Contains(SyntaxKind.ProtectedKeyword))
        {
            return true;
        }

        if (ignoreMemberAccessibilities.HasFlag(Accessibilities.Internal)
            && tokenKinds.Contains(SyntaxKind.InternalKeyword))
        {
            return true;
        }

        if (ignoreMemberAccessibilities.HasFlag(Accessibilities.Private)
            && tokenKinds.Contains(SyntaxKind.PrivateKeyword))
        {
            return true;
        }
        return false;
    }

    private static string GetMemberModifiersText(
        SyntaxTokenList modifiers,
        bool isInterfaceMember)
    {
        var tokens = modifiers.Select(token =>
        {
            return token.Kind() switch
            {
                SyntaxKind.PublicKeyword => "+",
                SyntaxKind.PrivateKeyword => "-",
                SyntaxKind.ProtectedKeyword => "#",
                SyntaxKind.AbstractKeyword or SyntaxKind.StaticKeyword => $"{{{token.ValueText}}}",
                _ => $"<<{token.ValueText}>>",
            };
        }).ToList();
        if (!isInterfaceMember && !HasAccessModifier(modifiers))
        {
            tokens.Add("-");
        }
        var result = string.Join(" ", tokens);
        if (result != string.Empty)
        {
            result += " ";
        };
        return result;
    }

    private static bool HasAccessModifier(SyntaxTokenList modifiers)
    {
        return modifiers.Any(token =>
            token.IsKind(SyntaxKind.PublicKeyword)
            || token.IsKind(SyntaxKind.PrivateKeyword)
            || token.IsKind(SyntaxKind.ProtectedKeyword)
            || token.IsKind(SyntaxKind.InternalKeyword));
    }

    private static string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        if (input.Length == 1)
            return char.ToUpper(input[0]) + "";

        return char.ToUpper(input[0]) + input.Substring(1);
    }
}
