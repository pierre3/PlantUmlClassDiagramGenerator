using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;
using SourceGeneratorTest.Library.Logs;
using SourceGeneratorTest.Library.Types;

namespace SourceGeneratorTest.Classes;

[PlantUmlDiagram(IncludeMemberAccessibilities = Accessibilities.All,
    DisableAssociationTypes = AssociationTypes.Field)]
[PlantUmlExtraAssociationTargets(typeof(System.IO.TextWriter))]
internal class SampleModel
{
    private readonly ILogger logger;

    public IDictionary<string, Item> Items { get; } = new Dictionary<string, Item>();

    public SampleModel(ILogger logger)
    {
        this.logger = logger;
    }

    public void Write(TextWriter writer)
    {
        writer.Write(Items.Count);
    }

    public async ValueTask Execute(Parameters parameters)
    {
        await Task.Delay(1000);
    }
}
