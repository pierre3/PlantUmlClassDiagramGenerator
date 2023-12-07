using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantUmlClassDiagramGeneratorTest.testData;

class ClassA
{
    int PropA { get; }
    public int PropB { get; }
    protected internal int PropD { get; }
    void MethodA() { }
    private int MethodB() => 1;
    public ClassA() { }
    ClassA() { }
}

struct StructA
{
    string PropA { get; }
    static string PropB { get; }
    private string PropC { get; }
    static void MethodA() { }
    private static int MethodB() => 2;
    public StructA() { }
    StructA() { }
}

interface Interface
{
    int PropA { get; }
    void MethodA();
}
