using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using PlantUmlClassDiagramGenerator.Generator;

namespace PlantUmlClassDiagramGenerator;

class Program
{
    static int Main(string[] args)
    {
        var inputArg = new Argument<string>("input")
        {
            Description = "Source file path, directory path, or multi-dir spec (Label=Path;Label=Path;...)"
        };

        var outputArg = new Argument<string>("output")
        {
            Description = "Output file or directory path",
            DefaultValueFactory = _ => null!
        };

        var dirOption = new Option<bool>("--dir", "-d")
        {
            Description = "Process input as a directory of .cs files"
        };

        var publicOption = new Option<bool>("--public", "-p")
        {
            Description = "Only include public members"
        };

        var ignoreOption = new Option<string>("--ignore", "-i")
        {
            Description = "Comma-separated accessibilities to ignore (e.g. Private,Internal)"
        };

        var excludePathsOption = new Option<string>("--exclude-paths", "-e")
        {
            Description = "Comma-separated paths to exclude from processing"
        };

        var createAssociationOption = new Option<bool>("--create-association", "-a")
        {
            Description = "Generate association lines between types"
        };

        var allInOneOption = new Option<bool>("--all-in-one")
        {
            Description = "Combine all output into a single file (directory mode)"
        };

        var attributeRequiredOption = new Option<bool>("--attribute-required")
        {
            Description = "Only process types with [PlantUmlDiagram] attribute"
        };

        var excludeUmlTagsOption = new Option<bool>("--exclude-uml-tags")
        {
            Description = "Exclude @startuml/@enduml tags from output"
        };

        var addPackageTagsOption = new Option<bool>("--add-package-tags")
        {
            Description = "Add package tags for namespaces"
        };

        var rootCommand = new RootCommand("Generate PlantUML class diagrams from C# source code")
        {
            Arguments = { inputArg, outputArg },
            Options =
            {
                dirOption,
                publicOption,
                ignoreOption,
                excludePathsOption,
                createAssociationOption,
                allInOneOption,
                attributeRequiredOption,
                excludeUmlTagsOption,
                addPackageTagsOption
            }
        };

        rootCommand.SetAction(parseResult =>
        {
            var input = parseResult.GetValue(inputArg)!;
            var output = parseResult.GetValue(outputArg);

            var parameters = new Dictionary<string, string> { ["in"] = input };
            if (!string.IsNullOrEmpty(output)) parameters["out"] = output;
            if (parseResult.GetValue(dirOption)) parameters["-dir"] = string.Empty;
            if (parseResult.GetValue(publicOption)) parameters["-public"] = string.Empty;
            if (parseResult.GetValue(ignoreOption) is { } ignore) parameters["-ignore"] = ignore;
            if (parseResult.GetValue(excludePathsOption) is { } exclude) parameters["-excludePaths"] = exclude;
            if (parseResult.GetValue(createAssociationOption)) parameters["-createAssociation"] = string.Empty;
            if (parseResult.GetValue(allInOneOption)) parameters["-allInOne"] = string.Empty;
            if (parseResult.GetValue(attributeRequiredOption)) parameters["-attributeRequired"] = string.Empty;
            if (parseResult.GetValue(excludeUmlTagsOption)) parameters["-excludeUmlBeginEndTags"] = string.Empty;
            if (parseResult.GetValue(addPackageTagsOption)) parameters["-addPackageTags"] = string.Empty;

            IPlantUmlGenerator generator;
            if (input.Contains(';'))
                generator = new PlantUmlFromMultiDirGenerator();
            else if (parameters.ContainsKey("-dir"))
                generator = new PlantUmlFromDirGenerator();
            else
                generator = new PlantUmlFromFileGenerator();

            return generator.GeneratePlantUml(parameters) ? 0 : 1;
        });

        for (int i = 0; i < rootCommand.Options.Count; i++)
        {
            if (rootCommand.Options[i] is HelpOption helpOption)
            {
                helpOption.Action = new HelpWithExamplesAction((HelpAction)helpOption.Action!);
                break;
            }
        }

        var parseResult = rootCommand.Parse(args);
        return parseResult.Invoke();
    }
}

sealed class HelpWithExamplesAction(HelpAction inner) : SynchronousCommandLineAction
{
    public override int Invoke(ParseResult parseResult)
    {
        inner.Invoke(parseResult);

        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  puml-gen C:\\Source\\App1\\ClassA.cs --public");
        Console.WriteLine("  puml-gen C:\\Source\\App1 C:\\PlantUml\\App1 --dir --ignore Private,Protected --create-association --all-in-one");
        Console.WriteLine("  puml-gen C:\\Source\\App1 C:\\PlantUml\\App1 --dir --exclude-paths bin,obj,Properties");

        return 0;
    }
}
