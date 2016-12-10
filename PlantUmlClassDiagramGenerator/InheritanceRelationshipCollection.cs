using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlantUmlClassDiagramGenerator
{
    public class InheritanceRelationshipCollection : IEnumerable<InheritanceRelationsip>
    {
        private IList<InheritanceRelationsip> _items = new List<InheritanceRelationsip>();

        public void AddFrom(TypeDeclarationSyntax syntax)
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
                _items.Add(new InheritanceRelationsip(baseTypeName, subTypeName));
            }

        }

        public IEnumerator<InheritanceRelationsip> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
