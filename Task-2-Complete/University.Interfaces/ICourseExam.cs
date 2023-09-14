using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Interfaces
{
    public interface ICourseExam
    {
        long CourseExamId { get; set; }
        long CourseId { get; set; }
        long ExamId { get; set; }
       // ICourse Course { get; set; }
       // IExam Exam { get; set; }
    }
}
