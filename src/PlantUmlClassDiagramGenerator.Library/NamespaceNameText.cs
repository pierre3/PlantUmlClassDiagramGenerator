namespace PlantUmlClassDiagramGenerator.Library;

using Microsoft.CodeAnalysis.CSharp.Syntax;

public class NamespaceNameText
{
    public string Identifier { get; set; }
    
    public static NamespaceNameText From(FileScopedNamespaceDeclarationSyntax syntax)
    {
        return new NamespaceNameText
        {
            Identifier = syntax.Name.ToString()
        };
    }
    
    public static NamespaceNameText From(NamespaceDeclarationSyntax syntax)
    {
        return new NamespaceNameText
        {
            Identifier = syntax.Name.ToString()
        };
    }
}