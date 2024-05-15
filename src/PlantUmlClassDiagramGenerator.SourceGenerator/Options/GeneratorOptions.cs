using Microsoft.CodeAnalysis.Diagnostics;
namespace PlantUmlClassDiagramGenerator.SourceGenerator.Options;

internal class GeneratorOptions(AnalyzerConfigOptionsProvider config, string assemblyName)
{
    public string AssemblyName { get; set; } = assemblyName;
    public string OutputDir { get; set; } = GetOutputDir(config);

    public static string GetOutputDir(AnalyzerConfigOptionsProvider config)
    {
        return config.GlobalOptions.TryGetValue("build_property.PlantUmlGenerator_OutputDir", out var path)
            ? path
            : config.GlobalOptions.TryGetValue("build_property.projectDir", out var dir)
                ? Path.Combine(dir, "generated-uml")
                : "";
    }

}
