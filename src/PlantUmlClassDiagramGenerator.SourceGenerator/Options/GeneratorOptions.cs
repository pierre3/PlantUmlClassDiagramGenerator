using Microsoft.CodeAnalysis.Diagnostics;
namespace PlantUmlClassDiagramGenerator.SourceGenerator.Options;

internal class GeneratorOptions(AnalyzerConfigOptionsProvider config, string assemblyName)
{
    public string AssemblyName { get; set; } = assemblyName;
    public string OutputDir { get; set; } = GetOutputDir(config);

    public static Accessibilities GetIncludeAccessibilities(AnalyzerConfigOptionsProvider config)
    {
        return GetAccessibilities(config, "build_property.PlantUmlGenerator_IncludeMemberAccessibilities", Accessibilities.All);
    }

    public static Accessibilities GetExcludeAccessibilities(AnalyzerConfigOptionsProvider config)
    {
        return GetAccessibilities(config, "build_property.PlantUmlGenerator_ExcludeMemberAccessibilities", Accessibilities.None);
    }

    private static Accessibilities GetAccessibilities(AnalyzerConfigOptionsProvider config, string optionName, Accessibilities defaultValue)
    {
        return config.GlobalOptions.TryGetValue(optionName, out var acc)
            ? Enum.TryParse<Accessibilities>(acc, out var value)
                ? value
                : defaultValue
            : defaultValue;
    }

    public static bool GetAttributeRequierd(AnalyzerConfigOptionsProvider config)
    {
        return !config.GlobalOptions.TryGetValue("build_property.PlantUmlGenerator_AttributeRequierd", out var requierd)
            || !bool.TryParse(requierd, out var boolean)
            || boolean;
    }

    public static string GetOutputDir(AnalyzerConfigOptionsProvider config)
    {
        return config.GlobalOptions.TryGetValue("build_property.PlantUmlGenerator_OutputDir", out var path)
            ? path
            : config.GlobalOptions.TryGetValue("build_property.projectDir", out var dir)
                ? Path.Combine(dir, "generated-uml")
                : "";
    }

}
