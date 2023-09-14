using Microsoft.VisualStudio.TestTools.UnitTesting;
using University.Models;

namespace University.Tests
{
    [TestClass]
    public class CourseTests
    {
        [TestMethod]
        public void CourseProperties_DefaultValues()
        {
            // Arrange
            var course = new Course();

            // Assert
            Assert.AreEqual(0, course.CourseId);
            Assert.AreEqual(string.Empty, course.CourseCode);
            Assert.AreEqual(string.Empty, course.Title);
            Assert.AreEqual(string.Empty, course.Instructor);
            Assert.AreEqual(string.Empty, course.Schedule);
            Assert.AreEqual(string.Empty, course.Description);
            Assert.AreEqual(0, course.Credits);
            Assert.AreEqual(string.Empty, course.Department);
            Assert.IsFalse(course.IsSelected);
            Assert.IsNull(course.Students);
            Assert.IsNull(course.Prerequisite);
        }

        [TestMethod]
        public void CourseProperties_SetValues()
        {
            // Arrange
            var course = new Course
            {
                CourseId = 1,
                CourseCode = "COURSE101",
                Title = "Introduction to Programming",
                Instructor = "John Smith",
                Schedule = "MWF 10:00 AM - 11:30 AM",
                Description = "An introductory programming course",
                Credits = 3,
                Department = "Computer Science",
                IsSelected = true,
                Students = null, // You can set this to a collection if needed
                Prerequisite = null, // You can set this to a collection if needed
            };

            // Assert
            Assert.AreEqual(1, course.CourseId);
            Assert.AreEqual("COURSE101", course.CourseCode);
            Assert.AreEqual("Introduction to Programming", course.Title);
            Assert.AreEqual("John Smith", course.Instructor);
            Assert.AreEqual("MWF 10:00 AM - 11:30 AM", course.Schedule);
            Assert.AreEqual("An introductory programming course", course.Description);
            Assert.AreEqual(3, course.Credits);
            Assert.AreEqual("Computer Science", course.Department);
            Assert.IsTrue(course.IsSelected);
            Assert.IsNull(course.Students);
            Assert.IsNull(course.Prerequisite);

        }

        [TestMethod]
        public void CourseProperties_InvalidCredits()
        {
            // Arrange
            var course = new Course
            {
                Credits = -1, // Negative credits value
            };

            // Assert
            Assert.AreEqual(0, course.Credits); // Should default to 0 for invalid value
        }

        [TestMethod]
        public void CourseProperties_NullValues()
        {
            // Arrange
            var course = new Course
            {
                CourseCode = null,
                Title = null,
                Instructor = null,
                Schedule = null,
                Description = null,
                Department = null,
            };

            // Assert
            Assert.AreEqual(string.Empty, course.CourseCode); // Should default to empty string for null values
            Assert.AreEqual(string.Empty, course.Title);
            Assert.AreEqual(string.Empty, course.Instructor);
            Assert.AreEqual(string.Empty, course.Schedule);
            Assert.AreEqual(string.Empty, course.Description);
            Assert.AreEqual(string.Empty, course.Department);
        }
    }
}
