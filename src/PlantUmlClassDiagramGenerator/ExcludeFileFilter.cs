using System;
using System.Collections.Generic;
using System.Linq;

namespace PlantUmlClassDiagramGenerator
{
    public class ExcludeFileFilter
    {
        public IEnumerable<string> GetFilesToProcess(IEnumerable<string> files, IList<string> excludePaths, string inputRoot)
        {
            return files.Where(f => !IsFileExcluded(f, excludePaths, inputRoot));
        }

        private static bool IsFileExcluded(string inputFile, IEnumerable<string> excludePaths, string inputRoot)
        {
            bool isExcluded = excludePaths
                .Select(p => PathHelper.CombinePath(inputRoot, p))
                .Any(p => inputFile.StartsWith(p, StringComparison.InvariantCultureIgnoreCase));

            if (isExcluded)
            {
                Console.WriteLine($"Skipped \"{inputFile}\"...");
            }

            return isExcluded;
        }
    }
}