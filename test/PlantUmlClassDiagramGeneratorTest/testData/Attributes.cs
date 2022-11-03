using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlantUmlClassDiagramGenerator.Attributes;

namespace PlantUmlClassDiagramGeneratorTest.testData
{

    class Parameters
    {
        public string A { get; set; }
        public string B { get; set; }
    }

    class MyClass
    {
        [PlantUmlAssociation(Name = "Item", Association = "o--", LeafLabel = "0..*", Label = "Items")]
        public IList<Item> Items { get; set; }

        [PlantUmlAssociation(Name = "IItem", Association = "*--", RootLabel = "1", Label = "ItemDictionary", LeafLabel = "0..*")]
        public IDictionary<string, IItem> ItemDictionary { get; set; } = new Dictionary<string, IItem>();

        [PlantUmlIgnore]
        public string HiddenProp { get; set; }

        [PlantUmlIgnoreAssociation]
        public IReadOnlyCollection<Item> ReadOnlyItems { get; }

        public void Run([PlantUmlAssociation(Association = "..>", Label = "use")] Parameters p)
        {
            Console.WriteLine($"{p.A},{p.B}");
        }

        private ILogger logger;
        public MyClass([PlantUmlAssociation(Association = "..>", Label = "Injection")] ILogger logger)
        {
            this.logger = logger;
        }
    }

    struct MyStruct
    {
        [PlantUmlAssociation(Name = "int", Association = "o--", LeafLabel = "0..*", Label = "intCollection:List<int>")]
        public IList<int> intCollection;

        [PlantUmlAssociation(Name = "Parameters", Association = "-l->")]
        public MyStruct(Parameters p)
        {
        }
    }

    record MyRecord(string name, [PlantUmlAssociation(Association = "o--")] Settings s);

    record struct MyStructRecord
    {
        [PlantUmlAssociation(Name="string", Association = "o--",LeafLabel="Name")]
        public string Name { get; init; }
        
    }


    [PlantUmlIgnore]
    class HiddenClass 
    {
        public string PropA { get; set; }
    }

}
