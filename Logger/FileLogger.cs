namespace Logger;

using System.IO;

public class FileLogger : BaseLogger
{
    private string _filePath;

    public override void Log(LogLevel logLevel, string message)
    {
        using StreamWriter sw = File.AppendText(_filePath);

        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        string className = nameof(this.ClassName);


        sw.WriteLine($"{timestamp} {className} {logLevel}: {message}");

    }

    public FileLogger(string filePath)
    {
        this._filePath = filePath;
    }

    public string GetFilePath()
    {
        return _filePath;
    }
}