using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;
using System;
using System.Reflection.Metadata;

namespace SourceGeneratorTest.Library
{
    [PlantUmlDiagram(IncludeMemberAccessibilities = Accessibilities.All)]
    public class SampleClass
    {
        public static readonly string ValueX;
        private int value1 = 100;
        protected int Value2 = 0;
        
        internal int Value3 { get; set; }
        protected internal int Value4 { get; set; }

        public event Action<string> EventA = s=> Console.WriteLine(s);

        [PlantUmlIgnore]
        public int Value5 { get; set; }
        public SampleClass()
        {

        }

        static SampleClass()
        {
            ValueX = "X";
        }

        private protected void MethodA() => throw new NotImplementedException();

        [PlantUmlIgnore]
        protected void MethodX() => throw new NotImplementedException();

        protected virtual void VirtualMethod() => throw new NotImplementedException();
        public int MethodB() => value1;
    }
}
