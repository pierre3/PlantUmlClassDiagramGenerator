﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PlantUmlClassDiagramGenerator.Attributes;

namespace PlantUmlClassDiagramGenerator.Library;

public class RelationshipCollection : IEnumerable<Relationship>
{
    private readonly IList<Relationship> items = new List<Relationship>();

    public void AddAll(RelationshipCollection collection)
    {
        foreach (var c in collection)
        {
            items.Add(c);
        }
    }

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
        if (node.Declaration.Type is not SimpleNameSyntax leafNode 
            || node.Parent is not BaseTypeDeclarationSyntax rootNode) return;

        var symbol = field.Initializer == null ? "-->" : "o->";
        var fieldIdentifier = field.Identifier.ToString();
        var leafName = TypeNameText.From(leafNode);
        var rootName = TypeNameText.From(rootNode);
        AddRelationship(leafName, rootName, symbol, fieldIdentifier);
    }
    
    public void AddAssociationFromWithNoLabel(FieldDeclarationSyntax node, VariableDeclaratorSyntax field)
    {
        if (node.Declaration.Type is not SimpleNameSyntax leafNode 
            || node.Parent is not BaseTypeDeclarationSyntax rootNode) return;

        var symbol = field.Initializer == null ? "-->" : "o->";
        var leafName = TypeNameText.From(leafNode);
        var rootName = TypeNameText.From(rootNode);
        AddRelationship(leafName, rootName, symbol, "");
    }

    public void AddAssociationFrom(PropertyDeclarationSyntax node, TypeSyntax typeIgnoringNullable)
    {
        if (typeIgnoringNullable is not SimpleNameSyntax leafNode 
            || node.Parent is not BaseTypeDeclarationSyntax rootNode) return;

        var symbol = node.Initializer == null ? "-->" : "o->";
        var nodeIdentifier = node.Identifier.ToString();
        var leafName = TypeNameText.From(leafNode);
        var rootName = TypeNameText.From(rootNode);
        AddRelationship(leafName, rootName, symbol, nodeIdentifier);
    }
    
    public void AddAssociationFromWithNoLabel(PropertyDeclarationSyntax node, TypeSyntax typeIgnoringNullable)
    {
        if (typeIgnoringNullable is not SimpleNameSyntax leafNode 
            || node.Parent is not BaseTypeDeclarationSyntax rootNode) return;

        var symbol = node.Initializer == null ? "-->" : "o->";
        var leafName = TypeNameText.From(leafNode);
        var rootName = TypeNameText.From(rootNode);
        AddRelationship(leafName, rootName, symbol, "");
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
        AddRelationship(attribute, leafName, rootName);

    }

    public void AddAssociationFrom(MethodDeclarationSyntax node, ParameterSyntax parameter, PlantUmlAssociationAttribute attribute)
    {
        if (node.Parent is not BaseTypeDeclarationSyntax rootNode) return;
        var leafName = GetLeafName(attribute.Name, parameter.Type);
        if (leafName is null) { return; }
        var rootName = TypeNameText.From(rootNode);
        AddRelationship(attribute, leafName, rootName);
    }

    public void AddAssociationFrom(RecordDeclarationSyntax node, ParameterSyntax parameter, PlantUmlAssociationAttribute attribute)
    {
        if (node is not BaseTypeDeclarationSyntax rootNode) { return; }
        var leafName = GetLeafName(attribute.Name, parameter.Type);
        if (leafName is null) { return; }
        var rootName = TypeNameText.From(rootNode);
        AddRelationship(attribute, leafName, rootName);
    }

    public void AddAssociationFrom(ConstructorDeclarationSyntax node, ParameterSyntax parameter, PlantUmlAssociationAttribute attribute)
    {
        if (node.Parent is not BaseTypeDeclarationSyntax rootNode) { return; }
        var leafName = GetLeafName(attribute.Name, parameter.Type);
        if (leafName is null) { return; }
        var rootName = TypeNameText.From(rootNode);
        AddRelationship(attribute, leafName, rootName);
    }

    public void AddAssociationFrom(FieldDeclarationSyntax node, PlantUmlAssociationAttribute attribute)
    {
        if (node.Parent is not BaseTypeDeclarationSyntax rootNode) { return; }
        var leafName = GetLeafName(attribute.Name, node.Declaration.Type);
        if(leafName is null) { return; }
        var rootName = TypeNameText.From(rootNode);
        AddRelationship(attribute, leafName, rootName);
    }

    private static TypeNameText GetLeafName(string attributeName, TypeSyntax typeSyntax)
    {
        if (!string.IsNullOrWhiteSpace(attributeName))
        {
            return new TypeNameText() { Identifier = attributeName, TypeArguments = ""};
        }
        else if (typeSyntax is SimpleNameSyntax simpleNode)
        {
            return TypeNameText.From(simpleNode);
        }
        return null;
        
    }

    private void AddRelationship(PlantUmlAssociationAttribute attribute, TypeNameText leafName, TypeNameText rootName)
    {
        var symbol = string.IsNullOrEmpty(attribute.Association) ? "--" : attribute.Association;
        var relationship = new Relationship(rootName, leafName, symbol, attribute.RootLabel, attribute.LeafLabel, attribute.Label);
        if (!items.Contains(relationship))
            items.Add(relationship);
    }

    private void AddRelationship(TypeNameText leafName, TypeNameText rootName, string symbol, string nodeIdentifier)
    {
        var relationship = new Relationship(rootName, leafName, symbol, "", nodeIdentifier + leafName.TypeArguments);
        if (!items.Contains(relationship))
            items.Add(relationship);
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
