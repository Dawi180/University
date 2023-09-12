using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using University.Interfaces;
using University.Models;
using University.Services;

namespace University.Services.Tests
{
    [TestClass]
    public class DataAccessServiceTests
    {
        [TestMethod]
        public void SaveData_SavesDataToFile()
        {
            // Arrange
            var fileMock = new Mock<IFileWrapper>();
            var dataService = new DataAccessService(fileMock.Object);
            string filePath = "test.json";
            var testData = new Student
            {
                StudentId = 1,
                Name = "Alice",
                LastName = "Smith",
            };

            // Set up the behavior for File.WriteAllText method
            fileMock.Setup(x => x.WriteAllText(filePath, It.IsAny<string>())).Verifiable();

            // Act
            dataService.SaveData(filePath, testData);

            // Assert
            // Verify that File.WriteAllText was called with the expected arguments
            fileMock.Verify(x => x.WriteAllText(filePath, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void LoadData_LoadsDataFromFile()
        {
            // Arrange
            var fileMock = new Mock<IFileWrapper>();
            var dataService = new DataAccessService(fileMock.Object);
            string filePath = "test.json";

            // Define the JSON data that should be read from the file
            string jsonData = "{\"StudentId\": 1, \"Name\": \"Alice\", \"LAstName\": \"Smith\"}";

            // Set up the behavior for File.Exists and File.ReadAllText methods
            fileMock.Setup(x => x.Exists(filePath)).Returns(true);
            fileMock.Setup(x => x.ReadAllText(filePath)).Returns(jsonData);

            // Act
            var loadedData = dataService.LoadData<Student>(filePath);

            // Assert
            // Verify that the File methods were called with the expected arguments
            fileMock.Verify(x => x.Exists(filePath), Times.Once);
            fileMock.Verify(x => x.ReadAllText(filePath), Times.Once);

            // Add assertions to verify that loadedData matches your expectations.
            Assert.IsNotNull(loadedData);
            Assert.AreEqual(1, loadedData.StudentId);
            Assert.AreEqual("Alice", loadedData.Name);
            Assert.AreEqual("Smith", loadedData.LastName);
            // Add more assertions as needed.
        }
    }
}