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

            var expected = ConvertNewLineCode(File.ReadAllText(@"uml\all.puml"),Environment.NewLine);
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

            var expected = ConvertNewLineCode(File.ReadAllText(@"uml\public.puml"),Environment.NewLine);
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

            var expected = ConvertNewLineCode( File.ReadAllText(@"uml\withoutPrivate.puml"), Environment.NewLine);
            var actual = output.ToString();
            Console.Write(actual);
            Assert.AreEqual(expected, actual);
        }

        private string ConvertNewLineCode(string text,string newline)
        {
            var reg = new System.Text.RegularExpressions.Regex("\r\n|\r|\n");
            return reg.Replace(text, newline);
        }
    }
}
