using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using System.Collections.Generic;

namespace PlantUmlClassDiagramGenerator
{
    public class ClassDiagramGenerator : CSharpSyntaxWalker
    {
        private IList<SyntaxNode> _innerTypeDeclarationNodes;
        private Accessibilities _ignoreMemberAccessibilities;
        private InheritanceRelationshipCollection _inheritanceRelationsips
            = new InheritanceRelationshipCollection();
        private TextWriter writer;
        private string indent;
        private int nestingDepth = 0;

        public ClassDiagramGenerator(TextWriter writer, string indent, Accessibilities ignoreMemberAccessibilities = Accessibilities.None)
        {
            this.writer = writer;
            this.indent = indent;
            _innerTypeDeclarationNodes = new List<SyntaxNode>();
            _ignoreMemberAccessibilities = ignoreMemberAccessibilities;
        }

        public void Generate(SyntaxNode root)
        {
            WriteLine("@startuml");
            GenerateInternal(root);
            WriteLine("@enduml");
        }

        public void GenerateInternal(SyntaxNode root)
        {
            Visit(root);
            GenerateInnerTypeDeclarations();
            foreach(var inheritance in _inheritanceRelationsips)
            {
                WriteLine(inheritance.ToString());
            }
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            VisitTypeDeclaration(node, () => base.VisitInterfaceDeclaration(node));
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            VisitTypeDeclaration(node, () => base.VisitClassDeclaration(node));
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            if (SkipInnerTypeDeclaration(node)) { return; }

            _inheritanceRelationsips.AddFrom(node);

            var typeName = TypeNameText.From(node);
            var name = typeName.Identifier;
            var typeParam = typeName.TypeArguments;

            WriteLine($"class {name}{typeParam} <<struct>> {{");

            nestingDepth++;
            base.VisitStructDeclaration(node);
            nestingDepth--;

            WriteLine("}");
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            if (SkipInnerTypeDeclaration(node)) { return; }

            WriteLine($"{node.EnumKeyword} {node.Identifier} {{");

            nestingDepth++;
            base.VisitEnumDeclaration(node);
            nestingDepth--;

            WriteLine("}");
        }

        public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            if (IsIgnoreMember(node.Modifiers)) { return; }

            var modifiers = GetMemberModifiersText(node.Modifiers);
            var name = node.Identifier.ToString();
            var args = node.ParameterList.Parameters.Select(p => $"{p.Identifier}:{p.Type}");

            WriteLine($"{modifiers}{name}({string.Join(", ", args)})");
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            if (IsIgnoreMember(node.Modifiers)) { return; }

            var modifiers = GetMemberModifiersText(node.Modifiers);
            var typeName = node.Declaration.Type.ToString();
            var variables = node.Declaration.Variables;
            foreach (var field in variables)
            {
                var useLiteralInit = field.Initializer?.Value?.Kind().ToString().EndsWith("LiteralExpression") ?? false;
                var initValue = useLiteralInit ? (" = " + field.Initializer.Value.ToString()) : "";

                WriteLine($"{modifiers}{field.Identifier} : {typeName}{initValue}");
            }
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            if (IsIgnoreMember(node.Modifiers)) { return; }

            var modifiers = GetMemberModifiersText(node.Modifiers);
            var name = node.Identifier.ToString();
            var typeName = node.Type.ToString();

            //Property does not have an accessor is an expression-bodied property. (get only)
            var accessorStr = "<<get>>"; 
            if (node.AccessorList != null)
            {
                var accessor = node.AccessorList.Accessors
                    .Where(x => !x.Modifiers.Select(y => y.Kind()).Contains(SyntaxKind.PrivateKeyword))
                    .Select(x => $"<<{(x.Modifiers.ToString() == "" ? "" : (x.Modifiers.ToString() + " "))}{x.Keyword}>>");
                accessorStr = string.Join(" ", accessor);
            }
            var useLiteralInit = node.Initializer?.Value?.Kind().ToString().EndsWith("LiteralExpression") ?? false;
            var initValue = useLiteralInit ? (" = " + node.Initializer.Value.ToString()) : "";

            WriteLine($"{modifiers}{name} : {typeName} {accessorStr}{initValue}");
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (IsIgnoreMember(node.Modifiers)) { return; }

            var modifiers = GetMemberModifiersText(node.Modifiers);
            var name = node.Identifier.ToString();
            var returnType = node.ReturnType.ToString();
            var args = node.ParameterList.Parameters.Select(p => $"{p.Identifier}:{p.Type}");

            WriteLine($"{modifiers}{name}({string.Join(", ", args)}) : {returnType}");
        }

        public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
        {
            WriteLine($"{node.Identifier}{node.EqualsValue},");
        }

        public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            if (IsIgnoreMember(node.Modifiers)) { return; }

            var modifiers = GetMemberModifiersText(node.Modifiers);
            var name = string.Join(",", node.Declaration.Variables.Select(v => v.Identifier));
            var typeName = node.Declaration.Type.ToString();

            WriteLine($"{modifiers} <<{node.EventKeyword}>> {name} : {typeName} ");
        }

        private void WriteLine(string line)
        {
            var space = string.Concat(Enumerable.Repeat(indent, nestingDepth));
            writer.WriteLine(space + line);
        }

        private bool SkipInnerTypeDeclaration(SyntaxNode node)
        {
            if (nestingDepth > 0)
            {
                _innerTypeDeclarationNodes.Add(node);
                return true;
            }
            return false;
        }

        private void GenerateInnerTypeDeclarations()
        {
            foreach (var node in _innerTypeDeclarationNodes)
            {
                var generator = new ClassDiagramGenerator(writer, indent);
                generator.GenerateInternal(node);

                var outerTypeNode = node.Parent as BaseTypeDeclarationSyntax;
                var innerTypeNode = node as BaseTypeDeclarationSyntax;
                if (outerTypeNode != null && innerTypeNode != null)
                {
                    var outerTypeName = TypeNameText.From(outerTypeNode);
                    var innerTypeName = TypeNameText.From(innerTypeNode);
                    WriteLine($"{outerTypeName.Identifier} +-- {innerTypeName.Identifier}");
                }
            }
        }

        private void VisitTypeDeclaration(TypeDeclarationSyntax node, Action visitBase)
        {
            if (SkipInnerTypeDeclaration(node)) { return; }

            _inheritanceRelationsips.AddFrom(node);

            var modifiers = GetTypeModifiersText(node.Modifiers);
            var keyword = (node.Modifiers.Any(SyntaxKind.AbstractKeyword) ? "abstract " : "")
                + node.Keyword.ToString();

            var typeName = TypeNameText.From(node);
            var name = typeName.Identifier;
            var typeParam = typeName.TypeArguments;

            WriteLine($"{keyword} {name}{typeParam} {modifiers}{{");

            nestingDepth++;
            visitBase();
            nestingDepth--;

            WriteLine("}");
        }

        private string GetTypeModifiersText(SyntaxTokenList modifiers)
        {
            var tokens = modifiers.Select(token =>
            {
                switch (token.Kind())
                {
                    case SyntaxKind.PublicKeyword:
                    case SyntaxKind.PrivateKeyword:
                    case SyntaxKind.ProtectedKeyword:
                    case SyntaxKind.InternalKeyword:
                    case SyntaxKind.AbstractKeyword:
                        return "";
                    default:
                        return $"<<{token.ValueText}>>";
                }
            }).Where(token => token != "");

            var result = string.Join(" ", tokens);
            if (result != string.Empty)
            {
                result += " ";
            };
            return result;
        }

        private bool IsIgnoreMember(SyntaxTokenList modifiers)
        {
            if (_ignoreMemberAccessibilities == Accessibilities.None) { return false; }

            var tokenKinds = modifiers.Select(x => x.Kind()).ToArray();

            if (_ignoreMemberAccessibilities.HasFlag(Accessibilities.ProtectedInternal)
                && tokenKinds.Contains(SyntaxKind.ProtectedKeyword)
                && tokenKinds.Contains(SyntaxKind.InternalKeyword))
            {
                return true;
            }

            if (_ignoreMemberAccessibilities.HasFlag(Accessibilities.Public)
                && tokenKinds.Contains(SyntaxKind.PublicKeyword))
            {
                return true;
            }

            if (_ignoreMemberAccessibilities.HasFlag(Accessibilities.Protected)
                && tokenKinds.Contains(SyntaxKind.ProtectedKeyword))
            {
                return true;
            }

            if (_ignoreMemberAccessibilities.HasFlag(Accessibilities.Internal)
                && tokenKinds.Contains(SyntaxKind.InternalKeyword))
            {
                return true;
            }

            if (_ignoreMemberAccessibilities.HasFlag(Accessibilities.Private)
                && tokenKinds.Contains(SyntaxKind.PrivateKeyword))
            {
                return true;
            }
            return false;
        }

        private string GetMemberModifiersText(SyntaxTokenList modifiers)
        {
            var tokens = modifiers.Select(token =>
            {
                switch (token.Kind())
                {
                    case SyntaxKind.PublicKeyword:
                        return "+";
                    case SyntaxKind.PrivateKeyword:
                        return "-";
                    case SyntaxKind.ProtectedKeyword:
                        return "#";
                    case SyntaxKind.AbstractKeyword:
                    case SyntaxKind.StaticKeyword:
                        return $"{{{token.ValueText}}}";
                    case SyntaxKind.InternalKeyword:
                    default:
                        return $"<<{token.ValueText}>>";
                }
            });

            var result = string.Join(" ", tokens);
            if (result != string.Empty)
            {
                result += " ";
            };
            return result;
        }
    }

}
