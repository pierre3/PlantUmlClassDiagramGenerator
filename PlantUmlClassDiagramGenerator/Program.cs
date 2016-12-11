using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PlantUmlClassDiagramGenerator
{
    class Program
    {
        enum OptionType
        {
            Value,
            Switch
        }

        static readonly Dictionary<string, OptionType> options = new Dictionary<string, OptionType>()
        {
            ["-dir"] = OptionType.Switch,
            ["-public"] = OptionType.Switch,
            ["-ignore"] = OptionType.Value,
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
            {
                if (!GeneratePlantUmlFromDir(parameters)) { return -1; }
            }
            else
            {
                if (!GeneratePlantUmlFromFile(parameters)) { return -1; }
            }
            return 0;
        }

        private static bool GeneratePlantUmlFromFile(Dictionary<string, string> parameters)
        {
            var inputFileName = parameters["in"];
            if (!File.Exists(inputFileName))
            {
                Console.WriteLine($"\"{inputFileName}\" does not exist.");
                return false;
            }
            var outputFileName = "";
            if (parameters.ContainsKey("out"))
            {
                outputFileName = parameters["out"];
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
                outputFileName = Path.Combine(Path.GetDirectoryName(inputFileName),
                    Path.GetFileNameWithoutExtension(inputFileName) + ".puml");
            }

            try
            {
                using (var stream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
                {
                    var tree = CSharpSyntaxTree.ParseText(SourceText.From(stream));
                    var root = tree.GetRoot();
                    Accessibilities ignoreAcc = GetIgnoreAccessibilities(parameters);

                    using(var filestream = new FileStream(outputFileName,FileMode.Create,FileAccess.Write))
                    using (var writer = new StreamWriter(filestream))
                    {
                        var gen = new ClassDiagramGenerator(writer, "    ", ignoreAcc);
                        gen.Generate(root);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        private static bool GeneratePlantUmlFromDir(Dictionary<string, string> parameters)
        {
            var inputRoot = parameters["in"];
            if (!Directory.Exists(inputRoot))
            {
                Console.WriteLine($"Directory \"{inputRoot}\" does not exist.");
                return false;
            }
            var outputRoot = inputRoot;
            if (parameters.ContainsKey("out"))
            {
                outputRoot = parameters["out"];
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
            var files = Directory.EnumerateFiles(inputRoot, "*.cs", SearchOption.AllDirectories);
            var includeRefs = new StringBuilder();
            var error = false;
            foreach (var inputFile in files)
            {
                Console.WriteLine( $"Processing \"{inputFile}\"..." );
                try
                {
                    var outputDir = outputRoot + Path.DirectorySeparatorChar + Path.GetDirectoryName(inputFile).Replace(inputRoot, "");
                    Directory.CreateDirectory(outputDir);
                    var outputFile = Path.Combine(outputDir,
                        Path.GetFileNameWithoutExtension(inputFile) + ".puml");

                    using (var stream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                    {
                        var tree = CSharpSyntaxTree.ParseText(SourceText.From(stream));
                        var root = tree.GetRoot();
                        Accessibilities ignoreAcc = GetIgnoreAccessibilities(parameters);

                        using(var filestream = new FileStream(outputFile,FileMode.Create,FileAccess.Write))
                        using (var writer = new StreamWriter(filestream))
                        {
                            var gen = new ClassDiagramGenerator(writer, "    ", ignoreAcc);
                            gen.Generate(root);
                        }
                    }

                    includeRefs.AppendLine("!include " + outputFile.Replace(outputRoot, @".\"));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    error = true;
                }
            }
            File.WriteAllText(Path.Combine(outputRoot, "include.puml"), includeRefs.ToString());
            if (error)
            {
                Console.WriteLine("There were files that could not be processed.");
                return false;
            }
            return true;
        }

        private static Accessibilities GetIgnoreAccessibilities(Dictionary<string, string> parameters)
        {
            var ignoreAcc = Accessibilities.None;
            if (parameters.ContainsKey("-public"))
            {
                ignoreAcc = Accessibilities.Private | Accessibilities.Internal
                    | Accessibilities.Protected | Accessibilities.ProtectedInternal;
            }
            else if (parameters.ContainsKey("-ignore"))
            {
                var ignoreItems = parameters["-ignore"].Split(',');
                foreach (var item in ignoreItems)
                {
                    Accessibilities acc;
                    if (Enum.TryParse(item, true, out acc))
                    {
                        ignoreAcc |= acc;
                    }
                }
            }
            return ignoreAcc;
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

                if (options.ContainsKey(arg))
                {
                    if (options[arg] == OptionType.Value)
                    {
                        currentKey = arg;
                    }
                    else
                    {
                        parameters.Add(arg, string.Empty);
                    }
                }
                else if (!parameters.ContainsKey("in"))
                {
                    parameters.Add("in", arg);
                }
                else if (!parameters.ContainsKey("out"))
                {
                    parameters.Add("out", arg);
                }
            }
            return parameters;
        }
    }
}
