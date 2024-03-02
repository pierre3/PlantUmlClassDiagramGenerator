using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;
using System;
using System.Linq;

namespace SourceGeneratorTest.Library.Types;

[PlantUmlDiagram]
class ClassA
{
    public string Name { get; }
    public int Value { get; }
    public ClassA(string name, int value) => (Name, Value) = (name, value);
}

[PlantUmlDiagram]
static class StaticClass
{
    public static string SpecificName = "Hoge";
    public static string Piyo(int count) => string.Join(" ", Enumerable.Repeat("Pyyo", count));
}

[PlantUmlDiagram]
abstract class AbstractClass
{
    public abstract void MethodA();
    public abstract void MethodB();
}

[PlantUmlDiagram]
record RecordA(string Name,int Value);

[PlantUmlDiagram]
public struct StructA()
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

[PlantUmlDiagram]
public record struct RecordStruct(float X, float Y, float Z);

[PlantUmlDiagram]
public readonly struct ReadonlyStruct
{
    public float X { get; init; }
    public float Y { get; init; }
    public float Z { get; init; }
}

[PlantUmlDiagram]
[Flags]
enum Accessibilities
{
    None = 0,
    Public = 0x01,
    Protected = 0x02,
    Internal = 0x04,
    ProtectedInternal = 0x08,
    PrivateProtected = 0x10,
    Private = 0x20,
    All = Public | Protected | Internal | ProtectedInternal | PrivateProtected | Private
}