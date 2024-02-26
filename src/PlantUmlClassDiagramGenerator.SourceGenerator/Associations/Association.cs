using Microsoft.CodeAnalysis;
using PlantUmlClassDiagramGenerator.SourceGenerator.Extensions;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Associations;


public record Association(ITypeSymbol RootSymbol, ITypeSymbol LeafSymbol, AssociationKind Kind,
    string Label = "", string RootLabel = "", string LeafLabel = "")
{
    protected ITypeSymbol RootSymbol { get; } = RootSymbol;
    protected ITypeSymbol LeafSymbol { get; } = LeafSymbol;
    protected AssociationKind Kind { get; } = Kind;
    public string Label { get; } = Label;
    public string RootLabel { get; } = RootLabel;
    public string LeafLabel { get; } = LeafLabel;

    public override string ToString()
    {
        var nodeLabel = Label == "" ? "" : $" : {Label}";
        var rootLabel = RootLabel == "" ? "" : $" \"{RootLabel}\"";
        var leafLabel = LeafLabel == "" ? "" : $"\"{LeafLabel}\" ";
        var rootName = RootSymbol.GetMetadataName();
        if (rootName.Contains('`'))
        {
            rootName = $"\"{rootName}\"";
        }
        var leafName = LeafSymbol.GetMetadataName();
        if (leafName.Contains('`'))
        {
            leafName = $"\"{leafName}\"";
        }
        return $"{rootName}{rootLabel} {Kind.Node} {leafLabel}{leafName}{nodeLabel}";
    }
}
