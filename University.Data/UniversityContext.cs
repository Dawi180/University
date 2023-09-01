using University.Models;
using Microsoft.EntityFrameworkCore;

namespace University.Data
{
    public class UniversityContext : DbContext
    {
        public UniversityContext()
        {
        }

        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<FacultyMember> FacultyMembers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("UniversityDb");
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().Ignore(s => s.IsSelected);

            modelBuilder.Entity<Student>().HasData(
                new Student { StudentId = 1, Name = "Wieńczysław", LastName = "Nowakowicz", PESEL = "PESEL1", BirthDate = new DateTime(1987, 05, 22) },
                new Student { StudentId = 2, Name = "Stanisław", LastName = "Nowakowicz", PESEL = "PESEL2", BirthDate = new DateTime(2019, 06, 25) },
                new Student { StudentId = 3, Name = "Eugenia", LastName = "Nowakowicz", PESEL = "PESEL3", BirthDate = new DateTime(2021, 06, 08) });

            modelBuilder.Entity<Course>().HasData(
                new Course { CourseId = 1, CourseCode = "kod kursu", Title = "tytuł kursu", Instructor = "prowadzący kurs", Schedule = "harmonogram kursu", Description = "opis kursu", Credits = 10, Department = "wydział, do którego przynależy kurs" },
                new Course { CourseId = 2, CourseCode = "kod kursu2", Title = "tytuł kursu2", Instructor = "prowadzący kurs2", Schedule = "harmonogram kursu2", Description = "opis kursu2", Credits = 10, Department = "wydział, do którego przynależy kurs" }

            );
            modelBuilder.Entity<FacultyMember>().HasData(
                new FacultyMember { FacultyId = 1, Name = "Imię", Age = 22, Gender = "Gender", Department = "Department", Position = "Position", Email = "Email", OfficeRoomNumber = "OffieceRoomNumber" }
            );
            modelBuilder.Entity<FacultyMember>().HasKey(fm => fm.FacultyId);
        }
    }
}
