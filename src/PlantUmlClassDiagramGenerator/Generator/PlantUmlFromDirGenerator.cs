using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using PlantUmlClassDiagramGenerator.Library;
using PlantUmlClassDiagramGenerator.Library.ClassDiagramGenerator;

namespace PlantUmlClassDiagramGenerator.Generator;

public class PlantUmlFromDirGenerator: IPlantUmlGenerator
{
    public bool GeneratePlantUml(Dictionary<string, string> parameters)
    {
        var inputRoot = parameters["in"];
        if (!Directory.Exists(inputRoot))
        {
            Console.WriteLine($"Directory \"{inputRoot}\" does not exist.");
            return false;
        }

        // Use GetFullPath to fully support relative paths.
        var outputRoot = Path.GetFullPath(inputRoot);
        if (parameters.TryGetValue("out", out string outValue))
        {
            outputRoot = outValue;
            try
            {
                Directory.CreateDirectory(outputRoot);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        var excludePaths = new List<string>();
        var pumlexclude = PathHelper.CombinePath(inputRoot, ".pumlexclude");
        if (File.Exists(pumlexclude))
        {
            excludePaths = File
                .ReadAllLines(pumlexclude)
                .Where(path => !string.IsNullOrWhiteSpace(path))
                .Select(path => path.Trim())
                .ToList();
        }
        if (parameters.TryGetValue("-excludePaths", out string excludePathValue))
        {
            var splitOptions = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
            excludePaths.AddRange(excludePathValue.Split(',', splitOptions));
        }

        var excludeUmlBeginEndTags = parameters.ContainsKey("-excludeUmlBeginEndTags");
        var files = Directory.EnumerateFiles(inputRoot, "*.cs", SearchOption.AllDirectories);

        var includeRefs = new StringBuilder();
        if (!excludeUmlBeginEndTags) includeRefs.AppendLine("@startuml");

        var error = false;
        var filesToProcess = ExcludeFileFilter.GetFilesToProcess(files, excludePaths, inputRoot);
        RelationshipCollection relationships = new();
        foreach (var inputFile in filesToProcess)
        {
            Console.WriteLine($"Processing \"{inputFile}\"...");
            try
            {
                var outputDir = PathHelper.CombinePath(outputRoot, Path.GetDirectoryName(inputFile).Replace(inputRoot, ""));
                Directory.CreateDirectory(outputDir);
                var outputFile = PathHelper.CombinePath(outputDir, Path.GetFileNameWithoutExtension(inputFile) + ".puml");

                using (var stream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                {
                    var tree = CSharpSyntaxTree.ParseText(SourceText.From(stream));
                    var root = tree.GetRoot();
                    Accessibilities ignoreAcc = IPlantUmlGenerator.GetIgnoreAccessibilities(parameters);

                    using var filestream = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
                    using var writer = new StreamWriter(filestream);
                    var gen = new ClassDiagramGenerator(
                        writer,
                        "    ",
                        ignoreAcc,
                        parameters.ContainsKey("-createAssociation"),
                        parameters.ContainsKey("-attributeRequired"),
                        excludeUmlBeginEndTags,
                        parameters.ContainsKey("-addPackageTags"));
                    gen.Generate(root);
                    relationships.AddAll(gen.relationships);
                }

                if (parameters.ContainsKey("-allInOne"))
                {
                    var lines = File.ReadAllLines(outputFile);
                    if (!excludeUmlBeginEndTags)
                    {
                        lines = lines.Skip(1).SkipLast(1).ToArray();
                    }
                    foreach (string line in lines)
                    {
                        includeRefs.AppendLine(line);
                    }
                }
                else
                {
                    var newRoot = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? @".\" : @".";
                    includeRefs.AppendLine("!include " + outputFile.Replace(outputRoot, newRoot));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                error = true;
            }
        }

        if (parameters.ContainsKey("-addPackageTags"))
        {
            var lines = ClassDiagramGenerator.GenerateRelationships(relationships);
            foreach (string line in lines.Distinct())
            {
                includeRefs.AppendLine(line);
            }
        }
            
        if (!excludeUmlBeginEndTags) includeRefs.AppendLine("@enduml");
        File.WriteAllText(PathHelper.CombinePath(outputRoot, "include.puml"), includeRefs.ToString());

        if (error)
        {
            Console.WriteLine("There were files that could not be processed.");
            return false;
        }
        return true;
    }
}