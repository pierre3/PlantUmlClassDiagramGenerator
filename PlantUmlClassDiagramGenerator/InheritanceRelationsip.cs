using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlantUmlClassDiagramGenerator
{
    public class InheritanceRelationsip
    {
        private TypeNameText _baseTypeName;
        private TypeNameText _subTypeName;

        public InheritanceRelationsip(TypeNameText baseTypeName, TypeNameText subTypeName)
        {
            _baseTypeName = baseTypeName;
            _subTypeName = subTypeName;
        }

        public override string ToString()
        {
            var typeArgs = string.IsNullOrWhiteSpace(_baseTypeName.TypeArguments) ?
                "" :
                " \"" + _baseTypeName.TypeArguments + "\"";
            return $"{_baseTypeName.Identifier}{typeArgs} <|-- {_subTypeName.Identifier}";
        }
    }
}
