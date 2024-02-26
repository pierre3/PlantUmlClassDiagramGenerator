using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;
using System;

namespace SourceGeneratorTest.Library.Logs
{
    [PlantUmlDiagram]
    public interface ILogger
    {
        void Write(string message, LogLevel logLevel, Exception exception);
        void WriteTrace(string message);
        void WriteDebug(string message);
        void WriteInfo(string message);
        void WriteWarn(string message);
        void WriteError(string message, Exception exception);
        void WriteFatal(string message, Exception exception);
    }
}
