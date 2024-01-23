using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlantUmlClassDiagramGenerator.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public partial class PlantUmlSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        //RegisterPlantUmlClassDiagramGenerator(context);
        RegisterAttributes(context);
        RegisterClassDiagram(context);
    }

    private static void RegisterClassDiagram(IncrementalGeneratorInitializationContext context)
    {
        var projectDir = context.AnalyzerConfigOptionsProvider
            .Select(static (configOptions, _) => configOptions.GlobalOptions
                .TryGetValue("build_property.projectdir", out var path) ? path : null);
        var typeDeclarations = context.SyntaxProvider.ForAttributeWithMetadataName(
            "PlantUmlClassDiagramGenerator.SourceGenerator.Attributes.PlantUmlDiagramAttribute",
            predicate: static (node, token) => node is BaseTypeDeclarationSyntax,
            transform: static (context, token) => (BaseTypeDeclarationSyntax)context.TargetNode)
            .Collect();
        var generateSource = projectDir
            .Combine(context.CompilationProvider)
            .Combine(typeDeclarations);

        context.RegisterSourceOutput(generateSource, static (context, source) =>
        {
            var ((projectDir, compilation), typeDeclarations) = source;
            var outputDir = Path.Combine(projectDir, "GeneratedUml");
            Directory.CreateDirectory(outputDir);
            var symbols = typeDeclarations
                .Select(syntax =>
                {
                    var symbol = compilation.GetSemanticModel(syntax.SyntaxTree).GetDeclaredSymbol(syntax);
                    return symbol;
                })
                .Where(symbol => symbol is not null)
                .ToDictionary(
                    symbol => symbol,
                    symbol => symbol!,
                    SymbolEqualityComparer.Default);

            foreach (var item in symbols)
            {
                var builder = new PlantUmlDiagramBuilder(item.Value);
                File.WriteAllText(
                    Path.Combine(outputDir, item.Value.MetadataName + ".puml"),
                    builder.Build(symbols));
            }
        });

    }

    //private static void RegisterPlantUmlClassDiagramGenerator(IncrementalGeneratorInitializationContext context)
    //{
    //    var projectDir = context.AnalyzerConfigOptionsProvider
    //            .Select(static (configOptions, _) => configOptions.GlobalOptions.TryGetValue("build_property.projectdir", out var path) ? path : null);
    //    var generateSource = context.CompilationProvider
    //        .SelectMany(static (context, _) => context.SyntaxTrees)
    //        .Combine(projectDir);

    //    context.RegisterSourceOutput(generateSource, static (context, item) =>
    //    {
    //        var (syntaxTree, projectDir) = item;
    //        var outputDir = Path.Combine(projectDir, "generatedUml");
    //        Directory.CreateDirectory(outputDir);
    //        var root = syntaxTree.GetRoot(context.CancellationToken);
    //        using var writer = new StreamWriter(
    //            Path.Combine(
    //                outputDir,
    //                Path.GetFileNameWithoutExtension(syntaxTree.FilePath) + ".puml"));
    //        var generator = new ClassDiagramGenerator(writer, "    ");
    //        generator.Visit(root);
    //    });
    //}

}
