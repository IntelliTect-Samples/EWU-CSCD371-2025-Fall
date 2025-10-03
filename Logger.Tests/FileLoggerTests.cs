using Microsoft.VisualStudio.TestTools.UnitTesting;

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

}
