using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;

[assembly: PlantUmlDiagram(
    IncludeMemberAccessibilities = Accessibilities.Public | Accessibilities.Protected)]
[assembly: PlantUmlExtraAssociationTargets(
    typeof(KeyValuePair<,>),
    typeof(System.Net.Http.HttpClient))]