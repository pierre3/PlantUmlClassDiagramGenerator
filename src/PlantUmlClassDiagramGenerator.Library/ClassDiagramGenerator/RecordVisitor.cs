using System.Linq;
using System;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlantUmlClassDiagramGenerator.Library.ClassDiagramGenerator;

public partial class ClassDiagramGenerator
{
    public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
    {
        if (attributeRequired && !node.AttributeLists.HasDiagramAttribute()) { return; }
        if (node.AttributeLists.HasIgnoreAttribute()) { return; }
        if (SkipInnerTypeDeclaration(node)) { return; }

        relationships.AddInnerclassRelationFrom(node);
        relationships.AddInheritanceFrom(node);
        var modifiers = GetTypeModifiersText(node.Modifiers);
        var abstractKeyword = (node.Modifiers.Any(SyntaxKind.AbstractKeyword) ? "abstract " : "");

        var typeName = TypeNameText.From(node);
        var name = typeName.Identifier;
        var typeParam = typeName.TypeArguments;
        var type = $"{name}{typeParam}";
        var typeParams = typeParam.TrimStart('<').TrimEnd('>').Split([','], StringSplitOptions.RemoveEmptyEntries);
        types.Add(name);

        var typeKeyword = (node.Kind() == SyntaxKind.RecordStructDeclaration) ? "struct" : "class";
        WriteLine($"{abstractKeyword}{typeKeyword} {type} {modifiers}<<record>> {{");

        nestingDepth++;
        var parameters = node.ParameterList?.Parameters ?? Enumerable.Empty<ParameterSyntax>();
        foreach (var parameter in parameters)
        {
            VisitRecordParameter(node, type, typeParams, parameter);
        }
        base.VisitRecordDeclaration(node);
        nestingDepth--;

        WriteLine("}");
    }

    private void VisitRecordParameter(RecordDeclarationSyntax node, string type, string[] typeParams, ParameterSyntax parameter)
    {
        var parameterType = parameter.Type;
        var isTypeParameterProp = typeParams.Contains(parameterType.ToString());
        var associationAttrSyntax = parameter.AttributeLists.GetAssociationAttributeSyntax();
        if (associationAttrSyntax is not null)
        {
            var associationAttr = CreateAssociationAttribute(associationAttrSyntax);
            relationships.AddAssociationFrom(node, parameter, associationAttr);
        }
        else if (!createAssociation
                 || parameter.AttributeLists.HasIgnoreAssociationAttribute()
                 || parameterType.GetType() == typeof(PredefinedTypeSyntax)
                 || parameterType.GetType() == typeof(NullableTypeSyntax)
                 || isTypeParameterProp)
        {
            // ParameterList-Property: always public
            var parameterModifiers = "+ ";
            var parameterName = parameter.Identifier.ToString();

            // ParameterList-Property always have get and init accessor
            var accessorStr = "<<get>> <<init>>";

            var useLiteralInit = parameter.Default?.Value is not null;
            var initValue = useLiteralInit
                ? (" = " + escapeDictionary.Aggregate(parameter.Default.Value.ToString(),
                    (n, e) => Regex.Replace(n, e.Key, e.Value)))
                : "";
            WriteLine($"{parameterModifiers}{parameterName} : {parameterType} {accessorStr}{initValue}");
        }
        else
        {
            if (type.GetType() == typeof(GenericNameSyntax))
            {
                additionalTypeDeclarationNodes.Add(parameterType);
            }
            relationships.AddAssociationFrom(parameter, node);
        }
    }
}