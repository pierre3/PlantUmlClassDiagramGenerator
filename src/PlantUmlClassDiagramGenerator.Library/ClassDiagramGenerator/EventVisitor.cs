using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlantUmlClassDiagramGenerator.Library.ClassDiagramGenerator;

public partial class ClassDiagramGenerator
{
    public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
    {
        if (IsIgnoreMember(node.Modifiers)) { return; }

        var modifiers = GetMemberModifiersText(node.Modifiers,
            isInterfaceMember: node.Parent.IsKind(SyntaxKind.InterfaceDeclaration));
        var name = string.Join(",", node.Declaration.Variables.Select(v => v.Identifier));
        var typeName = node.Declaration.Type.ToString();

        WriteLine($"{modifiers} <<{node.EventKeyword}>> {name} : {typeName} ");
    }
}