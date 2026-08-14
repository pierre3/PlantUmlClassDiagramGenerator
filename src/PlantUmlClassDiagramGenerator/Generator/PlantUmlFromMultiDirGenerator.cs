using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using PlantUmlClassDiagramGenerator.Library;
using PlantUmlClassDiagramGenerator.Library.ClassDiagramGenerator;

namespace PlantUmlClassDiagramGenerator.Generator;

/// <summary>
/// Processes multiple source directories into a single PlantUML diagram.
/// Input format for "in" parameter: "Label=Path;Label=Path;..."
/// Each directory becomes a package block in the output.
/// </summary>
public class PlantUmlFromMultiDirGenerator : IPlantUmlGenerator
{
    public bool GeneratePlantUml(Dictionary<string, string> parameters)
    {
        var inputSpec = parameters["in"];
        var entries = ParseEntries(inputSpec);

        if (entries.Count == 0)
        {
            Console.WriteLine("No valid directory entries found.");
            return false;
        }

        foreach (var (label, path) in entries)
        {
            if (!Directory.Exists(path))
            {
                Console.WriteLine($"Directory \"{path}\" (label \"{label}\") does not exist.");
                return false;
            }
        }

        if (!parameters.TryGetValue("out", out string outputFile))
        {
            Console.WriteLine("Output file path required for multi-dir mode.");
            return false;
        }

        try
        {
            var outdir = Path.GetDirectoryName(outputFile);
            if (!string.IsNullOrEmpty(outdir))
                Directory.CreateDirectory(outdir);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        var excludePaths = new List<string>();
        if (parameters.TryGetValue("-excludePaths", out string excludePathValue))
        {
            var splitOptions = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
            excludePaths.AddRange(excludePathValue.Split(',', splitOptions));
        }

        Accessibilities ignoreAcc = IPlantUmlGenerator.GetIgnoreAccessibilities(parameters);
        bool createAssociation = parameters.ContainsKey("-createAssociation");
        bool attributeRequired = parameters.ContainsKey("-attributeRequired");

        var output = new StringBuilder();
        output.AppendLine("@startuml");
        output.AppendLine();

        RelationshipCollection allRelationships = new();
        var error = false;

        foreach (var (label, inputRoot) in entries)
        {
            output.AppendLine($"package \"{label}\" {{");

            var pumlexclude = PathHelper.CombinePath(inputRoot, ".pumlexclude");
            var dirExcludes = new List<string>(excludePaths);
            if (File.Exists(pumlexclude))
            {
                dirExcludes.AddRange(
                    File.ReadAllLines(pumlexclude)
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .Select(p => p.Trim()));
            }

            var files = Directory.EnumerateFiles(inputRoot, "*.cs", SearchOption.AllDirectories);
            var filesToProcess = ExcludeFileFilter.GetFilesToProcess(files, dirExcludes, inputRoot);

            foreach (var inputFile in filesToProcess)
            {
                Console.WriteLine($"Processing \"{inputFile}\"...");
                try
                {
                    using var stream = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
                    var tree = CSharpSyntaxTree.ParseText(SourceText.From(stream));
                    var root = tree.GetRoot();

                    using var sw = new StringWriter();
                    var gen = new ClassDiagramGenerator(
                        sw, "    ", ignoreAcc, createAssociation, attributeRequired,
                        excludeUmlBeginEndTags: true, addPackageTags: false);
                    gen.Generate(root);
                    allRelationships.AddAll(gen.relationships);

                    foreach (var line in sw.ToString().Split('\n'))
                    {
                        var trimmed = line.TrimEnd('\r');
                        if (!string.IsNullOrWhiteSpace(trimmed))
                            output.AppendLine("    " + trimmed);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    error = true;
                }
            }

            output.AppendLine("}");
            output.AppendLine();
        }

        var relLines = ClassDiagramGenerator.GenerateRelationships(allRelationships);
        foreach (var line in relLines.Distinct())
        {
            output.AppendLine(line);
        }

        output.AppendLine("@enduml");

        File.WriteAllText(outputFile, output.ToString());
        Console.WriteLine($"Written to \"{outputFile}\"");

        if (error)
        {
            Console.WriteLine("There were files that could not be processed.");
            return false;
        }
        return true;
    }

    private static List<(string Label, string Path)> ParseEntries(string spec)
    {
        var result = new List<(string, string)>();
        var splitOptions = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
        var segments = spec.Split(';', splitOptions);

        foreach (var segment in segments)
        {
            var eqIndex = segment.IndexOf('=');
            if (eqIndex > 0)
            {
                var label = segment[..eqIndex].Trim();
                var path = segment[(eqIndex + 1)..].Trim();
                result.Add((label, Path.GetFullPath(path)));
            }
            else
            {
                var path = Path.GetFullPath(segment);
                var label = new DirectoryInfo(path).Name;
                result.Add((label, path));
            }
        }

        return result;
    }
}
