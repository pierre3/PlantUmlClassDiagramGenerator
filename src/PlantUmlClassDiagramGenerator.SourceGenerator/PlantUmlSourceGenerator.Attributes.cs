using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace PlantUmlClassDiagramGenerator.SourceGenerator;

public partial class PlantUmlSourceGenerator
{
    public void RegisterAttributes(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(context =>
        {
            const string source = """
            namespace PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;

            [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Enum | AttributeTargets.Struct)]
            public class PlantUmlDiagramAttribute : Attribute
            { }
            """;
            context.AddSource("Attributes", SourceText.From(source,Encoding.UTF8));
        });
    }
}
