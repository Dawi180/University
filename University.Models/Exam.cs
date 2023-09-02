using System;
using System.Collections.Generic;

namespace University.Models
{
    public class Exam
    {
        public long ExamId { get; set; } = 0;
        public string CourseCode { get; set; } = string.Empty;
        public DateTime? Date { get; set; } = null;
        public TimeSpan? StartTime { get; set; } = null;
        public TimeSpan? EndTime { get; set; } = null;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Professor { get; set; } = string.Empty;
    }
}
