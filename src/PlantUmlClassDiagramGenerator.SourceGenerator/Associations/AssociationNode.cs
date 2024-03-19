using Microsoft.CodeAnalysis;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Associations;

public record AssociationNode(string Node)
{
    public static AssociationNode Association = new("--");
    public static AssociationNode Aggregation = new("o--");
    public static AssociationNode Composition = new("*--");
    public static AssociationNode Dependency = new("..>");
    public static AssociationNode Inheritance = new("<|--");
    public static AssociationNode Realization = new("<|..");
    public static AssociationNode Nest = new("+..");

    public string Node { get; } = Node;

    public Association Create(ITypeSymbol rootSymbol, ITypeSymbol leafSymbol,
        string label = "", string rootLabel = "", string leafLabel = "")
    {
        return new Association(rootSymbol, leafSymbol, this, label, rootLabel, leafLabel);
    }
}
