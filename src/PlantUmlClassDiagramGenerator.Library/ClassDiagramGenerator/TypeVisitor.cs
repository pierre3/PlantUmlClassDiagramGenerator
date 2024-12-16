using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlantUmlClassDiagramGenerator.Library.ClassDiagramGenerator;

public partial class ClassDiagramGenerator
{
    public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
    {
        VisitTypeDeclaration(node, () => base.VisitInterfaceDeclaration(node));
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        VisitTypeDeclaration(node, () => base.VisitClassDeclaration(node));
    }
    
    private void VisitTypeDeclaration(TypeDeclarationSyntax node, Action visitBase)
    {
        if (attributeRequired && !node.AttributeLists.HasDiagramAttribute()) { return; }
        if (node.AttributeLists.HasIgnoreAttribute()) { return; }
        if (SkipInnerTypeDeclaration(node)) { return; }

        relationships.AddInnerclassRelationFrom(node);
        relationships.AddInheritanceFrom(node);

        var modifiers = GetTypeModifiersText(node.Modifiers);
        var keyword = (node.Modifiers.Any(SyntaxKind.AbstractKeyword) ? "abstract " : "")
                      + node.Keyword.ToString();

        var typeName = TypeNameText.From(node);
        var name = typeName.Identifier;
        var typeParam = typeName.TypeArguments;
        var type = $"{name}{typeParam}";

        types.Add(name);

        WriteLine($"{keyword} {type} {modifiers}{{");

        nestingDepth++;
        visitBase();
        nestingDepth--;

        WriteLine("}");
    }
}