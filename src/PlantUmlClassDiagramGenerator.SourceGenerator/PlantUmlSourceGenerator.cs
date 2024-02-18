using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PlantUmlClassDiagramGenerator.SourceGenerator.Extensions;
using System.Collections.Immutable;

namespace PlantUmlClassDiagramGenerator.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public partial class PlantUmlSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        RegisterAttributes(context);
        RegisterClassDiagram(context);
    }

    private static void RegisterClassDiagram(IncrementalGeneratorInitializationContext context)
    {
        var options = context.AnalyzerConfigOptionsProvider
            .Select(static (configOptions, _) => configOptions.GlobalOptions.TryGetValue("build_property.projectdir", out var path) ? path : null)
            .Combine(context.ParseOptionsProvider.Select(static (options, _) => options.PreprocessorSymbolNames));

        var typeDeclarations = context.SyntaxProvider.ForAttributeWithMetadataName(
            "PlantUmlClassDiagramGenerator.SourceGenerator.Attributes.PlantUmlDiagramAttribute",
            predicate: static (node, token) => true,
            transform: static (context, token) => context.TargetSymbol)
            .Collect();

        var generateSource = options.Combine(typeDeclarations);

        context.RegisterSourceOutput(generateSource, static (context, source) =>
        {
            var ((projectDir, preprocessors), targetSymbols) = source;
            if (!preprocessors.Any(s => s == "GENERATE_PLANTUML"))
            {
                return;
            }

            string outputDir = InitiarizeOutputDirectory(projectDir);
            var symbols = targetSymbols
                .OfType<INamedTypeSymbol>()
                .SelectMany(symbol => symbol.EnumerateNestedTypeSymbols()) //Include nested types
                .ToImmutableHashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);

            foreach (var symbol in symbols)
            {
                if (context.CancellationToken.IsCancellationRequested) { break; }

                var builder = new PlantUmlDiagramBuilder(symbol);
                builder.Build(symbols);

                if (context.CancellationToken.IsCancellationRequested) { break; }

                File.WriteAllText(
                    Path.Combine(outputDir, symbol.MetadataName + ".puml"),
                    builder.UmlString);
            }
        });

    }

    private static string InitiarizeOutputDirectory(string? projectDir)
    {
        var outputDir = Path.Combine(projectDir, "generated-uml");
        var info = Directory.CreateDirectory(outputDir);
        foreach (var file in info.EnumerateFiles())
        {
            file.Delete();
        }
        return outputDir;
    }
}
