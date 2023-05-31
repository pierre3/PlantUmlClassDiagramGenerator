using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantUmlClassDiagramGeneratorTest.testData
{
    record MyRecord
    {
        public string A { get;}
        public int B { get; }
        public MyRecord(string a, int b) => (A, B) = (a, b);
    }

    record struct MyStructRecord
    {
        public string A { get; init; }
        public int B { get; init; }
    }
    
    record MyRecord2(string A, int B);
    record struct MyStructRecord2(string A, int B);

    record MyGenericRecord< T1 ,T2 >(T1 Type1, T2 Type2);
    record struct MyGenericStructRecord<T1, T2>
    {
        public T1 Type1 { get; private set; }
        public T2 Type2 { get; private set; }
        public MyGenericStructRecord(T1 type1, T2 type2) => (Type1, Type2) = (type1, type2);
    }

   record MyRecord3(MyGenericRecord<long,bool> p1);

   abstract record MyRecord4()
   {
       public abstract string Execute();
   };
}
