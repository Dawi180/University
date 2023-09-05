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
    public class ExamsTest
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
                List<Exam> exams = new List<Exam>
                {
                    new Exam
                    {
                        ExamId = 1,
                        CourseCode = "COURSE1",
                        Date = new DateTime(2023, 9, 10),
                        StartTime = TimeSpan.FromHours(9),
                        EndTime = TimeSpan.FromHours(12),
                        Location = "Room 101",
                        Description = "Midterm Exam",
                        Professor = "Prof. Smith"
                    },
                    new Exam
                    {
                        ExamId = 2,
                        CourseCode = "COURSE2",
                        Date = new DateTime(2023, 9, 15),
                        StartTime = TimeSpan.FromHours(14),
                        EndTime = TimeSpan.FromHours(17),
                        Location = "Room 202",
                        Description = "Final Exam",
                        Professor = "Prof. Johnson"
                    },
                };

                context.Exams.AddRange(exams);
                context.SaveChanges();
            }
        }
        [TestMethod]
        public void GetExamFromDatabase()
        {
            using (var context = new UniversityContext(_options))
            {
                // Act
                var exam = context.Exams.FirstOrDefault(e => e.CourseCode == "COURSE1");

                // Assert
                Assert.IsNotNull(exam);
                Assert.AreEqual("Midterm Exam", exam.Description);
            }
        }
        [TestMethod]
        public void UpdateExamInDatabase()
        {
            using (var context = new UniversityContext(_options))
            {
                // Arrange
                var examToUpdate = context.Exams.FirstOrDefault(e => e.CourseCode == "COURSE1");
                Assert.IsNotNull(examToUpdate);
                examToUpdate.Location = "Room 105";

                // Act
                context.SaveChanges();

                // Assert
                var updatedExam = context.Exams.FirstOrDefault(e => e.CourseCode == "COURSE1");
                Assert.IsNotNull(updatedExam);
                Assert.AreEqual("Room 105", updatedExam.Location);
            }
        }
        [TestMethod]
        public void DeleteExamFromDatabase()
        {
            using (var context = new UniversityContext(_options))
            {
                // Arrange
                var examToDelete = context.Exams.FirstOrDefault(e => e.CourseCode == "COURSE2");
                Assert.IsNotNull(examToDelete);

                // Act
                context.Exams.Remove(examToDelete);
                context.SaveChanges();

                // Assert
                var deletedExam = context.Exams.FirstOrDefault(e => e.CourseCode == "COURSE2");
                Assert.IsNull(deletedExam);
            }
        }


    }
}