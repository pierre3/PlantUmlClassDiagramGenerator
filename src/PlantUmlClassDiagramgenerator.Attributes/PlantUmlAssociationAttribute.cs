using System;

namespace PlantUmlClassDiagramGenerator.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class PlantUmlAssociationAttribute : Attribute
    {
        public string Name { get; set; } = string.Empty;
        public string Association { get; set; } = string.Empty;
        public string RootLabel { get; set; } = string.Empty;
        public string Label { get; set; }=string.Empty;
        public string LeafLabel { get; set; } = string.Empty;
    }
}