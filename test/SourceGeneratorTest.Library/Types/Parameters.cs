using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;

namespace SourceGeneratorTest.Library.Types;

[PlantUmlDiagram]
public record Parameters
{
    public int X { get; }
    public int Y { get; }
    public Parameters(int x, int y) => (X, Y) = (x, y);

    public int Area() => X * Y;
}
