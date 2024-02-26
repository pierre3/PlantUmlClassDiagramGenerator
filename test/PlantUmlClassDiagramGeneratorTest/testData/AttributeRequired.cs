using PlantUmlClassDiagramGenerator.Attributes;

namespace PlantUmlClassDiagramGeneratorTest.testData;


class ClassA
{
    [PlantUmlAssociation(Association ="*--")]
    public string Prop1 { get; set; }
    [PlantUmlIgnore]
    public int Prop2 { get; set; }
}


class ClassB
{
    [PlantUmlAssociation(Association = "*--")]
    public string Prop1 { get; set; }
    [PlantUmlIgnore]
    public int Prop2 { get; set; }
}

interface IInterfaceA
{
    [PlantUmlAssociation(Association ="--",Label ="double property")]
    double Num1 { get; }
    double Num2 { get; }
}


interface IInterfaceB
{
    [PlantUmlAssociation(Association = "--", Label = "double property")]
    double Num1 { get; }
    double Num2 { get; }
}

record RecordA
{
    public string Prop1 { get; }
    public string Prop2 { get; }
    public RecordA(string prop1, string prop2) => (Prop1, Prop2) = (prop1, prop2);
    
}


record RecordB
{
    public string Prop1 { get; }
    public string Prop2 { get; }
    public RecordB(string prop1, string prop2) => (Prop1, Prop2) = (prop1, prop2);
}
    
struct StructA
{
    private RecordA field1;
    public StructA([PlantUmlAssociation(Association = "-->", Label = "use")] RecordA field1)
    {
        this.field1 = field1;
    }
}


struct StructB
{
    private RecordB field1;
    public StructB([PlantUmlAssociation(Association ="-->",Label ="use")]RecordB field1)
    {
        this.field1 = field1;
    }
}

record struct RecordStructA(int X,int Y);


record struct RecordStructB(int X,int Y);

enum EnumA
{
    A,
    B,
    C
}


enum EnumB
{
    A,
    B,
    C
}
