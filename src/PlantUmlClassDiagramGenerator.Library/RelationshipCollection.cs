using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PlantUmlClassDiagramGenerator.Attributes;

namespace PlantUmlClassDiagramGenerator.Library
{
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
            if (node.Declaration.Type is not SimpleNameSyntax baseNode 
                || node.Parent is not BaseTypeDeclarationSyntax subNode) return;

            var symbol = field.Initializer == null ? "-->" : "o->";
            var fieldIdentifier = field.Identifier.ToString();
            AddRelationship(baseNode, subNode, symbol, fieldIdentifier);
        }

        public void AddAssociationFrom(PropertyDeclarationSyntax node)
        {
            if (node.Type is not SimpleNameSyntax baseNode 
                || node.Parent is not BaseTypeDeclarationSyntax subNode) return;

            var symbol = node.Initializer == null ? "-->" : "o->";
            var nodeIdentifier = node.Identifier.ToString();
            AddRelationship(baseNode, subNode, symbol, nodeIdentifier);
        }

        public void AddAssociationFrom(ParameterSyntax node, RecordDeclarationSyntax parent)
        {
            if (node.Type is not SimpleNameSyntax baseNode 
                || parent is not BaseTypeDeclarationSyntax subNode) return;

            var symbol = node.Default == null ? "-->" : "o->";
            var nodeIdentifier = node.Identifier.ToString();
            AddRelationship(baseNode, subNode, symbol, nodeIdentifier);
        }

        public void AddAssociationFrom(PropertyDeclarationSyntax node, PlantUmlAssociationAttribute attribute)
        {
            if (node.Type is not SimpleNameSyntax baseNode
                || node.Parent is not BaseTypeDeclarationSyntax subNode) return;
            AddeRationship(attribute, baseNode, subNode);

        }

        public void AddAssociationFrom(MethodDeclarationSyntax node, ParameterSyntax parameter, PlantUmlAssociationAttribute attribute)
        {
            if (parameter.Type is not SimpleNameSyntax baseNode
                || node.Parent is not BaseTypeDeclarationSyntax subNode) return;
            AddeRationship(attribute, baseNode, subNode);
        }

        public void AddAssociationFrom(ConstructorDeclarationSyntax node, ParameterSyntax parameter, PlantUmlAssociationAttribute attribute)
        {
            if (parameter.Type is not SimpleNameSyntax baseNode
                || node.Parent is not BaseTypeDeclarationSyntax subNode) return;
            AddeRationship(attribute, baseNode, subNode);
        }

        public void AddAssociationFrom(FieldDeclarationSyntax node, PlantUmlAssociationAttribute attribute)
        {
            if (node.Declaration.Type is not SimpleNameSyntax baseNode 
                || node.Parent is not BaseTypeDeclarationSyntax subNode) return;
            AddeRationship(attribute, baseNode, subNode);
        }

        private void AddeRationship(PlantUmlAssociationAttribute attribute, SimpleNameSyntax baseNode, BaseTypeDeclarationSyntax subNode)
        {
            var symbol = string.IsNullOrEmpty(attribute.Association) ? "--" : attribute.Association;

            var baseName = string.IsNullOrWhiteSpace(attribute.Name)
                ? TypeNameText.From(baseNode)
                : new TypeNameText() { Identifier = attribute.Name };
            var subName = TypeNameText.From(subNode);
            items.Add(new Relationship(subName, baseName, symbol, "", attribute.Multiplicity, attribute.Label));
        }

        private void AddRelationship(SimpleNameSyntax baseNode, BaseTypeDeclarationSyntax subNode, string symbol, string nodeIdentifier)
        {
            var baseName = TypeNameText.From(baseNode);
            var subName = TypeNameText.From(subNode);
            items.Add(new Relationship(subName, baseName, symbol, "", nodeIdentifier + baseName.TypeArguments));
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
}
