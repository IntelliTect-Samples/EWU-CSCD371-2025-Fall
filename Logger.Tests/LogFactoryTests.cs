using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;

namespace Logger.Tests;

[TestClass]
public class LogFactoryTests
{

    [TestMethod]
    public void LogFactory_CreateLoggerReturnsNull_Success()
    {
        //Arrange
        var factory = new LogFactory();

        //Act
        var logger = factory.CreateLogger(nameof(LogFactoryTests));

        //Assert
        Assert.IsNull(logger);
    }

    [TestMethod]
    public void LogFactory_CreateLoggerReturnsLogger_Success()
    {
        //Arrange
        var factory = new LogFactory();
        string path = "goodpath";
        factory.ConfigureFileLogger(path);
        //Act
        var logger = factory.CreateLogger(nameof(LogFactoryTests));
        //Assert
        Assert.IsNotNull(logger);
        Assert.IsInstanceOfType(logger, typeof(FileLogger));
        Assert.AreEqual((logger as FileLogger)?.GetFilePath(), path);
    }

    [TestMethod]
    public void LogFactory_ConfigureFileLoggerWithInvalidPath_ThrowsException()
    {
        //Arrange
        var factory = new LogFactory();
        //Act & Assert
        Assert.ThrowsException<ArgumentException>(() => factory.ConfigureFileLogger(null));
        Assert.ThrowsException<ArgumentException>(() => factory.ConfigureFileLogger(""));
        Assert.ThrowsException<ArgumentException>(() => factory.ConfigureFileLogger("   "));
    }

    [TestMethod]
    public void LogFactory_ConfigureFileLoggerWithValidPath_Success()
    {
        //Arrange
        var factory = new LogFactory();
        string path = "goodpath";
        //Act
        factory.ConfigureFileLogger(path);
        var logger = factory.CreateLogger(nameof(LogFactoryTests));
        //Assert
        Assert.IsNotNull(logger);
        Assert.IsInstanceOfType(logger, typeof(FileLogger));
        Assert.AreEqual((logger as FileLogger)?.GetFilePath(), path);
    }
}
