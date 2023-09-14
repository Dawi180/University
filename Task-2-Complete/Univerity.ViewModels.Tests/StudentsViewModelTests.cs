using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using University.Interfaces;
using University.Models;
using University.ViewModels;

namespace University.ViewModels.Tests
{
    [TestClass]
    public class StudentsViewModelTests
    {
        [TestMethod]
        public void AddNewStudentCommand_Executed_StudentsCollectionUpdated()
        {
            // Arrange
            var dataAccessServiceMock = new Mock<IDataAccessService>();
            var dialogServiceMock = new Mock<IDialogService>();
            var viewModel = new StudentsViewModel(dataAccessServiceMock.Object, dialogServiceMock.Object);
            var initialStudentCount = viewModel.Students?.Count ?? 0;

            // Act
            viewModel.AddNewStudent(null);

            // Assert
            Assert.AreEqual(initialStudentCount + 1, viewModel.Students?.Count);
        }

        [TestMethod]
        public void EditStudentCommand_Executed_StudentsCollectionUpdated()
        {
            // Arrange
            var dataAccessServiceMock = new Mock<IDataAccessService>();
            var dialogServiceMock = new Mock<IDialogService>();
            var viewModel = new StudentsViewModel(dataAccessServiceMock.Object, dialogServiceMock.Object);

            // Add a sample student to the collection
            var student = new Student { StudentId = 1, Name = "John", LastName = "Doe" };
            viewModel.Students = new ObservableCollection<IStudent> { student };

            // Act
            viewModel.EditStudent(1);

            // Assert
            // Verify that the EditStudent method updates the Students collection with the edited student.
            Assert.AreEqual("Edited", viewModel.Students?.First().Name);
        }

        [TestMethod]
        public void RemoveStudentCommand_Executed_StudentsCollectionUpdated()
        {
            // Arrange
            var dataAccessServiceMock = new Mock<IDataAccessService>();
            var dialogServiceMock = new Mock<IDialogService>();
            var viewModel = new StudentsViewModel(dataAccessServiceMock.Object, dialogServiceMock.Object);

            // Add a sample student to the collection
            var student = new Student { StudentId = 1, Name = "John", LastName = "Doe" };
            viewModel.Students = new ObservableCollection<IStudent> { student };

            dialogServiceMock.Setup(d => d.Show(It.IsAny<string>())).Returns(true);

            // Act
            viewModel.RemoveStudent(1);

            // Assert
            // Verify that the RemoveStudent method updates the Students collection by removing the student.
            Assert.AreEqual(0, viewModel.Students?.Count);
        }
    }
}
