using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PlantUmlClassDiagramGenerator.Attributes;
using PlantUmlClassDiagramGenerator.Library.Enums;

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
                FillAssociatedField(field, modifiers, type);
            }
            else
            {
                if (type.GetType() == typeof(GenericNameSyntax))
                    ProcessGenericType(node, type, field, modifiers);
                else
                    relationships.AddAssociationFrom(node, field);
            }
        }
    }

    private void FillAssociatedField(VariableDeclaratorSyntax field, string modifiers, TypeSyntax type)
    {
        var useLiteralInit = field.Initializer?.Value?.Kind().ToString().EndsWith("LiteralExpression") ?? false;
        var initValue = useLiteralInit
            ? (" = " + escapeDictionary.Aggregate(field.Initializer.Value.ToString(),
                (f, e) => Regex.Replace(f, e.Key, e.Value)))
            : "";
        WriteLine($"{modifiers}{field.Identifier} : {type}{initValue}");
    }
    
    private void ProcessGenericType(FieldDeclarationSyntax node, TypeSyntax type, VariableDeclaratorSyntax field, string modifiers)
    {
        if (this.removeSystemCollectionsAssociations)
        {
            ProcessWithoutSystemCollections(node, type, field, modifiers);
        }
        else
        {
            additionalTypeDeclarationNodes.Add(type);
            relationships.AddAssociationFrom(node, field);
        }
    }

    private void ProcessWithoutSystemCollections(FieldDeclarationSyntax node, TypeSyntax type, VariableDeclaratorSyntax field, string modifiers)
    {
        var t = type.ToString().Split('<')[0];
        if (!Enum.TryParse(t, out SystemCollectionsTypes _))
        {
            additionalTypeDeclarationNodes.Add(type);
            relationships.AddAssociationFrom(node, field);
        }
        else
        {
            FillAssociatedField(field, modifiers, type);
            var s = type.ToString().Split('<')[1];
            s = s.Remove(s.Length - 1);
            if (!Enum.TryParse(CapitalizeFirstLetter(s), out BaseTypes _))
                relationships.AddAssociationFrom(node, new PlantUmlAssociationAttribute()
                {
                    Association = "o--",
                    Name = s
                });
        }
    }
}