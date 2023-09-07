using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Xml.Linq;
using University.Services;

[TestClass]
public class DataAccessServiceTests
{
    [TestMethod]
    public void SaveData_Success()
    {
        // Arrange
        var data = new MyData { Name = "John", Age = 30 };
        var filePath = "test.json";

        var fileSystemMock = new Mock<IFileSystem>();
        fileSystemMock.Setup(fs => fs.WriteAllText(filePath, It.IsAny<string>()));

        var dataService = new DataAccessService<MyData>(fileSystemMock.Object);

        // Act
        dataService.SaveData(data, filePath);

        // Assert
        fileSystemMock.Verify(fs => fs.WriteAllText(filePath, It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void ReadData_Success()
    {
        // Arrange
        var filePath = "test.json";
        var jsonData = "{\"Name\":\"John\",\"Age\":30}";

        var fileSystemMock = new Mock<IFileSystem>();
        fileSystemMock.Setup(fs => fs.Exists(filePath)).Returns(true);
        fileSystemMock.Setup(fs => fs.ReadAllText(filePath)).Returns(jsonData);

        var dataService = new DataAccessService<MyData>(fileSystemMock.Object);

        // Act
        var loadedData = dataService.ReadData(filePath);

        // Assert
        Assert.IsNotNull(loadedData);
        Assert.AreEqual("John", loadedData.Name);
        Assert.AreEqual(30, loadedData.Age);
    }
}