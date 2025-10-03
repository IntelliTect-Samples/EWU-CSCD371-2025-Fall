namespace Logger;

public abstract class BaseLogger
{
    public string ClassName { get; set; }

    protected BaseLogger()
    {
        // Default to the actual derived class name
        ClassName = GetType().Name;
    }

    public abstract void Log(LogLevel logLevel, string message);
}

