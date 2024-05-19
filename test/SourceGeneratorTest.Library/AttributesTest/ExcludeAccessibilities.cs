using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;

namespace SourceGeneratorTest.Library.AttributesTest
{
    [PlantUmlDiagram(
        IncludeMemberAccessibilities = Accessibilities.Public | Accessibilities.Protected,
        ExcludeMemberAccessibilities = Accessibilities.Public)]
    class Protected
    {
        public static readonly int FieldA = 100;
        private int fieldB;
        protected int fiedlC;
        protected internal int fiedlD;
        private protected int fiedlE;
        internal int fieldF;
        public string PropA { get; set; } = "";
        private string PropB { get; set; } = "";
        protected string PropC { get; set; } = "";
        protected internal string PropD { get; set; } = "";
        private protected string PropE { get; set; } = "";
        internal string PropF { get; set; } = "";
        public Protected()
        {

        }

        public void MethodA() { }
        private void MethodB() { }
        protected void MethodC() { }
        protected internal void MethodD() { }
        private protected void MethodE() { }
        internal void MethodF() { }
    }

    [PlantUmlDiagram(
        IncludeMemberAccessibilities = Accessibilities.Private | Accessibilities.Internal,
        ExcludeMemberAccessibilities = Accessibilities.Private)]
    class Internal
    {
        public static readonly int FieldA = 100;
        private int fieldB;
        protected int fiedlC;
        protected internal int fiedlD;
        private protected int fiedlE;
        internal int fieldF;
        public string PropA { get; set; } = "";
        private string PropB { get; set; } = "";
        protected string PropC { get; set; } = "";
        protected internal string PropD { get; set; } = "";
        private protected string PropE { get; set; } = "";
        internal string PropF { get; set; } = "";
        public Internal()
        {

        }

        public void MethodA() { }
        private void MethodB() { }
        protected void MethodC() { }
        protected internal void MethodD() { }
        private protected void MethodE() { }
        internal void MethodF() { }
    }

    [PlantUmlDiagram(
        IncludeMemberAccessibilities = Accessibilities.ProtectedInternal | Accessibilities.PrivateProtected,
        ExcludeMemberAccessibilities = Accessibilities.ProtectedInternal)]
    class PrivateProtected
    {
        public static readonly int FieldA = 100;
        private int fieldB;
        protected int fiedlC;
        protected internal int fiedlD;
        private protected int fiedlE;
        internal int fieldF;
        public string PropA { get; set; } = "";
        private string PropB { get; set; } = "";
        protected string PropC { get; set; } = "";
        protected internal string PropD { get; set; } = "";
        private protected string PropE { get; set; } = "";
        internal string PropF { get; set; } = "";
        public PrivateProtected()
        {

        }

        public void MethodA() { }
        private void MethodB() { }
        protected void MethodC() { }
        protected internal void MethodD() { }
        private protected void MethodE() { }
        internal void MethodF() { }
    }
}
