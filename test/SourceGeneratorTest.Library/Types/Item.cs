using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;

namespace SourceGeneratorTest.Library.Types;

[PlantUmlDiagram]
public record Item(string Name, double Value);

[PlantUmlDiagram]
interface IItemProvider
{
    Item Item { get; }
}

[PlantUmlDiagram]
class ItemProviderA : IItemProvider
{
    private Item _item;
    public Item Item { get => _item; }
    public ItemProviderA(Item item)
    {
        _item = item;
    }
}

[PlantUmlDiagram(DisableAssociationTypes = AssociationTypes.Field
    | AssociationTypes.Realization)]
class ItemProviderB : IItemProvider
{
    private Item _item;
    public Item Item { get => _item; }
    public ItemProviderB(Item item)
    {
        _item = item;
    }
}

