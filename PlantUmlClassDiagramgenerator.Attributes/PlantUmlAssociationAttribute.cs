namespace PlantUmlClassDiagramGenerator.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class PlantUmlAssociationAttribute : Attribute
    {
        public string Name { get; set; } = string.Empty;
        public string Association { get; set; } = string.Empty;
        public string Multiplicity { get; set; } = string.Empty;
        public string Label { get; set; }=string.Empty;
    }
}