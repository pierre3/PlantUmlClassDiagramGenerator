using Microsoft.CodeAnalysis;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Associations;

public class Realization : Association
{
    public Realization(ITypeSymbol root, ITypeSymbol leaf) : base(root, leaf)
    {
        Node = "<|..";
    }
}
