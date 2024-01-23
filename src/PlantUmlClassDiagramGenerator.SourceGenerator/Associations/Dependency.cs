using Microsoft.CodeAnalysis;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Associations;

public class Dependency : Association
{
    public Dependency(ITypeSymbol root, ITypeSymbol leaf) : base(root, leaf)
    {
        Node = "..>";
    }
}
