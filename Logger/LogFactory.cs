using System;

namespace Logger;

public class LogFactory
{
    private string _filePath;

    public BaseLogger CreateLogger(string className)
    {
        if(string.IsNullOrEmpty(_filePath))
        {
            throw new ArgumentNullException(nameof(_filePath));
        }

        return new FileLogger(_filePath)
        {
            
            ClassName = className
        };
    }

    public void ConfigureFileLogger(string filePath)
    {
        this._filePath = filePath;
    }
}
