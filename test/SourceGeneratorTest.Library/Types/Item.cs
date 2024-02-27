using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;

namespace SourceGeneratorTest.Library.Types;

[PlantUmlDiagram]
public record Item(string Name, double Value);
