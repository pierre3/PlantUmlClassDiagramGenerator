using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
            .Combine(context.ParseOptionsProvider.Select(static (options,_)=> options.PreprocessorSymbolNames));

        var typeDeclarations = context.SyntaxProvider.ForAttributeWithMetadataName(
            "PlantUmlClassDiagramGenerator.SourceGenerator.Attributes.PlantUmlDiagramAttribute",
            predicate: static (node, token) => true,
            transform: static (context, token) => context.TargetSymbol)
            .Collect();

        var generateSource = options.Combine(typeDeclarations);

        context.RegisterSourceOutput(generateSource, static (context, source) =>
        {
            var ((projectDir, preprocessors), targetSymbols) = source;
            if(!preprocessors.Any(s=>s == "GENERATE_PLANTMUL")) 
            { 
                return; 
            }
            var outputDir = Path.Combine(projectDir, "generated-uml");
            Directory.Delete(outputDir, true);
            Directory.CreateDirectory(outputDir);
            var symbols = targetSymbols
                .OfType<INamedTypeSymbol>()
                .ToDictionary(
                    symbol => symbol,
                    symbol => symbol,
                    SymbolEqualityComparer.Default);

            foreach (var item in symbols)
            {
                if (context.CancellationToken.IsCancellationRequested) { break; }

                var builder = new PlantUmlDiagramBuilder(item.Value);
                builder.Build(symbols);
                
                if (context.CancellationToken.IsCancellationRequested) { break; }
                
                File.WriteAllText(
                    Path.Combine(outputDir, item.Value.MetadataName + ".puml"),
                    builder.UmlString);
            }
        });

    }
}
