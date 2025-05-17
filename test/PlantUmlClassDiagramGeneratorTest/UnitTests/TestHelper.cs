using System;
using System.IO;
using System.Linq;

namespace PlantUmlClassDiagramGeneratorTest;

static partial class TestHelper
{
    public static bool CompareDirectories(string expectedDir, string actualDir)
    {
        var expectedFiles = Directory.EnumerateFiles(expectedDir, "*.puml", SearchOption.AllDirectories).OrderBy(s => s);
        var actualFiles = Directory.EnumerateFiles(actualDir, "*.puml", SearchOption.AllDirectories).OrderBy(s => s);

        var zippedFiles = expectedFiles.Zip(actualFiles, (e, a) => new { e, a });

        return zippedFiles.All(x => CompareFileContents(x.e, x.a));
    }

    public static bool CompareFileContents(string expectedFile, string actualFile)
    {
        var expected = ConvertNewLineCode(File.ReadAllText(expectedFile), Environment.NewLine);
        var actual = ConvertNewLineCode(File.ReadAllText(actualFile), Environment.NewLine);
        return expected == actual;
    }

    public static string ConvertNewLineCode(string text, string newline)
    {
        var reg = EndLineRegex();
        return reg.Replace(text, newline);
    }

    [System.Text.RegularExpressions.GeneratedRegex("\r\n|\r|\n")]
    private static partial System.Text.RegularExpressions.Regex EndLineRegex();
}