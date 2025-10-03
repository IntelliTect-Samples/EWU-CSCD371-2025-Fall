namespace Logger;

using System;
using System.IO;

public class FileLogger : BaseLogger
{
    private readonly string _filePath;

    public FileLogger(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("filePath cannot be null or empty.", nameof(filePath));

        _filePath = filePath;
    }

    public override void Log(LogLevel logLevel, string message)
    {
        string timestamp = DateTime.Now.ToString("M/d/yyyy h:mm:ss tt");
        string className = ClassName;

        using var sw = File.AppendText(_filePath);
        sw.WriteLine($"{timestamp} {className} {logLevel}: {message}");

    }
}