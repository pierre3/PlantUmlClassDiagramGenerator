namespace PlantUmlClassDiagramGeneratorTest.testData6;

partial class GenericsType
{
    public object Value { get; }
}

class GenericsType2<T>
{
    public T Value { get; }
}

class GenericsType3<T1, T2>
{
    public T1 Value1 { get; }
    public T2 Value2;
}

class SubClass : GenericsType3<string, int>
{
    public string Value1 { get; }
    public int Value2;
}

class SubClass<T>: GenericsType3<GenericsType2<int>, T>
{
    public GenericsType2<int> Value1 { get; }
    public T Value2 { get; }
}

class SubClassX : BaseClass<int>
{
    public GenericsType Gt { get; set; }
}

class BaseClass<Tx>
{
    public Tx X { get; set; }
}

partial class GenericsType
{
    public int Number { get; set; }
}
