using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PlantUmlClassDiagramGenerator.SourceGenerator.Extensions;
using PlantUmlClassDiagramGenerator.SourceGenerator.Options;
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
        var assemblyName = context.CompilationProvider
            .Select((compilation, _) => compilation.AssemblyName);

        var options = context.AnalyzerConfigOptionsProvider
            .Combine(assemblyName)
            .Select(static (config, _) => new GeneratorOptions(config.Left, config.Right??""))
            .Combine(context.ParseOptionsProvider
                .Select(static (options, _) => options.PreprocessorSymbolNames));
                
        var typeDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => node is BaseTypeDeclarationSyntax,
                transform: static (context, _) => context.SemanticModel.GetDeclaredSymbol(context.Node))
            .Collect();

        var generateSource = options.Combine(typeDeclarations);

        context.RegisterImplementationSourceOutput(generateSource, static (context, source) =>
        {
            var ((options, preprocessors), targetSymbols) = source;
            if (!preprocessors.Any(s => s == "GENERATE_PLANTUML"))
            {
                return;
            }
            InitiarizeOutputDirectory(Path.Combine(options.OutputDir, options.AssemblyName));
            var symbols = targetSymbols
                .OfType<INamedTypeSymbol>()
                .Where(predicate: GeneratorAttributes.DeclaredTypeFilter)
                .SelectMany(symbol => symbol.EnumerateNestedTypeSymbols()) //Include nested types
                .ToImmutableHashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);

            foreach (var symbol in symbols)
            {
                context.CancellationToken.ThrowIfCancellationRequested();
                var builder = new PlantUmlDiagramBuilder(symbol, options);
                builder.Build(symbols);
                context.CancellationToken.ThrowIfCancellationRequested();
                builder.Write();
            }
        });
    }

    private static void InitiarizeOutputDirectory(string baseDir)
    {
        var info = Directory.CreateDirectory(baseDir);
        foreach (var dir in info.GetDirectories())
        {
            dir.Delete(true);
        }
    }

}