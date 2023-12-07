namespace PlantUmlClassDiagramGenerator.Library
{
    public class InheritanceRelationship(TypeNameText baseTypeName, TypeNameText subTypeName) : Relationship(baseTypeName, subTypeName, "<|--", baseTypeName.TypeArguments)
    {
    }
}