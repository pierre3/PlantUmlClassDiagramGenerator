using Microsoft.CodeAnalysis;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Associations;

public class Composition : Association
{
    public Composition(ITypeSymbol root, ITypeSymbol leaf) : base(root, leaf)
    {
        Node = "*--";
    }
}
