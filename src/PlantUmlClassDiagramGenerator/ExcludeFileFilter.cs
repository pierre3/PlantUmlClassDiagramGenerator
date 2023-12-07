using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlantUmlClassDiagramGenerator
{
    public class ExcludeFileFilter
    {
        public static IEnumerable<string> GetFilesToProcess(IEnumerable<string> files, IList<string> excludePaths, string inputRoot)
        {
            return files.Where(f => !IsFileExcluded(f, excludePaths, inputRoot));
        }

        private static bool IsFileExcluded(string inputFile, IEnumerable<string> excludePaths, string inputRoot)
        {
            bool isExcluded = excludePaths.Any(excludePath => IsFileExcluded(inputFile, excludePath, inputRoot));

            if (isExcluded)
            {
                Console.WriteLine($"Skipped \"{inputFile}\"...");
            }

            return isExcluded;
        }

        private static bool IsFileExcluded(string inputFile, string excludePath, string inputRoot)
        {
            if (excludePath.StartsWith("**/"))
            {
                return inputFile.Split('\\', '/').Any(x => x.StartsWith(excludePath[3..]));
            }
            else
            {
                string fullPath = PathHelper.CombinePath(inputRoot, excludePath);
                return inputFile.StartsWith(fullPath, StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }
}