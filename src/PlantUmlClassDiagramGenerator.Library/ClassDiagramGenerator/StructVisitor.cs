using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlantUmlClassDiagramGenerator.Library.ClassDiagramGenerator;

public partial class ClassDiagramGenerator
{
    public override void VisitStructDeclaration(StructDeclarationSyntax node)
    {
        if (attributeRequired && !node.AttributeLists.HasDiagramAttribute()) { return; }
        if (node.AttributeLists.HasIgnoreAttribute()) { return; }
        if (SkipInnerTypeDeclaration(node)) { return; }

        relationships.AddInnerclassRelationFrom(node);
        relationships.AddInheritanceFrom(node);

        var typeName = TypeNameText.From(node);
        var name = typeName.Identifier;
        var typeParam = typeName.TypeArguments;
        var type = $"{name}{typeParam}";

        types.Add(name);

        WriteLine($"struct {type} {{");

        nestingDepth++;
        base.VisitStructDeclaration(node);
        nestingDepth--;

        WriteLine("}");
    }
}