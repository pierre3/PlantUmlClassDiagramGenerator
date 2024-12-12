using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PlantUmlClassDiagramGenerator.Library;
using System.Runtime.InteropServices;
using PlantUmlClassDiagramGenerator.Generator;
using PlantUmlClassDiagramGenerator.Library.ClassDiagramGenerator;

namespace PlantUmlClassDiagramGenerator;

class Program
{
    private static IPlantUmlGenerator generator;

    enum OptionType
    {
        Value,
        Switch
    }

    static readonly Dictionary<string, OptionType> options = new()
    {
        ["-dir"] = OptionType.Switch,
        ["-public"] = OptionType.Switch,
        ["-ignore"] = OptionType.Value,
        ["-excludePaths"] = OptionType.Value,
        ["-createAssociation"] = OptionType.Switch,
        ["-allInOne"] = OptionType.Switch,
        ["-attributeRequired"] = OptionType.Switch,
        ["-excludeUmlBeginEndTags"] = OptionType.Switch,
        ["-addPackageTags"] = OptionType.Switch
    };

    static int Main(string[] args)
    {
        Dictionary<string, string> parameters = MakeParameters(args);
        if (!parameters.ContainsKey("in"))
        {
            Console.WriteLine("Specify a source file name or directory name.");
            return -1;
        }

        if (parameters.ContainsKey("-dir"))
            generator = new PlantUmlFromDirGenerator();
        else
            generator = new PlantUmlFromFileGenerator();

        return !generator.GeneratePlantUml(parameters) ? 1 : 0;
    }


    private static Dictionary<string, string> MakeParameters(string[] args)
    {
        var currentKey = "";
        var parameters = new Dictionary<string, string>();

        foreach (var arg in args)
        {
            if (currentKey != string.Empty)
            {
                parameters.Add(currentKey, arg);
                currentKey = "";
                continue;
            }

            if (options.TryGetValue(arg, out OptionType value))
            {
                if (value == OptionType.Value)
                {
                    currentKey = arg;
                }
                else
                {
                    parameters.Add(arg, string.Empty);
                }
            }
            else
            {
                if (!parameters.TryAdd("in", arg))
                {
                    parameters.TryAdd("out", arg);
                }
            }
        }

        return parameters;
    }
}