using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlantUmlClassDiagramGenerator.Library.ClassDiagramGenerator;

public partial class ClassDiagramGenerator
{
    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (node.AttributeLists.HasIgnoreAttribute()) { return; }
        if (IsIgnoreMember(node.Modifiers)) { return; }
        foreach (var parameter in node.ParameterList?.Parameters)
        {
            var associationAttrSyntax = parameter.AttributeLists.GetAssociationAttributeSyntax();
            if (associationAttrSyntax is not null)
            {
                var associationAttr = CreateAssociationAttribute(associationAttrSyntax);
                relationships.AddAssociationFrom(node, parameter, associationAttr);
            }
        }
        var modifiers = GetMemberModifiersText(node.Modifiers,
            isInterfaceMember: node.Parent.IsKind(SyntaxKind.InterfaceDeclaration));
        var name = node.Identifier.ToString();
        var returnType = node.ReturnType.ToString();
        var args = node.ParameterList.Parameters.Select(p => $"{p.Identifier}:{p.Type}");

        WriteLine($"{modifiers}{name}({string.Join(", ", args)}) : {returnType}");
    }
}