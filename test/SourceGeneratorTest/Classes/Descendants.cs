namespace SourceGeneratorTest.Classes;
using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;

[PlantUmlDiagram(
    DisableAssociationTypes = AssociationTypes.None,
    IncludeMemberAccessibilities = Accessibilities.Private)]
internal class Parent
{
    [PlantUmlDiagram(
        DisableAssociationTypes = AssociationTypes.None,
        IncludeMemberAccessibilities = Accessibilities.Private)]
    class ChiledA
    {
        [PlantUmlDiagram(
            DisableAssociationTypes = AssociationTypes.None,
            IncludeMemberAccessibilities = Accessibilities.Private)]
        class GrandchildA
        {
            [PlantUmlDiagram(
                DisableAssociationTypes = AssociationTypes.None,
                IncludeMemberAccessibilities = Accessibilities.Private)]
            class GreatGrandchild
            {

            }
        }
        [PlantUmlDiagram(
            DisableAssociationTypes = AssociationTypes.None,
            IncludeMemberAccessibilities = Accessibilities.Private)]
        class GrandchildB
        {
            [PlantUmlDiagram(
                DisableAssociationTypes = AssociationTypes.None,
                IncludeMemberAccessibilities = Accessibilities.Private)]
            class GreatGrandchild
            {

            }
        }
    }
    class ChildeB
    {
        class GrandchildA
        {
            class GreatGrandchild
            {
            }
        }
        class GrandchildB
        {

        }
    }
}
