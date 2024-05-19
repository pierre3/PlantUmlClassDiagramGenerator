using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;

[assembly: PlantUmlDiagram(
    IncludeMemberAccessibilities = Accessibilities.Public | Accessibilities.Protected | Accessibilities.Internal,
    DisableAssociationTypes = AssociationTypes.Field | AssociationTypes.Nest)]
[assembly: PlantUmlExtraAssociationTargets(
    typeof(KeyValuePair<,>),
    typeof(System.Net.Http.HttpClient))]