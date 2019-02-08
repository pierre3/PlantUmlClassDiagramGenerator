using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlantUmlClassDiagramGenerator.Library
{
    public class RelationshipCollection : IEnumerable<Relationship>
    {
        private IList<Relationship> _items = new List<Relationship>();

        public void AddInheritanceFrom(TypeDeclarationSyntax syntax)
        {
            if (syntax.BaseList == null) { return; }

            var subTypeName = TypeNameText.From(syntax);

            foreach (var typeStntax in syntax.BaseList.Types)
            {
                var typeNameSyntax = typeStntax.Type as SimpleNameSyntax;
                if (typeNameSyntax == null)
                {
                    continue;
                }
                var baseTypeName = TypeNameText.From(typeNameSyntax);
                _items.Add(new InheritanceRelationship(baseTypeName, subTypeName));
            }

        }

        public IEnumerator<Relationship> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
