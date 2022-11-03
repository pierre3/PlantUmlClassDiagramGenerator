using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PlantUmlClassDiagramGenerator.Attributes;
using System.Linq;

namespace PlantUmlClassDiagramGenerator.Library
{
    static class AttributeListSyntaxListExtensions
    {
        public static bool HasIgnoreAttribute(this SyntaxList<AttributeListSyntax> attributeLists)
        {
            return GetIgnoreAttribute(attributeLists) is not null;
        }

        public static AttributeSyntax GetIgnoreAttribute(this SyntaxList<AttributeListSyntax> attributeLists)
        {
            return attributeLists.SelectMany(list => list.Attributes)
                .FirstOrDefault(
                      attr => attr.Name.ToString() == nameof(PlantUmlIgnoreAttribute)
                        || attr.Name.ToString() == nameof(PlantUmlIgnoreAttribute).Replace("Attribute", ""));
        }

        public static bool HasIgnoreAssociationAttribute(this SyntaxList<AttributeListSyntax> attributeLists)
        {
            return GetIgnoreAssociationAttribute(attributeLists) is not null;
        }

        public static AttributeSyntax GetIgnoreAssociationAttribute(this SyntaxList<AttributeListSyntax> attributeLists)
        {
            return attributeLists.SelectMany(list => list.Attributes)
                .FirstOrDefault(
                      attr => attr.Name.ToString() == nameof(PlantUmlIgnoreAssociationAttribute)
                        || attr.Name.ToString() == nameof(PlantUmlIgnoreAssociationAttribute).Replace("Attribute", ""));
        }

        public static bool HasAssociationAttribute(this SyntaxList<AttributeListSyntax> attributeLists)
        {
            return GetAssociationAttributeSyntax(attributeLists) is not null;

        }

        public static AttributeSyntax GetAssociationAttributeSyntax(this SyntaxList<AttributeListSyntax> attributeLists)
        {
            return attributeLists.SelectMany(list => list.Attributes)
                .FirstOrDefault(
                      attr => attr.Name.ToString() == nameof(PlantUmlAssociationAttribute)
                        || attr.Name.ToString() == nameof(PlantUmlAssociationAttribute).Replace("Attribute", ""));
        }

        public static bool HasDiagramAttribute(this SyntaxList<AttributeListSyntax> attributeLists)
        {
            return GetDiagramAttributeSyntax(attributeLists) is not null;

        }

        public static AttributeSyntax GetDiagramAttributeSyntax(this SyntaxList<AttributeListSyntax> attributeLists)
        {
            return attributeLists.SelectMany(list => list.Attributes)
                .FirstOrDefault(
                      attr => attr.Name.ToString() == nameof(PlantUmlDiagramAttribute)
                        || attr.Name.ToString() == nameof(PlantUmlDiagramAttribute).Replace("Attribute", ""));
        }
    }
}
