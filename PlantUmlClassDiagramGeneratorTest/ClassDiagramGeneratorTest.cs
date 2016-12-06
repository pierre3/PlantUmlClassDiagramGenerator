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
        public void GenerateTest_All()
        {
            var code = File.ReadAllText("inputClasses.cs");
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot();

            var output = new StringBuilder();
            using (var writer = new StringWriter(output))
            {
                var gen = new ClassDiagramGenerator(writer, "    ");
                gen.Generate(root);
            }

            var expected = File.ReadAllText(@"uml\all.puml");
            var actual = output.ToString();
            Console.Write(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GenerateTest_Public()
        {
            var code = File.ReadAllText("inputClasses.cs");
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot();

            var output = new StringBuilder();
            using (var writer = new StringWriter(output))
            {
                var gen = new ClassDiagramGenerator(writer, "    ",
                    Accessibilities.Private | Accessibilities.Internal 
                    | Accessibilities.Protected | Accessibilities.ProtectedInternal);
                gen.Generate(root);
            }

            var expected = File.ReadAllText(@"uml\public.puml");
            var actual = output.ToString();
            Console.Write(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GenerateTest_WithoutPrivate()
        {
            var code = File.ReadAllText("inputClasses.cs");
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot();

            var output = new StringBuilder();
            using (var writer = new StringWriter(output))
            {
                var gen = new ClassDiagramGenerator(writer, "    ",Accessibilities.Private);
                gen.Generate(root);
            }

            var expected = File.ReadAllText(@"uml\withoutPrivate.puml");
            var actual = output.ToString();
            Console.Write(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}
