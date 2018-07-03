using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantUmlClassDiagramGeneratorTest
{
    class GenericsType
    {
        public object Value { get; }
    }

    class GenericsType<T>
    {
        public T Value { get; }
    }

    class GenericsType<T1, T2>
    {
        public T1 Value1 { get; }
        public T2 Value2 { get; }
    }

    class SubClass : GenericsType<string, int>
    {
        public string Value1 { get; }
        public int Value2 { get; }
    }

    class SubClass<T>: GenericsType<GenericsType<int>, T>
    {
        public GenericsType<int> Value1 { get; }
        public T Value2 { get; }
    }
}
