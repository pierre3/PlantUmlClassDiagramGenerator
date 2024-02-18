using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace PlantUmlClassDiagramGenerator.SourceGenerator
{

    public partial class PlantUmlSourceGenerator
    {
        public void RegisterAttributes(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(context =>
            {
                const string source = """
                    namespace PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;

                    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Interface | System.AttributeTargets.Enum | System.AttributeTargets.Struct)]
                    internal class PlantUmlDiagramAttribute : System.Attribute
                    { }
                    """;
                context.AddSource("Attributes", SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}
