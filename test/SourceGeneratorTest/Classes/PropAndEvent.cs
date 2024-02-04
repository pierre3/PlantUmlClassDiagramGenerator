using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceGeneratorTest.Classes;

[PlantUmlDiagram]
class ClassA
{
    public int X { get; }
    public ClassB B { get; private set; } = new();
    public ClassC C { get; private set; }
    private ClassD d = new ClassD();

    public event EventHandler<EventData>? Changed;
    
    private void RaiseChanged(object o, EventData e)
    {
        Changed?.Invoke(o, e);
    }

    public ClassA()
    {
        C = new ClassC();
    }

    public void Update(ClassB b,ClassC c)
    {
        (B, C) = (b, c);
        RaiseChanged(this, new EventData(100, "update"));
    }
}

[PlantUmlDiagram]
class ClassB { }

[PlantUmlDiagram]
class ClassC { }

[PlantUmlDiagram]
class ClassD { }

[PlantUmlDiagram]
record EventData
{
    public int ID { get; }
    public string Name { get; }
    public EventData(int id, string name) => (ID, Name) = (id, name);
}