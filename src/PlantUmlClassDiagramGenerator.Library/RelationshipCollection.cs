using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                if (!(typeStntax.Type is SimpleNameSyntax typeNameSyntax)) continue;
                var baseTypeName = TypeNameText.From(typeNameSyntax);
                items.Add(new Relationship(baseTypeName, subTypeName, "<|--", baseTypeName.TypeArguments));             
            }
        }

        public void AddInnerclassRelationFrom(SyntaxNode node)
        {
            if (!(node.Parent is BaseTypeDeclarationSyntax outerTypeNode) || !(node is BaseTypeDeclarationSyntax innerTypeNode)) return;

            var outerTypeName = TypeNameText.From(outerTypeNode);
            var innerTypeName = TypeNameText.From(innerTypeNode);
            items.Add(new Relationship(outerTypeName, innerTypeName, "+--"));
        }

        public void AddAssociationFrom(FieldDeclarationSyntax node, VariableDeclaratorSyntax field)
        {
            if (!(node.Declaration.Type is SimpleNameSyntax baseNode) || !(node.Parent is BaseTypeDeclarationSyntax subNode)) return;

            var symbol = field.Initializer == null ? "-->" : "o->";

            var baseName = TypeNameText.From(baseNode);
            var subName = TypeNameText.From(subNode);
            items.Add(new Relationship(subName, baseName, symbol, "", field.Identifier.ToString() + baseName.TypeArguments));
        }

        public void AddAssociationFrom(PropertyDeclarationSyntax node)
        {
            if (!(node.Type is SimpleNameSyntax baseNode) || !(node.Parent is BaseTypeDeclarationSyntax subNode)) return;

            var symbol = node.Initializer == null ? "-->" : "o->";

            var baseName = TypeNameText.From(baseNode);
            var subName = TypeNameText.From(subNode);
            items.Add(new Relationship(subName, baseName, symbol, "", node.Identifier.ToString() + baseName.TypeArguments));
        }

        public void AddAssociationFrom(ParameterSyntax node, RecordDeclarationSyntax parent)
        {
            if (!(node.Type is SimpleNameSyntax baseNode) || !(parent is BaseTypeDeclarationSyntax subNode)) return;

            var symbol = node.Default == null ? "-->" : "o->";

            var baseName = TypeNameText.From(baseNode);
            var subName = TypeNameText.From(subNode);
            items.Add(new Relationship(subName, baseName, symbol, "", node.Identifier.ToString() + baseName.TypeArguments));
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
