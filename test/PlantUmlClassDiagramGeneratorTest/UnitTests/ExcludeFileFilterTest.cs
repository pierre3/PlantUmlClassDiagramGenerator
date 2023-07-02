using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlantUmlClassDiagramGenerator;

namespace PlantUmlClassDiagramGeneratorTest.UnitTests
{
    [TestClass]
    public class ExcludeFileFilterTest
    {
        private ExcludeFileFilter testObject;

        private const string InputRoot = "D:\\Development\\ProductA\\src";

        private const string TestFile0 = "D:\\Development\\ProductA\\src\\ProjectA\\File1.cs";
        private const string TestFile1 = "D:\\Development\\ProductA\\src\\ProjectA\\File2.cs";
        private const string TestFile2 = "D:\\Development\\ProductA\\src\\ProjectA\\bin\\Domain.dll";
        private const string TestFile3 = "D:\\Development\\ProductA\\src\\ProjectA\\obj\\Domain.dll";
        private const string TestFile4 = "D:\\Development\\ProductA\\src\\ProjectB\\File1.cs";
        private const string TestFile5 = "D:\\Development\\ProductA\\src\\ProjectB\\bin\\Domain.dll";
        private const string TestFile6 = "D:\\Development\\ProductA\\src\\ProjectB\\obj\\Domain.dll";

        private readonly string[] TestFiles =
            {TestFile0, TestFile1, TestFile2, TestFile3, TestFile4, TestFile5, TestFile6};

        [TestInitialize]
        public void TestInitialize()
        {
            testObject = new ExcludeFileFilter();
        }

        [DataTestMethod]
        [DataRow(new string[] { }, new[] {0, 1, 2, 3, 4, 5, 6}, DisplayName = "Exclude path (empty array)")]
        [DataRow(new[] {"ProjectA\\bin"}, new[] {0, 1, 3, 4, 5, 6}, DisplayName = "Exclude path (one)")]
        [DataRow(new[] {"ProjectA\\bin", "ProjectB\\bin"}, new[] {0, 1, 3, 4, 6}, DisplayName = "Exclude path (multiple)")]
        public void GetFilesToProcessTest(string[] excludePaths, int[] expectedTestFileIndices)
        {
            // Act
            List<string> result = testObject.GetFilesToProcess(TestFiles, excludePaths, InputRoot).ToList();

            // Assert
            string[] expected = GetByIndices(TestFiles, expectedTestFileIndices);
            CollectionAssert.AreEquivalent(expected, result);
        }

        private static string[] GetByIndices(string[] array, params int[] indices)
        {
            return indices.Select(i => array[i]).ToArray();
        }
    }
}