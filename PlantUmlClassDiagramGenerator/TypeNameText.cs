using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using System.Collections.Generic;

namespace PlantUmlClassDiagramGenerator
{
    public class TypeNameText
    {
        public string Identifier { get; set; }

        public string TypeArguments { get; set; }

        public static TypeNameText From(SimpleNameSyntax syntax)
        {
            var identifier = syntax.Identifier.Text;
            var typeArgs = string.Empty;
            var genericName = syntax as GenericNameSyntax;
            if (genericName != null)
            {
                var count = genericName.TypeArgumentList.Arguments.Count;
                identifier = $"\"{identifier}`{count}\"";
                typeArgs = "<" + string.Join(",", genericName.TypeArgumentList.Arguments) + ">";
            }
            return new TypeNameText
            {
                Identifier = identifier,
                TypeArguments = typeArgs
            };
        }

        public static TypeNameText From(BaseTypeDeclarationSyntax syntax)
        {
            var identifier = syntax.Identifier.Text;
            var typeArgs = string.Empty;
            var typeDeclaration = syntax as TypeDeclarationSyntax;
            if (typeDeclaration != null && typeDeclaration.TypeParameterList != null)
            {
                var count = typeDeclaration.TypeParameterList.Parameters.Count;
                identifier = $"\"{identifier}`{count}\"";
                typeArgs = "<" + string.Join(",", typeDeclaration.TypeParameterList.Parameters) + ">";
            }
            return new TypeNameText
            {
                Identifier = identifier,
                TypeArguments = typeArgs
            };
        }
    }
}
