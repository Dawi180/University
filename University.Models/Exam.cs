using System;
using System.Collections.Generic;

namespace University.Models
{
    public class Exam
    {
        public long ExamId { get; set; } = 0;
        public string CourseCode { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.MinValue;
        public TimeSpan StartTime { get; set; } = TimeSpan.Zero;
        public TimeSpan EndTime { get; set; } = TimeSpan.Zero;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Professor { get; set; } = string.Empty;
    }
}
