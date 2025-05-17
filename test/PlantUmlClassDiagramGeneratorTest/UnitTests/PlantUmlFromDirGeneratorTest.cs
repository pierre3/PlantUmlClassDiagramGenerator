using PlantUmlClassDiagramGenerator.Generator;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace PlantUmlClassDiagramGeneratorTest;

public partial class PlantUmlFromDirGeneratorTest : IDisposable
{
    private ITestOutputHelper outputHelper;
    private static string outputDir = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "TestResults",
        DateTime.Now.ToString("yyyyMMdd_HHmmss"));

    public PlantUmlFromDirGeneratorTest(ITestOutputHelper outputHelper)
    {
        this.outputHelper = outputHelper;
        Directory.CreateDirectory(outputDir);
    }


    [Theory]
    [InlineData(1, false, false, false)]
    [InlineData(2, false, true, false)]
    [InlineData(3, false, false, true)]
    [InlineData(4, false, true, true)]
    [InlineData(5, true, false, false)]
    [InlineData(6, true, true, false)]
    [InlineData(7, true, false, true)]
    [InlineData(8, true, true, true)]
    public void Generate(int testNum, bool allInOne, bool createAssociation, bool addPackageTags)
    {
        var actualDir = Path.Combine("uml", "Planets", $"case_{testNum}");
        var expectedDir = Path.Combine(outputDir, $"case_{testNum}");
        var inputDir = Path.Combine("testData", "Planets");
        var parameters = new Dictionary<string, string>{
            {"in", inputDir},
            {"out", Path.Combine(outputDir, $"case_{testNum}")}
        };
        if (createAssociation) parameters.Add("-createAssociation", "true");
        if (allInOne) parameters.Add("-allInOne", "true");
        if (addPackageTags) parameters.Add("-addPackageTags", "true");

        var generator = new PlantUmlFromDirGenerator();
        var result = generator.GeneratePlantUml(parameters);
        Assert.True(result);
        Assert.True(TestHelper.CompareDirectories(expectedDir, actualDir));
    }

    public void Dispose()
    {
        if (Directory.Exists(outputDir))
        {
            Directory.Delete(outputDir, true);
        }
    }


}