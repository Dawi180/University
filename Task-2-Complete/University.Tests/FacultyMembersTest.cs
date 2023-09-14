using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using University.Data;
using University.Models;

namespace University.Tests
{
    [TestClass]
    public class FacultyTests
    {
        private DbContextOptions<UniversityContext> _options;

        [TestInitialize()]
        public void Initialize()
        {
            _options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "UniversityTestDB")
                .Options;
            SeedTestDB();
        }

        private void SeedTestDB()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                context.Database.EnsureDeleted();
                List<FacultyMember> faculties = new List<FacultyMember>
                {
                    new FacultyMember
                    {
                        FacultyId = 1,
                        Name = "John Doe",
                        Age = 35,
                        Gender = "Male",
                        Department = "Computer Science",
                        Position = "Professor",
                        Email = "john.doe@example.com",
                        OfficeRoomNumber = "Room 101"
                    },
                    new FacultyMember
                    {
                        FacultyId = 2,
                        Name = "Jane Smith",
                        Age = 42,
                        Gender = "Female",
                        Department = "Mathematics",
                        Position = "Associate Professor",
                        Email = "jane.smith@example.com",
                        OfficeRoomNumber = "Room 202"
                    },
                };

                context.FacultyMembers.AddRange(faculties);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void GetFacultyFromDatabase()
        {
            using (var context = new UniversityContext(_options))
            {
                // Act
                var faculty = context.FacultyMembers.FirstOrDefault(f => f.Name == "John Doe");

                // Assert
                Assert.IsNotNull(faculty);
                Assert.AreEqual("Computer Science", faculty.Department);
            }
        }

        [TestMethod]
        public void UpdateFacultyInDatabase()
        {
            using (var context = new UniversityContext(_options))
            {
                // Arrange
                var facultyToUpdate = context.FacultyMembers.FirstOrDefault(f => f.Name == "John Doe");
                Assert.IsNotNull(facultyToUpdate);
                facultyToUpdate.OfficeRoomNumber = "Room 105";

                // Act
                context.SaveChanges();

                // Assert
                var updatedFaculty = context.FacultyMembers.FirstOrDefault(f => f.Name == "John Doe");
                Assert.IsNotNull(updatedFaculty);
                Assert.AreEqual("Room 105", updatedFaculty.OfficeRoomNumber);
            }
        }

        [TestMethod]
        public void DeleteFacultyFromDatabase()
        {
            using (var context = new UniversityContext(_options))
            {
                // Arrange
                var facultyToDelete = context.FacultyMembers.FirstOrDefault(f => f.Name == "Jane Smith");
                Assert.IsNotNull(facultyToDelete);

                // Act
                context.FacultyMembers.Remove(facultyToDelete);
                context.SaveChanges();

                // Assert
                var deletedFaculty = context.FacultyMembers.FirstOrDefault(f => f.Name == "Jane Smith");
                Assert.IsNull(deletedFaculty);
            }
        }
    }
}
