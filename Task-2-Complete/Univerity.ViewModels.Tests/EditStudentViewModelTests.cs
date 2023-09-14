using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using University.Interfaces;
using University.Models;
using University.Services;
using University.ViewModels;

namespace University.ViewModels.Tests
{
    [TestClass]
    public class EditStudentViewModelTests
    {
        [TestMethod]
        public void PESELValidation_ValidPESEL_ReturnsEmptyString()
        {
            // Arrange
            var validationServiceMock = new Mock<IValidationService>();
            validationServiceMock.Setup(vs => vs.IsValidPESEL(It.IsAny<string>())).Returns(true);
            var viewModel = new EditStudentViewModel(null, null, validationServiceMock.Object);

            // Act
            var result = viewModel["PESEL"];

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void PESELValidation_InvalidPESEL_ReturnsErrorMessage()
        {
            // Arrange
            var validationServiceMock = new Mock<IValidationService>();
            validationServiceMock.Setup(vs => vs.IsValidPESEL(It.IsAny<string>())).Returns(false);
            var viewModel = new EditStudentViewModel(null, null, validationServiceMock.Object);

            // Act
            var result = viewModel["PESEL"];

            // Assert
            Assert.AreEqual("PESEL is Invalid", result);
        }

        [TestMethod]
        public void BirthDateValidation_ValidDate_ReturnsEmptyString()
        {
            // Arrange
            var validationServiceMock = new Mock<IValidationService>();
            validationServiceMock.Setup(vs => vs.IsValidDateOfBirth(It.IsAny<DateTime?>())).Returns(true);
            var viewModel = new EditStudentViewModel(null, null, validationServiceMock.Object);

            // Act
            var result = viewModel["BirthDate"];

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void BirthDateValidation_InvalidDate_ReturnsErrorMessage()
        {
            // Arrange
            var validationServiceMock = new Mock<IValidationService>();
            validationServiceMock.Setup(vs => vs.IsValidDateOfBirth(It.IsAny<DateTime?>())).Returns(false);
            var viewModel = new EditStudentViewModel(null, null, validationServiceMock.Object);

            // Act
            var result = viewModel["BirthDate"];

            // Assert
            Assert.AreEqual("BirthDate is Invalid", result);
        }

        
    }
}
