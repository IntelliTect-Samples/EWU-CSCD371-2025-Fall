using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        var logger = factory.CreateLogger(nameof(factory));

        //Assert
        Assert.IsNull(logger);
    }
}
