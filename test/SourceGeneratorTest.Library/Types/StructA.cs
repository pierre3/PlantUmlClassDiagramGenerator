using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;

namespace SourceGeneratorTest.Library.Types;

[PlantUmlDiagram(IncludeMemberAccessibilities =Accessibilities.Public| Accessibilities.Private)]
public struct StructA(string name)
{
    private static int value1 = 100;
    
    public string Name { get; } = name;
    public int intValue { get; private set; }

    public void MethodA() { }
    public int MethodB() { return value1; }
}
