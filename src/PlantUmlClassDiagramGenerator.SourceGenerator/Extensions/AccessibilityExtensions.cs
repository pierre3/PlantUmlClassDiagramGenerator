using Microsoft.CodeAnalysis;

namespace PlantUmlClassDiagramGenerator.SourceGenerator.Extensions;

public static class AccessibilityExtensions
{
    public static string GetMemberAccessibilityString(this Accessibility accessibility) => accessibility switch
    {
        Accessibility.Public => "+ ",
        Accessibility.Private => "- ",
        Accessibility.Internal => "~ ",
        Accessibility.Protected or Accessibility.Friend => "# ",
        Accessibility.ProtectedOrInternal or Accessibility.ProtectedOrFriend => "<<protected internal>> ",
        Accessibility.ProtectedAndInternal or Accessibility.ProtectedAndFriend => "<<private protected>> ",
        _ => ""
    };

    public static string GetAccessorAccessibilityString(this Accessibility accessibility, Accessibility propertyAccessibility)
    {
        if (accessibility == propertyAccessibility) return "";
        return accessibility switch
        {
            Accessibility.Private => "private ",
            Accessibility.Protected or Accessibility.Friend => "protected ",
            Accessibility.ProtectedOrInternal or Accessibility.ProtectedOrFriend => "protected internal ",
            Accessibility.ProtectedAndInternal or Accessibility.ProtectedAndFriend => "private protected ",
            _ => ""
        };
    }
}
