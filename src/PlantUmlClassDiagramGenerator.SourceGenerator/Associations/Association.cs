using Microsoft.CodeAnalysis;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Associations;

public abstract class Association(ITypeSymbol root, ITypeSymbol leaf) : IEquatable<Association>
{
    protected ITypeSymbol RootSymbol { get; set; } = root;
    protected ITypeSymbol LeafSymbol { get; set; } = leaf;
    protected string Node { get; set; } = "-";
    public string NodeLabel { get; set; } = "";
    public string RootLabel { get; set; } = "";
    public string LeafLabel { get; set; } = "";

    public override string ToString()
    {
        var nodeLabel = NodeLabel == "" ? "" : $" : {NodeLabel}";
        var rootLabel = RootLabel == "" ? "" : $" \"{RootLabel}\"";
        var leafLabel = LeafLabel == "" ? "" : $" \"{LeafLabel}\"";
        var rootName = RootSymbol.MetadataName.Contains('`')
            ? $"\"{RootSymbol.MetadataName}\""
            : RootSymbol.MetadataName;
        var leafName = LeafSymbol.MetadataName.Contains('`')
            ? $"\"{LeafSymbol.MetadataName}\""
            : LeafSymbol.MetadataName;
        return $"{rootName}{rootLabel} {Node} {leafName}{leafLabel}{nodeLabel}";
    }

    public override bool Equals(object? obj)
    {
        return obj is Association association && Equals(association);
    }

    public override int GetHashCode()
    {
        int hashCode = 821845719;
        hashCode = hashCode * -1521134295 + EqualityComparer<ITypeSymbol>.Default.GetHashCode(RootSymbol);
        hashCode = hashCode * -1521134295 + EqualityComparer<ITypeSymbol>.Default.GetHashCode(LeafSymbol);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Node);
        return hashCode;
    }

    public bool Equals(Association other)
    {
        return EqualityComparer<ITypeSymbol>.Default.Equals(RootSymbol, other.RootSymbol) &&
               EqualityComparer<ITypeSymbol>.Default.Equals(LeafSymbol, other.LeafSymbol) &&
               Node == other.Node;
    }
}
