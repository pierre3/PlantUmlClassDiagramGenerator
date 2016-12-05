using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlantUmlClassDiagramGenerator;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;
using System.IO;

namespace PlantUmlClassDiagramGeneratorTest
{
    [TestClass]
    public class ClassDiagramGeneratorTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var code = File.ReadAllText("inputClasses.cs");
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot();

            var output = new StringBuilder();
            using (var writer = new StringWriter(output))
            {
                var gen = new ClassDiagramGenerator(writer, "    ",
                    Accessibilities.Internal | Accessibilities.Private | Accessibilities.Protected | Accessibilities.ProtectedInternal);
                gen.Generate(root);
            }

            Console.WriteLine(output);
        }
    }
}
