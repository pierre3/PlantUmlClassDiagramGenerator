using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;


namespace SourceGeneratorTest.Classes;

[PlantUmlDiagram]
struct Point
{
    public Point(int x, int y) => (X, Y) = (x, y);

    public int X { get; set; }
    public int Y { get; set; }
}

[PlantUmlDiagram]
record Paramters(string ParamA, string ParamB, int ParamC, int ParamD);

[PlantUmlDiagram]
enum LogLevel
{
    Trace,
    Debug,
    Info,
    Warning,
    Error
}

[PlantUmlDiagram]
[Flags]
enum ItemFlags
{
    None = 0x00,
    Alpha = 0x01,
    Beta = 0x02,
    Gamma = 0x04,
    Delta = 0x08
}

[PlantUmlDiagram]
interface ILogger<in T>
{
    string WriteLog(LogLevel level, string message);
}

[PlantUmlDiagram]
class Logger : ILogger<DerivedClass>
{
    public string WriteLog(LogLevel level, string message)
    {
        throw new NotImplementedException();
    }

}

[PlantUmlDiagram]
readonly record struct Vector(double X, double Y, double Z)
{
    public double X { get; } = X;
    public double Y { get; } = Y;
    public double Z { get; } = Z;
    public double Norm() => Math.Sqrt(X * X + Y * Y + Z * Z);
}

[PlantUmlDiagram]

class Item
{
    public string Name { get; set; } = "";
    public Vector Value { get; set; }
}

[PlantUmlDiagram]
interface IInterface
{
    void MethodA(Paramters parameters);
    int MethodB(int value);
}

[PlantUmlDiagram]
abstract class BaseClass<T1, T2>
{
    public abstract T1 Name { get; }
    public abstract T2 Value { get; }
    public abstract string GetNameValue();
}

[PlantUmlDiagram]
class DerivedClass : BaseClass<string, int>, IInterface
{
    private ILogger<DerivedClass> Logger { get; }
    public override string Name => throw new NotImplementedException();

    public override int Value => throw new NotImplementedException();

    public IList<Item> Item1 { get; set; } = new List<Item>();
    public Item[] Item2 { get; set; } = [new Item()];

    public DerivedClass(ILogger<DerivedClass> logger)
    {
        Logger = logger;
    }

    public override string GetNameValue()
    {
        throw new NotImplementedException();
    }

    public void MethodA(Paramters parameters)
    {
        throw new NotImplementedException();
    }

    public int MethodB(int value)
    {
        throw new NotImplementedException();
    }

    public void Process(Paramters parameters)
    {
        throw new NotImplementedException();
    }
}
