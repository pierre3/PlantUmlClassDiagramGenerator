using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlantUmlClassDiagramGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** PlantUML Class-Diagram Generator ***");

            if (args.Length < 1)
            {
                Console.WriteLine("Specify a source file name or directory name.");
                Console.Read();
                return;
            }

            var input = args[0];
            IEnumerable<string> files;
            if (Directory.Exists(input))
            {
                files = Directory.EnumerateFiles(Path.GetFullPath(input), "*.cs");
            }
            else if (File.Exists(input))
            {
                try
                {
                    var fullname = Path.GetFullPath(input);
                    files = new[] { fullname };
                }
                catch
                {
                    Console.WriteLine("Invalid file name.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Specify a source file name or directory name.");
                Console.Read();
                return;
            }

            var outputDir = "";
            if (args.Length >= 2)
            {
                if (Directory.Exists(args[1]))
                {
                    outputDir = args[1];
                }
            }

            if (outputDir == "")
            {
                outputDir = Path.Combine(Path.GetDirectoryName(files.First()),"uml");
                Directory.CreateDirectory(outputDir);
            }

            foreach (var file in files)
            {
                Console.WriteLine($"Generating PlantUML from {file}...");
                var outputFile = Path.Combine( outputDir, 
                    Path.GetFileNameWithoutExtension(file) + ".plantuml");

                using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    var tree = CSharpSyntaxTree.ParseText(SourceText.From(stream));
                    var root = tree.GetRoot();
                    
                    using (var writer = new StreamWriter(outputFile))
                    {
                        var gen = new ClassDiagramGenerator(writer, "    ");
                        gen.Visit(root);
                    }
                }
            }
            Console.WriteLine("Completed.");
            Console.Read();
        }
    }
}
