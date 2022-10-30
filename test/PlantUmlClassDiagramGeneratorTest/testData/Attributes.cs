using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlantUmlClassDiagramGenerator.Attributes;

namespace PlantUmlClassDiagramGeneratorTest.testData
{
    class Item
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; } = 0;
    }

    class Parameters
    {
        public string A { get; set; }
        public string B { get; set; }
    }

    class MyClass
    {
        [PlantUmlAssociation(Name="Item", Association ="o--", Multiplicity ="0..*", Label="Items")]
        public IList<Item> Items { get; set; }

        [PlantUmlAssociation(Name = "IItem", Association = "*--", Multiplicity = "0..*", Label="ItemDictionary")]
        public IDictionary<string, IItem> ItemDictionary { get; set; } = new Dictionary<string, IItem>();

        [PlantUmlIgnore]
        public string HiddenProp { get; set; }

        [PlantUmlIgnoreAssociation]
        public IReadOnlyCollection<Item> ReadOnlyItems { get; }

        public void Run([PlantUmlAssociation(Association ="..>", Label="use")]Parameters p)
        {
            Console.WriteLine($"{p.A},{p.B}");
        }

        private ILogger logger;
        public MyClass([PlantUmlAssociation(Association = "..>", Label = "Injection")] ILogger logger)
        {
            this.logger = logger;
        }
    }

    [PlantUmlIgnore]
    class HiddenClass 
    {
        public string PropA { get; set; }
    }

}
