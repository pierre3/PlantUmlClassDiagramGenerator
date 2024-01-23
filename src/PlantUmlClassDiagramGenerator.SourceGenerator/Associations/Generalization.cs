using Microsoft.CodeAnalysis;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Associations;

public class Inheritance : Association
{
    public Inheritance(ITypeSymbol root, ITypeSymbol leaf) : base(root, leaf)
    {
        Node = "<|--";
    }
}
