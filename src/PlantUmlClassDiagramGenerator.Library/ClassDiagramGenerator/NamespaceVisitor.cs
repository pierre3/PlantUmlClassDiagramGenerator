using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlantUmlClassDiagramGenerator.Library.ClassDiagramGenerator;

public partial class ClassDiagramGenerator
{
    public override void VisitFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax node)
    {
        if (this.addPackageTags)
            VisitFileScopedNamespaceDeclaration(node, () => base.VisitFileScopedNamespaceDeclaration(node));
        else
            base.VisitFileScopedNamespaceDeclaration(node);
    }
    
    public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
    {
        if (this.addPackageTags)
            VisitNamespaceDeclaration(node, () => base.VisitNamespaceDeclaration(node));
        else
            base.VisitNamespaceDeclaration(node);
    }
    
    private void VisitFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax node, Action visitBase)
    {
        if (attributeRequired && !node.AttributeLists.HasDiagramAttribute()) { return; }
        if (node.AttributeLists.HasIgnoreAttribute()) { return; }
        if (SkipInnerTypeDeclaration(node)) { return; }

        var typeName = NamespaceNameText.From(node);

        WriteLine($"package \"{typeName.Identifier}\" {{");
        nestingDepth++;
        visitBase();
        nestingDepth--;
        WriteLine("}");
    }
    
    private void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node, Action visitBase)
    {
        if (attributeRequired && !node.AttributeLists.HasDiagramAttribute()) { return; }
        if (node.AttributeLists.HasIgnoreAttribute()) { return; }
        if (SkipInnerTypeDeclaration(node)) { return; }

        var typeName = NamespaceNameText.From(node);

        WriteLine($"package \"{typeName.Identifier}\"{{");
        nestingDepth++;
        visitBase();
        nestingDepth--;
        WriteLine("}");
    }
}