using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlantUmlClassDiagramGenerator;
using System.IO;

namespace PlantUmlClassDiagramGeneratorTest.UnitTests
{
    [TestClass]
    public class ExcludeFileFilterTest
    {
        private ExcludeFileFilter testObject;

        private static string RootDir => Environment.OSVersion.Platform == PlatformID.Unix ? "/" : "D:\\";
        private static string InputRoot => Path.Combine(RootDir, "Development", "ProductA", "src");

        private static string TestFile0 => Path.Combine(InputRoot, "ProjectA", "File1.cs");
        private static string TestFile1 => Path.Combine(InputRoot, "ProjectA", "File2.cs");
        private static string TestFile2 => Path.Combine(InputRoot, "ProjectA", "bin", "Domain.dll");
        private static string TestFile3 => Path.Combine(InputRoot, "ProjectA", "obj", "Domain.dll");
        private static string TestFile4 => Path.Combine(InputRoot, "ProjectB", "File1.cs");
        private static string TestFile5 => Path.Combine(InputRoot, "ProjectB", "bin", "Domain.dll");
        private static string TestFile6 => Path.Combine(InputRoot, "ProjectB", "obj", "Domain.dll");

        private readonly string[] TestFiles =
            {TestFile0, TestFile1, TestFile2, TestFile3, TestFile4, TestFile5, TestFile6};

        [TestInitialize]
        public void TestInitialize()
        {
            testObject = new ExcludeFileFilter();
        }

        [DataTestMethod]
        [DataRow(new string[] { }, new[] { 0, 1, 2, 3, 4, 5, 6 }, DisplayName = "Exclude path (empty array)")]
        [DataRow(new[] { "ProjectA\\bin" }, new[] { 0, 1, 3, 4, 5, 6 }, DisplayName = "Exclude path (one)")]
        [DataRow(new[] { "ProjectA\\bin", "ProjectB\\bin" }, new[] { 0, 1, 3, 4, 6 }, DisplayName = "Exclude path (multiple)")]
        [DataRow(new[] { "**/bin" }, new[] { 0, 1, 3, 4, 6 }, DisplayName = "Exclude pattern (one)")]
        [DataRow(new[] { "**/bin", "**/obj" }, new[] { 0, 1, 4 }, DisplayName = "Exclude pattern (multiple)")]
        [DataRow(new[] { "**/bin", "ProjectB\\", "**/obj" }, new[] { 0, 1 }, DisplayName = "Mixed combination of exclude path and pattern")]
        public void GetFilesToProcessTest(string[] excludePaths, int[] expectedTestFileIndices)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                excludePaths = excludePaths.Select(s => s.Replace("\\", "/")).ToArray();
            }
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