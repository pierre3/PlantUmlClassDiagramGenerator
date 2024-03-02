using SourceGeneratorTest.Library.Logs;

namespace SourceGeneratorTest.Classes;

internal class Logger : ILogger
{
    public void Write(string message, LogLevel logLevel, Exception? exception = null)
    {
        Console.WriteLine($"[{logLevel}] {message}");
        if (exception != null)
        {
            Console.WriteLine(exception.Message);
        }
    }
    public void WriteDebug(string message) => Write(message, LogLevel.Debug);
    public void WriteInfo(string message) => Write(message, LogLevel.Info);
    public void WriteTrace(string message) => Write(message, LogLevel.Trace);
    public void WriteWarn(string message) => Write(message, LogLevel.Warn);
    public void WriteError(string message, Exception? exception = null) => Write(message, LogLevel.Error, exception);
    public void WriteFatal(string message, Exception? exception = null) => Write(message, LogLevel.Fatal, exception);
}
