using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using PlantUmlClassDiagramGenerator.Library;
using PlantUmlClassDiagramGenerator.Library.ClassDiagramGenerator;

namespace PlantUmlClassDiagramGenerator.Generator;

public class PlantUmlFromFileGenerator : IPlantUmlGenerator
{
    public bool GeneratePlantUml(Dictionary<string, string> parameters)
    {
        var inputFileName = parameters["in"];
        if (!File.Exists(inputFileName))
        {
            Console.WriteLine($"\"{inputFileName}\" does not exist.");
            return false;
        }
        string outputFileName;
        if (parameters.TryGetValue("out", out string value))
        {
            outputFileName = value;
            try
            {
                var outdir = Path.GetDirectoryName(outputFileName);
                Directory.CreateDirectory(outdir);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        else
        {
            outputFileName = PathHelper.CombinePath(Path.GetDirectoryName(inputFileName), Path.GetFileNameWithoutExtension(inputFileName) + ".puml");
        }

        try
        {
            using var stream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read);
            var tree = CSharpSyntaxTree.ParseText(SourceText.From(stream));
            var root = tree.GetRoot();
            Accessibilities ignoreAcc = IPlantUmlGenerator.GetIgnoreAccessibilities(parameters);

            using var filestream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(filestream);
            var gen = new ClassDiagramGenerator(
                writer,
                "    ",
                ignoreAcc,
                parameters.ContainsKey("-createAssociation"),
                parameters.ContainsKey("-attributeRequired"),
                parameters.ContainsKey("-excludeUmlBeginEndTags"),
                false,
                parameters.ContainsKey("-removeSystemCollectionsAssociations"));
            gen.Generate(root);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
}