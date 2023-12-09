using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PlantUmlClassDiagramGenerator.Attributes;

namespace PlantUmlClassDiagramGenerator.Library;

public class RelationshipCollection : IEnumerable<Relationship>
{
    private readonly IList<Relationship> items = new List<Relationship>();

    public void AddInheritanceFrom(TypeDeclarationSyntax syntax)
    {
        if (syntax.BaseList == null) return;

        var subTypeName = TypeNameText.From(syntax);

        foreach (var typeStntax in syntax.BaseList.Types)
        {
            if (typeStntax.Type is not SimpleNameSyntax typeNameSyntax) continue;
            var baseTypeName = TypeNameText.From(typeNameSyntax);
            items.Add(new Relationship(baseTypeName, subTypeName, "<|--", baseTypeName.TypeArguments));
        }
    }

    public void AddInnerclassRelationFrom(SyntaxNode node)
    {
        if (node.Parent is not BaseTypeDeclarationSyntax outerTypeNode 
            || node is not BaseTypeDeclarationSyntax innerTypeNode) return;

        var outerTypeName = TypeNameText.From(outerTypeNode);
        var innerTypeName = TypeNameText.From(innerTypeNode);
        items.Add(new Relationship(outerTypeName, innerTypeName, "+--"));
    }

    public void AddAssociationFrom(FieldDeclarationSyntax node, VariableDeclaratorSyntax field)
    {
        // Just ignore the nullability of the node
        var realDeclarationType = node.Declaration.Type is NullableTypeSyntax nullableTypeSyntax ? nullableTypeSyntax.ElementType : node.Declaration.Type;

        if ((realDeclarationType is not IdentifierNameSyntax && realDeclarationType is not GenericNameSyntax && realDeclarationType is not ArrayTypeSyntax)
            || node.Parent is not BaseTypeDeclarationSyntax rootNode) return;

        TypeNameText typeNameText = null;

        if(realDeclarationType is IdentifierNameSyntax identifierNameSyntax)
        {
            typeNameText = TypeNameText.From(identifierNameSyntax as SimpleNameSyntax); 
        }

        if(realDeclarationType is GenericNameSyntax genericNameSyntax)
        {
            var childNode = genericNameSyntax.TypeArgumentList.ChildNodes().FirstOrDefault();
            if(childNode is SimpleNameSyntax simpleNameSyntax)
            {
                typeNameText = TypeNameText.From(simpleNameSyntax);
            }

            if(childNode is PredefinedTypeSyntax predefinedTypeSyntax)
            {
                typeNameText = TypeNameText.From(predefinedTypeSyntax);
            }
        }

        if(realDeclarationType is ArrayTypeSyntax arrayTypeSyntax)
        {
            typeNameText = TypeNameText.From(arrayTypeSyntax.ElementType as SimpleNameSyntax);
        }

        var symbol = field.Initializer == null ? "-->" : "o->";
        var fieldIdentifier = field.Identifier.ToString();
        var leafName = typeNameText;
        var rootName = TypeNameText.From(rootNode);
        AddRelationship(leafName, rootName, symbol, fieldIdentifier);
    }

    public void AddAssociationFrom(PropertyDeclarationSyntax node)
    {
        if (node.Type is not SimpleNameSyntax leafNode 
            || node.Parent is not BaseTypeDeclarationSyntax rootNode) return;

        var symbol = node.Initializer == null ? "-->" : "o->";
        var nodeIdentifier = node.Identifier.ToString();
        var leafName = TypeNameText.From(leafNode);
        var rootName = TypeNameText.From(rootNode);
        AddRelationship(leafName, rootName, symbol, nodeIdentifier);
    }

    public void AddAssociationFrom(ParameterSyntax node, RecordDeclarationSyntax parent)
    {
        if (node.Type is not SimpleNameSyntax leafNode 
            || parent is not BaseTypeDeclarationSyntax rootNode) return;

        var symbol = node.Default == null ? "-->" : "o->";
        var nodeIdentifier = node.Identifier.ToString();
        var leafName = TypeNameText.From(leafNode);
        var rootName = TypeNameText.From(rootNode);
        AddRelationship(leafName, rootName, symbol, nodeIdentifier);
    }

    public void AddAssociationFrom(PropertyDeclarationSyntax node, PlantUmlAssociationAttribute attribute)
    {
        if (node.Parent is not BaseTypeDeclarationSyntax rootNode) return;
        var leafName = GetLeafName(attribute.Name, node.Type);
        if (leafName is null) { return; }
        var rootName = TypeNameText.From(rootNode);
        AddeRationship(attribute, leafName, rootName);

    }

    public void AddAssociationFrom(MethodDeclarationSyntax node, ParameterSyntax parameter, PlantUmlAssociationAttribute attribute)
    {
        if (node.Parent is not BaseTypeDeclarationSyntax rootNode) return;
        var leafName = GetLeafName(attribute.Name, parameter.Type);
        if (leafName is null) { return; }
        var rootName = TypeNameText.From(rootNode);
        AddeRationship(attribute, leafName, rootName);
    }

    public void AddAssociationFrom(RecordDeclarationSyntax node, ParameterSyntax parameter, PlantUmlAssociationAttribute attribute)
    {
        if (node is not BaseTypeDeclarationSyntax rootNode) { return; }
        var leafName = GetLeafName(attribute.Name, parameter.Type);
        if (leafName is null) { return; }
        var rootName = TypeNameText.From(rootNode);
        AddeRationship(attribute, leafName, rootName);
    }

    public void AddAssociationFrom(ConstructorDeclarationSyntax node, ParameterSyntax parameter, PlantUmlAssociationAttribute attribute)
    {
        if (node.Parent is not BaseTypeDeclarationSyntax rootNode) { return; }
        var leafName = GetLeafName(attribute.Name, parameter.Type);
        if (leafName is null) { return; }
        var rootName = TypeNameText.From(rootNode);
        AddeRationship(attribute, leafName, rootName);
    }

    public void AddAssociationFrom(FieldDeclarationSyntax node, PlantUmlAssociationAttribute attribute)
    {
        if (node.Parent is not BaseTypeDeclarationSyntax rootNode) { return; }
        var leafName = GetLeafName(attribute.Name, node.Declaration.Type);
        if(leafName is null) { return; }
        var rootName = TypeNameText.From(rootNode);
        AddeRationship(attribute, leafName, rootName);
    }

    private static TypeNameText GetLeafName(string attributeName, TypeSyntax typeSyntax)
    {
        if (!string.IsNullOrWhiteSpace(attributeName))
        {
            return new TypeNameText() { Identifier = attributeName };
        }
        else if (typeSyntax is SimpleNameSyntax simpleNode)
        {
            return TypeNameText.From(simpleNode);
        }
        return null;
        
    }

    private void AddeRationship(PlantUmlAssociationAttribute attribute, TypeNameText leafName, TypeNameText rootName)
    {
        var symbol = string.IsNullOrEmpty(attribute.Association) ? "--" : attribute.Association;
        items.Add(new Relationship(rootName, leafName, symbol, attribute.RootLabel, attribute.LeafLabel, attribute.Label));
    }

    private void AddRelationship(TypeNameText leafName, TypeNameText rootName, string symbol, string nodeIdentifier)
    {
        items.Add(new Relationship(rootName, leafName, symbol, "", nodeIdentifier + leafName.TypeArguments));
    }

    public IEnumerator<Relationship> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
