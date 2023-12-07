using System.IO;

namespace PlantUmlClassDiagramGenerator;

public static class PathHelper
{
    public static string CombinePath(string first, string second)
    {
        return first.TrimEnd(Path.DirectorySeparatorChar)
               + Path.DirectorySeparatorChar
               + second.TrimStart(Path.DirectorySeparatorChar);
    }
}