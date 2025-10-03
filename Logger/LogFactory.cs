using System;

namespace Logger;

public class LogFactory
{
    private string _filePath;

    public void ConfigureFileLogger(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("A non-empty file path is required.", nameof(filePath));

        _filePath = filePath;
    }

    public BaseLogger CreateLogger(string className)
    {
        if (string.IsNullOrWhiteSpace(_filePath))
        {
            return null;
        }

        return new FileLogger(_filePath)
        {
            ClassName = className
        };
    }

}
