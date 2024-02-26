using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;

namespace SourceGeneratorTest.Library.Types;

[PlantUmlDiagram]
public readonly struct ReadonlyStruct
{
    public string StringA { get; init; }
    public int IntA { get; init; }
    public int IntB { get; init; }
}
