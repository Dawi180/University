using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using University.Data;
using University.Interfaces;
using University.Models;
using University.Services;
using University.ViewModels;

namespace University.Tests;

[TestClass]
public class StudentsTest
{
    private IDialogService _dialogService;
    private DbContextOptions<UniversityContext> _options;

    [TestInitialize()]
    public void Initialize()
    {
        _options = new DbContextOptionsBuilder<UniversityContext>()
            .UseInMemoryDatabase(databaseName: "UniversityTestDB")
            .Options;
        SeedTestDB();
        _dialogService = new DialogService();
    }

    private void SeedTestDB()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            context.Database.EnsureDeleted();
            List<Student> students = new List<Student>
            {
                new Student { StudentId = 1, Name = "Wieñczys³aw", LastName = "Nowakowicz", PESEL="PESEL1", BirthDate = new DateTime(1987, 05, 22), Gender = "Male",
                        PlaceOfBirth = "Warsaw",
                        PlaceOfResidence = "Krakow",
                        AddressLine1 = "123 Main St",
                        AddressLine2 = "Apt 45",
                        PostalCode = "12345"},
                new Student { StudentId = 2, Name = "Stanis³aw", LastName = "Nowakowicz", PESEL = "PESEL2", BirthDate = new DateTime(2019, 06, 25), Gender = "Male",
                        PlaceOfBirth = "Warsaw",
                        PlaceOfResidence = "Krakow",
                        AddressLine1 = "123 Main St",
                        AddressLine2 = "Apt 45",
                        PostalCode = "12345" },
                new Student { StudentId = 3, Name = "Eugenia", LastName = "Nowakowicz", PESEL = "PESEL3", BirthDate = new DateTime(2021, 06, 08), Gender = "Male",
                        PlaceOfBirth = "Warsaw",
                        PlaceOfResidence = "Krakow",
                        AddressLine1 = "123 Main St",
                        AddressLine2 = "Apt 45",
                        PostalCode = "12345" }
            };
            List<Course> courses = new List<Course>
            {
               
                new Course { CourseId = 5, CourseCode = "kod kursu", Title = "tytu³ kursu", Instructor = "prowadz¹cy kurs", Schedule = "harmonogram kursu", Description = "opis kursu", Credits = 10, Department = "wydzia³, do którego przynale¿y kurs" }

            };
           
            context.Students.AddRange(students);
            context.Courses.AddRange(courses);
            context.SaveChanges();
        }
    }

    [TestMethod]
    public void Show_all_students()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            StudentsViewModel studentsViewModel = new StudentsViewModel(context, _dialogService);
            bool hasData = studentsViewModel.Students.Any();
            Assert.IsTrue(hasData);
        }
    }

    [TestMethod]
    public void Add_student_without_courses()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            AddStudentViewModel addStudentViewModel = new AddStudentViewModel(context, _dialogService)
            {
                Name = "John",
                LastName = "Doe",
                PESEL = "67111994116",
                BirthDate = new DateTime(1967, 12, 06),
                Gender = "Male",
                PlaceOfBirth = "New York",
                PlaceOfResidence = "Los Angeles",
                AddressLine1 = "456 Elm St",
                AddressLine2 = "Unit 12",
                PostalCode = "54321"
            };
            addStudentViewModel.Save.Execute(null);

            bool newStudentExists = context.Students.Any(s => s.Name == "John" && s.LastName == "Doe" && s.PESEL == "67111994116");
            Assert.IsTrue(newStudentExists);
        }
    }

    [TestMethod]
    public void Add_student_with_courses()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            Random random = new Random();
            int toSkip = random.Next(0, context.Courses.Count());
            Course course = context.Courses.OrderBy(x => x.CourseCode).Skip(toSkip).Take(1).FirstOrDefault();
            course.IsSelected = true;

            AddStudentViewModel addStudentViewModel = new AddStudentViewModel(context, _dialogService)
            {
                Name = "John",
                LastName = "Doe II",
                PESEL = "67111994116",
                BirthDate = new DateTime(1967, 12, 06),
                Gender = "Male",
                PlaceOfBirth = "Chicago",
                PlaceOfResidence = "Miami",
                AddressLine1 = "789 Oak St",
                AddressLine2 = "Apt 34",
                PostalCode = "67890",
                AssignedCourses = new ObservableCollection<Course>
                    {
                        course
                    }
            };
            addStudentViewModel.Save.Execute(null);

            bool newStudentExists = context.Students.Any(s => s.Name == "John" && s.LastName == "Doe II" && s.PESEL == "67111994116" && s.Courses.Any());
            Assert.IsTrue(newStudentExists);
        }
    }

    [TestMethod]
    public void Add_student_without_name()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            AddStudentViewModel addStudentViewModel = new AddStudentViewModel(context, _dialogService)
            {
                LastName = "Doe III",
                PESEL = "67111994116",
                BirthDate = new DateTime(1967, 12, 06),
                Gender = "Female",
                PlaceOfBirth = "London",
                PlaceOfResidence = "Paris",
                AddressLine1 = "987 Pine St",
                AddressLine2 = "Suite 5",
                PostalCode = "54321"
            };
            addStudentViewModel.Save.Execute(null);

            bool newStudentExists = context.Students.Any(s => s.LastName == "Doe III" && s.PESEL == "67111994116");
            Assert.IsFalse(newStudentExists);
        }
    }
    [TestMethod]
    public void Add_student_without_PESEL()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            AddStudentViewModel addStudentViewModel = new AddStudentViewModel(context, _dialogService)
            {
                Name = "John",
                LastName = "Doe IV",
                BirthDate = new DateTime(1967, 12, 06),
                Gender = "Male",
                PlaceOfBirth = "Berlin",
                PlaceOfResidence = "Munich",
                AddressLine1 = "456 Oak St",
                AddressLine2 = "Apt 78",
                PostalCode = "34567"
            };
            addStudentViewModel.Save.Execute(null);

            bool newStudentExists = context.Students.Any(s => s.Name == "John" && s.LastName == "Doe IV");
            Assert.IsFalse(newStudentExists);
        }
    }

    [TestMethod]
    public void Add_student_with_invalid_PESEL()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            AddStudentViewModel addStudentViewModel = new AddStudentViewModel(context, _dialogService)
            {
                Name = "John",
                LastName = "Doe V",
                PESEL = "12345", // Invalid PESEL
                BirthDate = new DateTime(1967, 12, 06),
                Gender = "Male",
                PlaceOfBirth = "Madrid",
                PlaceOfResidence = "Barcelona",
                AddressLine1 = "789 Elm St",
                AddressLine2 = "Unit 56",
                PostalCode = "78901"
            };
            addStudentViewModel.Save.Execute(null);

            bool newStudentExists = context.Students.Any(s => s.Name == "John" && s.LastName == "Doe V");
            Assert.IsFalse(newStudentExists);
        }
    }

    [TestMethod]
    public void Add_student_with_blank_name_and_last_name()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            AddStudentViewModel addStudentViewModel = new AddStudentViewModel(context, _dialogService)
            {
                PESEL = "67111994116",
                BirthDate = new DateTime(1967, 12, 06),
                Gender = "Female",
                PlaceOfBirth = "Vienna",
                PlaceOfResidence = "Salzburg",
                AddressLine1 = "123 Maple St",
                AddressLine2 = "Suite 3",
                PostalCode = "45678"
            };
            addStudentViewModel.Save.Execute(null);

            bool newStudentExists = context.Students.Any(s => s.PESEL == "67111994116");
            Assert.IsFalse(newStudentExists);
        }
    }

    [TestMethod]
    public void Add_student_with_invalid_postal_code()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            AddStudentViewModel addStudentViewModel = new AddStudentViewModel(context, _dialogService)
            {
                Name = "John",
                LastName = "Doe VI",
                PESEL = "67111994116",
                BirthDate = new DateTime(1967, 12, 06),
                Gender = "Male",
                PlaceOfBirth = "Amsterdam",
                PlaceOfResidence = "Rotterdam",
                AddressLine1 = "987 Birch St",
                AddressLine2 = "Apt 21",
                PostalCode = "123", // Invalid postal code
            };
            addStudentViewModel.Save.Execute(null);

            bool newStudentExists = context.Students.Any(s => s.Name == "John" && s.LastName == "Doe VI" && s.PESEL == "67111994116");
            Assert.IsFalse(newStudentExists);
        }
    }
    [TestMethod]
    public void Add_student_with_invalid_birth_date()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            AddStudentViewModel addStudentViewModel = new AddStudentViewModel(context, _dialogService)
            {
                Name = "John",
                LastName = "Doe IX",
                PESEL = "67111994116",
                BirthDate = new DateTime(2200, 01, 01), // Invalid birth date in the future
                Gender = "Male",
                PlaceOfBirth = "Paris",
                PlaceOfResidence = "Berlin",
                AddressLine1 = "789 Maple St",
                AddressLine2 = "Suite 8",
                PostalCode = "23456"
            };
            addStudentViewModel.Save.Execute(null);

            bool newStudentExists = context.Students.Any(s => s.Name == "John" && s.LastName == "Doe IX" && s.PESEL == "67111994116");
            Assert.IsFalse(newStudentExists);
        }
    }

    [TestMethod]
    public void Add_student_with_missing_required_properties()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            AddStudentViewModel addStudentViewModel = new AddStudentViewModel(context, _dialogService)
            {
                // Missing required properties: Name, LastName, PESEL
                BirthDate = new DateTime(1990, 05, 15),
                Gender = "Male",
                PlaceOfBirth = "Munich",
                PlaceOfResidence = "Vienna",
                AddressLine1 = "123 Elm St",
                AddressLine2 = "Apt 15",
                PostalCode = "34567"
            };
            addStudentViewModel.Save.Execute(null);

            bool newStudentExists = context.Students.Any(s => s.BirthDate == new DateTime(1990, 05, 15));
            Assert.IsFalse(newStudentExists);
        }
    }

    [TestMethod]
    public void Add_student_with_blank_PESEL()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            AddStudentViewModel addStudentViewModel = new AddStudentViewModel(context, _dialogService)
            {
                Name = "John",
                LastName = "Doe XIII",
                // Blank PESEL
                BirthDate = new DateTime(1990, 08, 12),
                Gender = "Male",
                PlaceOfBirth = "Paris",
                PlaceOfResidence = "London",
                AddressLine1 = "456 Oak St",
                AddressLine2 = "Suite 12",
                PostalCode = "34567"
            };
            addStudentViewModel.Save.Execute(null);

            bool newStudentExists = context.Students.Any(s => s.Name == "John" && s.LastName == "Doe XIII");
            Assert.IsFalse(newStudentExists);
        }
    }

    [TestMethod]
    public void Add_student_with_blank_birth_date()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            AddStudentViewModel addStudentViewModel = new AddStudentViewModel(context, _dialogService)
            {
                Name = "John",
                LastName = "Doe XIV",
                PESEL = "67111994116",
                // Blank birth date
                Gender = "Male",
                PlaceOfBirth = "Amsterdam",
                PlaceOfResidence = "Rotterdam",
                AddressLine1 = "987 Pine St",
                AddressLine2 = "Apt 28",
                PostalCode = "23456"
            };
            addStudentViewModel.Save.Execute(null);

            bool newStudentExists = context.Students.Any(s => s.Name == "John" && s.LastName == "Doe XIV" && s.PESEL == "67111994116");
            Assert.IsFalse(newStudentExists);
        }
    }
    [TestMethod]
    public void Add_student_with_blank_gender()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            AddStudentViewModel addStudentViewModel = new AddStudentViewModel(context, _dialogService)
            {
                Name = "John",
                LastName = "Doe XV",
                PESEL = "67111994116",
                BirthDate = new DateTime(1987, 03, 25),
                // Blank gender
                PlaceOfBirth = "Rome",
                PlaceOfResidence = "Florence",
                AddressLine1 = "123 Main St",
                AddressLine2 = "Apt 10",
                PostalCode = "45678"
            };
            addStudentViewModel.Save.Execute(null);

            bool newStudentExists = context.Students.Any(s => s.Name == "John" && s.LastName == "Doe XV" && s.PESEL == "67111994116");
            Assert.IsFalse(newStudentExists);
        }
    }

    [TestMethod]
    public void Add_student_with_blank_place_of_birth()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            AddStudentViewModel addStudentViewModel = new AddStudentViewModel(context, _dialogService)
            {
                Name = "John",
                LastName = "Doe XVI",
                PESEL = "67111994116",
                BirthDate = new DateTime(1990, 12, 15),
                Gender = "Male",
                // Blank place of birth
                PlaceOfResidence = "Barcelona",
                AddressLine1 = "456 Elm St",
                AddressLine2 = "Unit 15",
                PostalCode = "34567"
            };
            addStudentViewModel.Save.Execute(null);

            bool newStudentExists = context.Students.Any(s => s.Name == "John" && s.LastName == "Doe XVI" && s.PESEL == "67111994116");
            Assert.IsFalse(newStudentExists);
        }
    }

    [TestMethod]
    public void Add_student_with_blank_place_of_residence()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            AddStudentViewModel addStudentViewModel = new AddStudentViewModel(context, _dialogService)
            {
                Name = "John",
                LastName = "Doe XVII",
                PESEL = "67111994116",
                BirthDate = new DateTime(1985, 05, 20),
                Gender = "Female",
                PlaceOfBirth = "Madrid",
                // Blank place of residence
                AddressLine1 = "789 Oak St",
                AddressLine2 = "Apt 25",
                PostalCode = "23456"
            };
            addStudentViewModel.Save.Execute(null);

            bool newStudentExists = context.Students.Any(s => s.Name == "John" && s.LastName == "Doe XVII" && s.PESEL == "67111994116");
            Assert.IsFalse(newStudentExists);
        }
    }

    [TestMethod]
    public void Add_student_with_blank_address_lines_and_postal_code()
    {
        using UniversityContext context = new UniversityContext(_options);
        {
            AddStudentViewModel addStudentViewModel = new AddStudentViewModel(context, _dialogService)
            {
                Name = "John",
                LastName = "Doe XVIII",
                PESEL = "67111994116",
                BirthDate = new DateTime(1978, 07, 10),
                Gender = "Male",
                PlaceOfBirth = "Vienna",
                PlaceOfResidence = "Salzburg",
                // Blank address lines and postal code
            };
            addStudentViewModel.Save.Execute(null);

            bool newStudentExists = context.Students.Any(s => s.Name == "John" && s.LastName == "Doe XVIII" && s.PESEL == "67111994116");
            Assert.IsFalse(newStudentExists);
        }
    }



}
