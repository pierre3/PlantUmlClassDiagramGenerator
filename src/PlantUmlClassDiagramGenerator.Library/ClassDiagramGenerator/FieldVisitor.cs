using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlantUmlClassDiagramGenerator.Library.ClassDiagramGenerator;

public partial class ClassDiagramGenerator
{
        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
    {
        if (node.AttributeLists.HasIgnoreAttribute()) { return; }
        if (IsIgnoreMember(node.Modifiers)) { return; }

        var modifiers = GetMemberModifiersText(node.Modifiers,
                isInterfaceMember: node.Parent.IsKind(SyntaxKind.InterfaceDeclaration));
        var type = node.Declaration.Type;
        var variables = node.Declaration.Variables;
        var parentClass = (node.Parent as TypeDeclarationSyntax);
        var isTypeParameterField = parentClass?.TypeParameterList?.Parameters
            .Any(t => t.Identifier.Text == type.ToString()) ?? false;

        foreach (var field in variables)
        {
            Type fieldType = type.GetType();
            var associationAttrSyntax = node.AttributeLists.GetAssociationAttributeSyntax();
            if (associationAttrSyntax is not null)
            {
                var associationAttr = CreateAssociationAttribute(associationAttrSyntax);
                relationships.AddAssociationFrom(node, associationAttr);
            }
            else if (!createAssociation
                || node.AttributeLists.HasIgnoreAssociationAttribute()
                || fieldType == typeof(PredefinedTypeSyntax)
                || fieldType == typeof(NullableTypeSyntax)
                || isTypeParameterField)
            {
                var useLiteralInit = field.Initializer?.Value?.Kind().ToString().EndsWith("LiteralExpression") ?? false;
                var initValue = useLiteralInit
                    ? (" = " + escapeDictionary.Aggregate(field.Initializer.Value.ToString(),
                        (f, e) => Regex.Replace(f, e.Key, e.Value)))
                    : "";
                WriteLine($"{modifiers}{field.Identifier} : {type}{initValue}");
            }
            else
            {
                if (fieldType == typeof(GenericNameSyntax))
                {
                    additionalTypeDeclarationNodes.Add(type);
                }
                relationships.AddAssociationFrom(node, field);
            }
        }
    }
}