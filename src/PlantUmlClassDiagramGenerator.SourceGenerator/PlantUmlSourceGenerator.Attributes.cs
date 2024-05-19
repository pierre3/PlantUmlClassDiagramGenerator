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
                        NotSet = 0x8000,
                        None = 0,
                        Public = 0x01,
                        Protected = 0x02,
                        Internal = 0x04,
                        ProtectedInternal = 0x08,
                        PrivateProtected = 0x10,
                        Private = 0x20,
                        All = Public | Protected | Internal | ProtectedInternal | PrivateProtected | Private
                    }

                    [global::System.Flags]
                    internal enum AssociationTypes
                    {
                        NotSet = 0x8000,
                        None = 0,
                        Inheritance = 0x01,
                        Realization = 0x02,
                        Property = 0x04,
                        Field = 0x08,
                        MethodParameter = 0x10,
                        Nest = 0x20,
                        All = Inheritance | Realization | Property | Field | MethodParameter | Nest
                    }

                    [global::System.AttributeUsage(
                        global::System.AttributeTargets.Assembly
                        | global::System.AttributeTargets.Class
                        | global::System.AttributeTargets.Interface
                        | global::System.AttributeTargets.Enum
                        | global::System.AttributeTargets.Struct)]
                    internal class PlantUmlDiagramAttribute : global::System.Attribute
                    { 
                        public Accessibilities IncludeMemberAccessibilities { get; set; } = Accessibilities.NotSet;
                        public Accessibilities ExcludeMemberAccessibilities { get; set; } = Accessibilities.NotSet;
                        public AssociationTypes DisableAssociationTypes { get; set; } = AssociationTypes.NotSet;
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
                    internal class PlantUmlAssociationAttribute(string node) : global::System.Attribute
                    { 
                        public string Node { get;} = node;

                        public global::System.Type? LeafType { get; set; } = null;
                        public string RootLabel { get; set; } = "";
                        public string NodeLabel { get; set; } = "";
                        public string LeafLabel { get; set; } = "";
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
                    internal class PlantUmlIgnoreAssociationAttribute : global::System.Attribute
                    { }
                    
                    [global::System.AttributeUsage(
                        global::System.AttributeTargets.Assembly
                        | global::System.AttributeTargets.Class
                        | global::System.AttributeTargets.Interface
                        | global::System.AttributeTargets.Enum
                        | global::System.AttributeTargets.Struct)]
                    internal class PlantUmlExtraAssociationTargetsAttribute(params global::System.Type[] types ) : global::System.Attribute
                    {
                        public global::System.Type[] Types { get; } = types;
                    }
                    """;
                context.AddSource("Attributes", SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}
