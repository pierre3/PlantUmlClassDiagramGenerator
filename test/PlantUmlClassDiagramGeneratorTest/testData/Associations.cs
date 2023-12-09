using System.Collections.Generic;

namespace PlantUmlClassDiagramGeneratorTest.testData1;

internal class Associations
{
    public IList<AssociatedClass> ListOfA = new List<AssociatedClass>();
    public IList<AssociatedClass>? ListOfANullable = new List<AssociatedClass>();
    public ICollection<AssociatedClass> CollectionOfA = new List<AssociatedClass>();
    public ICollection<AssociatedClass>? CollectionOfANullable = new List<AssociatedClass>();
    public IEnumerable<AssociatedClass> IEnumarableOfA = new List<AssociatedClass>();
    public IEnumerable<AssociatedClass>? IEnumarableOfANullable = new List<AssociatedClass>();
    public AssociatedClass[] ArrayOfA = [];
    public AssociatedClass[]? ArrayOfANullable = [];
    public IList<int> ListOfInt = new List<int>(); //Should not output association (difficult to read association with primitive type in the diagram)
    public int[] ArrayOfInt = []; //Should not output association (difficult to read association with primitive type in the diagram)
}

internal class AssociatedClass
{
}
