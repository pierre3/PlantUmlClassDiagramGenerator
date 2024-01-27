using Microsoft.CodeAnalysis;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Associations;

public record AssociationKind(string Node)
{
    public static AssociationKind Association = new("--");
    public static AssociationKind Aggregation = new("o--");
    public static AssociationKind Composition = new("*--");
    public static AssociationKind Dependency = new("..>");
    public static AssociationKind Inheritance = new("<|--");
    public static AssociationKind Realization = new("<|..");

    public string Node { get; } = Node;

    public Association Create(ITypeSymbol rootSymbol, ITypeSymbol leafSymbol,
        string label = "", string rootLabel = "", string leafLabel = "")
    {
        return new Association(rootSymbol, leafSymbol, this, label, rootLabel, leafLabel);
    }
}
