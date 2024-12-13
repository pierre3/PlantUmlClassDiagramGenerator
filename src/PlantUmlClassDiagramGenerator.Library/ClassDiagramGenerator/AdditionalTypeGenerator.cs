using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlantUmlClassDiagramGenerator.Library.ClassDiagramGenerator;

public partial class ClassDiagramGenerator
{
    private void GenerateAdditionalTypeDeclarations()
    {
        for (int i = 0; i < additionalTypeDeclarationNodes.Count; i++)
        {
            SyntaxNode node = additionalTypeDeclarationNodes[i];
            if (node is GenericNameSyntax genericNode)
            {
                if (createAssociation)
                {
                    GenerateAdditionalGenericTypeDeclaration(genericNode);
                }
                continue;
            }
            Visit(node);
        }
    }

    private void GenerateAdditionalGenericTypeDeclaration(GenericNameSyntax genericNode)
    {
        var typename = TypeNameText.From(genericNode);
        if (!types.Contains(typename.Identifier))
        {
            WriteLine($"class {typename.Identifier}{typename.TypeArguments} {{");
            WriteLine("}");
            types.Add(typename.Identifier);
        }
    }
}