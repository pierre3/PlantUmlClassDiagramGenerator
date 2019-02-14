using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlantUmlClassDiagramGenerator.Library
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
            if (genericName != null && genericName.TypeArgumentList != null)
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

        public static TypeNameText From(GenericNameSyntax syntax)
        {
            int paramCount = syntax.TypeArgumentList.Arguments.Count;
            string[] parameters = new string[paramCount];
            if (paramCount > 1)
            {
                for (int i = 0; i < paramCount; i++)
                {
                    parameters[i] = $"T{i + 1}";
                }

            }
            else
            {
                parameters[0] = "T";
            }
            return new TypeNameText
            {
                Identifier = $"\"{syntax.Identifier.Text}`{paramCount}\"",
                TypeArguments = "<" + string.Join(",", parameters) + ">",
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