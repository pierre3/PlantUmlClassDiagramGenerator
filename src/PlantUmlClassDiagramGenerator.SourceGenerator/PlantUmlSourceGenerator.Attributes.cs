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

                    [global::System.Flags]
                    internal enum Accessibilities
                    {
                        None = 0,
                        Public = 0x01,
                        Protected = 0x02,
                        Internal = 0x04,
                        ProtectedInternal = 0x08,
                        PrivateProtected = 0x10,
                        Private = 0x20,
                        All = Public | Protected | Internal | ProtectedInternal | PrivateProtected | Private
                    }
                
                    [global::System.AttributeUsage(
                        global::System.AttributeTargets.Class
                        | global::System.AttributeTargets.Interface
                        | global::System.AttributeTargets.Enum
                        | global::System.AttributeTargets.Struct)]
                    internal class PlantUmlDiagramAttribute : global::System.Attribute
                    { 
                        public Accessibilities IncludeMemberAccessibilities { get; set; } = Accessibilities.All;
                        public Accessibilities ExcludeMemberAccessibilities { get; set; } = Accessibilities.None;
                    }

                    [global::System.AttributeUsage(
                        global::System.AttributeTargets.Class
                        | global::System.AttributeTargets.Struct
                        | global::System.AttributeTargets.Enum
                        | global::System.AttributeTargets.Constructor
                        | global::System.AttributeTargets.Method
                        | global::System.AttributeTargets.Property
                        | global::System.AttributeTargets.Field
                        | global::System.AttributeTargets.Event
                        | global::System.AttributeTargets.Interface
                        | global::System.AttributeTargets.Parameter)]
                    internal class PlantUmlIgnoreAttribute : global::System.Attribute
                    { }
                    """;
                context.AddSource("Attributes", SourceText.From(source, Encoding.UTF8));
            });



        }
    }
}
