using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Logger.Tests;

[TestClass]
public class FileLoggerTests
{

    [TestMethod]
    public void FileLogger_CheckFilePath_Success()
    {
        //Arrange
        string path = "goodpath";

        //Act
        FileLogger logger = new FileLogger(path);

        //Assert
        Assert.AreEqual(logger.GetFilePath(), path);

    }

    [TestMethod]
    public void FileLogger_CheckFilePath_Failure()
    {
        //Arrange
        string path = "goodpath";

        //Act
        FileLogger logger = new FileLogger(path);

        //Assert
        Assert.AreNotEqual(logger.GetFilePath(), "badPath");

    }

    [TestMethod]
    public void FileLogger_NullFilePath_ThrowsException()
    {
        //Arrange
        string path = null;
        //Act
        //Assert
        Assert.ThrowsException<ArgumentException>(() => new FileLogger(path));
    }

    [TestMethod]
    public void FileLogger_EmptyFilePath_ThrowsException()
    {
        //Arrange
        string path = "";
        //Act
        //Assert
        Assert.ThrowsException<ArgumentException>(() => new FileLogger(path));
    }

    [TestMethod]
    public void FileLogger_WhitespaceFilePath_ThrowsException()
    {
        //Arrange
        string path = "   ";
        //Act
        //Assert
        Assert.ThrowsException<ArgumentException>(() => new FileLogger(path));
    }

    [TestMethod]
    public void FileLogger_LogMessage_Success()
    {
        //Arrange
        string path = "testlog.txt";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        FileLogger logger = new FileLogger(path);
        string message = "This is a test message.";
        LogLevel level = LogLevel.Information;
        //Act
        logger.Log(level, message);
        //Assert
        Assert.IsTrue(File.Exists(path));
        string[] lines = File.ReadAllLines(path);
        Assert.AreEqual(1, lines.Length);
        StringAssert.Contains(lines[0], message);
        StringAssert.Contains(lines[0], level.ToString());
        // Cleanup
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
