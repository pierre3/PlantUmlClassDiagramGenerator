using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;

namespace SourceGeneratorTest.Library.AttributesTest
{
    [PlantUmlDiagram(IncludeMemberAccessibilities = Accessibilities.Public | Accessibilities.Protected)]
    class PublicProtected
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
        public PublicProtected()
        {

        }

        public void MethodA() { }
        private void MethodB() { }
        protected void MethodC() { }
        protected internal void MethodD() { }
        private protected void MethodE() { }
        internal void MethodF() { }
    }

    [PlantUmlDiagram(IncludeMemberAccessibilities = Accessibilities.Private | Accessibilities.Internal)]
    class PrivateInternal
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
        public PrivateInternal()
        {

        }

        public void MethodA() { }
        private void MethodB() { }
        protected void MethodC() { }
        protected internal void MethodD() { }
        private protected void MethodE() { }
        internal void MethodF() { }
    }

    [PlantUmlDiagram(IncludeMemberAccessibilities = Accessibilities.ProtectedInternal | Accessibilities.PrivateProtected)]
    class ProtectedInternal
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
        public ProtectedInternal()
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
