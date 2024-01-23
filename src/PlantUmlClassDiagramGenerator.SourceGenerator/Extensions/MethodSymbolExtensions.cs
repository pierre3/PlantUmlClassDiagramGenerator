using Microsoft.CodeAnalysis;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Extensions;

public static class MethodSymbolExtensions
{
    public static string GetName(this IMethodSymbol symbol)
    {
        return symbol.MethodKind switch
        {
            MethodKind.Constructor => symbol.ContainingType.Name,
            MethodKind.StaticConstructor => symbol.ContainingType.Name,
            _ => symbol.ToDisplayString(new SymbolDisplayFormat(
                genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters
                    | SymbolDisplayGenericsOptions.IncludeVariance))
        };
    }
    public static string GetReturnTypeString(this IMethodSymbol symbol)
    {
        return symbol.ReturnsVoid ? "void" : symbol.ReturnType.GetTypeName();
    }

    public static string GetModifiersString(this IMethodSymbol symbol)
    {
        var modifiers = string.Join(" ",
            new[]
            {
                symbol.ContainingType.TypeKind is not TypeKind.Interface 
                    && symbol.IsAbstract ? "{abstract}" : "",
                symbol.IsStatic ? "{static}" : "",
                symbol.IsSealed ? "<<sealed>>" : "",
                symbol.IsReadOnly ? "<<readonly>>" : "",
                symbol.IsOverride ? "<<override>>" : "",
                symbol.IsVirtual ? "<<virtual>>" : "",
                symbol.IsAsync ? "<<async>>":"",
            }.Where(s => !string.IsNullOrEmpty(s)));
        if (modifiers != string.Empty)
        {
            modifiers += " ";
        }
        return modifiers;
    }
    public static string GetParametersString(this IMethodSymbol symbol)
    {
        return string.Join(", ", symbol.Parameters
            .Select(param => $"{param.Name} : {param.Type.GetTypeName()}"));
    }
}
