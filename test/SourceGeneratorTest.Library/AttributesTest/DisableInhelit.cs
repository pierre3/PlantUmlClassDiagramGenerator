using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;
using SourceGeneratorTest.Library.Types;
using System;

namespace SourceGeneratorTest.Library.AttributesTest
{
    [PlantUmlDiagram(DisableAssociationTypes = AssociationTypes.Inheritance | AssociationTypes.Realization)]
    class DisableInhelit : ClassA, IInterfaceA
    {
        private Item item = new("name", 1);
        public Item Item { get => item; }

        public DisableInhelit() : base("name", 1)
        {

        }

        public void Execute(Parameters parameters)
        {
            throw new NotImplementedException();
        }

        public void MethodA()
        {
            throw new NotImplementedException();
        }

        class InnerClass
        {
            public int Value { get; set; }
        }
    }

    [PlantUmlDiagram(DisableAssociationTypes = AssociationTypes.Field)]
    class DisableField : ClassA, IInterfaceA
    {
        private Item item = new("name", 1);
        public Item Item { get => item; }

        public DisableField() : base("name", 1)
        {

        }

        public void Execute(Parameters parameters)
        {
            throw new NotImplementedException();
        }

        public void MethodA()
        {
            throw new NotImplementedException();
        }

        class InnerClass
        {
            public int Value { get; set; }
        }
    }

    [PlantUmlDiagram(DisableAssociationTypes = AssociationTypes.MethodParameter)]
    class DisableParameter : ClassA, IInterfaceA
    {
        private Item item = new("name", 1);
        public Item Item { get => item; }

        public DisableParameter() : base("name", 1)
        {

        }

        public void Execute(Parameters parameters)
        {
            throw new NotImplementedException();
        }

        public void MethodA()
        {
            throw new NotImplementedException();
        }

        class InnerClass
        {
            public int Value { get; set; }
        }
    }

    [PlantUmlDiagram(DisableAssociationTypes = AssociationTypes.Property)]
    class DisableProperty : ClassA, IInterfaceA
    {
        private Item item = new("name", 1);
        public Item Item { get => item; }

        public DisableProperty() : base("name", 1)
        {

        }

        public void Execute(Parameters parameters)
        {
            throw new NotImplementedException();
        }

        public void MethodA()
        {
            throw new NotImplementedException();
        }

        class InnerClass
        {
            public int Value { get; set; }
        }
    }

    [PlantUmlDiagram(DisableAssociationTypes = AssociationTypes.Nest)]
    class DisableNest : ClassA, IInterfaceA
    {
        private Item item = new("name", 1);
        public Item Item { get => item; }

        public DisableNest() : base("name", 1)
        {

        }

        public void Execute(Parameters parameters)
        {
            throw new NotImplementedException();
        }

        public void MethodA()
        {
            throw new NotImplementedException();
        }

        class InnerClass
        {
            public int Value { get; set; }
        }
    }

    [PlantUmlDiagram(DisableAssociationTypes = AssociationTypes.All)]
    class DisableAll : ClassA, IInterfaceA
    {
        private Item item = new("name", 1);
        public Item Item { get => item; }

        public DisableAll() : base("name", 1)
        {

        }

        public void Execute(Parameters parameters)
        {
            throw new NotImplementedException();
        }

        public void MethodA()
        {
            throw new NotImplementedException();
        }

        class InnerClass
        {
            public int Value { get; set; }
        }
    }
}
