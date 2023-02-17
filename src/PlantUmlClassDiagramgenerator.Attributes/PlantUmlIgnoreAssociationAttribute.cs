using System;

namespace PlantUmlClassDiagramGenerator.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PlantUmlIgnoreAssociationAttribute : Attribute
    { }
}