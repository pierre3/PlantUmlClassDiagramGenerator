using Microsoft.CodeAnalysis;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Associations;

public class Aggregation : Association
{
    public Aggregation(ITypeSymbol root, ITypeSymbol leaf) : base(root, leaf)
    {
        Node = "o--";
    }
}
