using System;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;
using System.IO;
using PlantUmlClassDiagramGenerator.Library;
using Xunit;
using Xunit.Abstractions;

namespace PlantUmlClassDiagramGeneratorTest;

public partial class ClassDiagramGeneratorTest
{
    private ITestOutputHelper outputHelper;
    public ClassDiagramGeneratorTest(ITestOutputHelper outputHelper)
    {
        this.outputHelper = outputHelper;
    }

    [Theory]
    [InlineData("InputClasses.cs", "all.puml", true, false, false, Accessibilities.None)]
    [InlineData("InputClasses.cs", "public.puml", true, false, false, Accessibilities.Private | Accessibilities.Internal | Accessibilities.Protected | Accessibilities.ProtectedInternal)]
    [InlineData("InputClasses.cs", "withoutPrivate.puml", true, false, false, Accessibilities.Private)]
    [InlineData("GenericsType.cs", "GenericsType.puml", true, false, false, Accessibilities.Private | Accessibilities.Internal | Accessibilities.Protected | Accessibilities.ProtectedInternal)]
    [InlineData("NullableType.cs", "nullableType.puml", true, false, false, Accessibilities.Private | Accessibilities.Internal | Accessibilities.Protected | Accessibilities.ProtectedInternal)]
    [InlineData("RecordType.cs", "RecordType.puml", true, false, false, Accessibilities.Private | Accessibilities.Internal | Accessibilities.Protected | Accessibilities.ProtectedInternal)]
    [InlineData("Attributes.cs", "Attributes.puml", true, false, false, Accessibilities.Private | Accessibilities.Internal | Accessibilities.Protected | Accessibilities.ProtectedInternal)]
    [InlineData("AttributeRequired.cs", "AttributeRequired.puml", true, true, false, Accessibilities.None)]
    [InlineData("AttributeRequired.cs", "NotAttributeRequired.puml", true, false, false, Accessibilities.None)]
    [InlineData("InputClasses.cs", "withoutStartEndUml.puml", true, false, true, Accessibilities.Private)]
    [InlineData("DefaultModifierType.cs", "DefaultModifierType.puml", true, false, false, Accessibilities.None)]
    [InlineData("NullableRelationship.cs", "NullableRelationship.puml", true, false, false, Accessibilities.None)]
    public void Generate(string inputClassFile, string outpulPumlFile, bool createAssociations, bool attributeRequired, bool excludeUmlBeginEndTags, Accessibilities accessibilities)
    {
        var code = File.ReadAllText(Path.Combine("testData", inputClassFile));
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = tree.GetRoot();

        var output = new StringBuilder();
        using (var writer = new StringWriter(output))
        {
            var gen = new ClassDiagramGenerator(writer, "    ", accessibilities, createAssociations, attributeRequired, excludeUmlBeginEndTags);
            gen.Generate(root);
        }

        var expected = ConvertNewLineCode(File.ReadAllText(Path.Combine("uml", outpulPumlFile)), Environment.NewLine);
        var actual = output.ToString();
        outputHelper.WriteLine(actual);
        Assert.Equal(expected, actual);
    }

    private static string ConvertNewLineCode(string text, string newline)
    {
        var reg = EndLineRegex();
        return reg.Replace(text, newline);
    }

    [System.Text.RegularExpressions.GeneratedRegex("\r\n|\r|\n")]
    private static partial System.Text.RegularExpressions.Regex EndLineRegex();
}