using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;

namespace PlantUmlClassDiagramGenerator.Library.ClassDiagramGenerator;

public partial class ClassDiagramGenerator
{
    private void GenerateRelationships()
    {
        foreach (var relationship in relationships)
        {
            WriteLine(relationship.ToString());
        }
    }
    
    public static string[] GenerateRelationships(RelationshipCollection relationshipCollection)
    {
        List<string> strings = new List<string>();
        strings.AddRange(relationshipCollection.Select(r => r.ToString()));

        return strings.ToArray();
    }
}