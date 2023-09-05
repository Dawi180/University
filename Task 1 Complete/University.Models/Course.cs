using System;
using System.Collections.Generic;

namespace University.Models
{
    public class Course
    {
        public long CourseId { get; set; } = 0;
        public string CourseCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Credits { get; set; } = 0;
        public string Department { get; set; } = string.Empty;
        public bool IsSelected { get; set; } = false;
        public virtual ICollection<Student>? Students { get; set; } = null;
        public virtual ICollection<Course>? Prerequisite { get; set; } = null;
        public virtual ICollection<CourseExam> CourseExams { get; set; } = new List<CourseExam>();

    }
}
