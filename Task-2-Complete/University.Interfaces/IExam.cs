using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Interfaces
{
    public interface IExam
    {
         long ExamId { get; set; }         
        string CourseCode { get; set; }
        DateTime? Date { get; set; }
        TimeSpan? StartTime { get; set; }
        TimeSpan? EndTime { get; set; }
        string Location { get; set; }
        string Description { get; set; }
        string Professor { get; set; }
       // ICollection<ICourseExam> CourseExams { get; set; }
        
    }
}
