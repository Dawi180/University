using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Interfaces
{
    public interface ICourse
    {
        long CourseId { get; set; }
        string CourseCode { get; set; } 
        string Title { get; set; } 
        string Instructor { get; set; }
        string Schedule { get; set; } 
        string Description { get; set; }
        int Credits { get; set; } 
        string Department { get; set; } 
        bool IsSelected { get; set; } 
       // ICollection<IStudent>? Students { get; set; } 
       // ICollection<ICourse>? Prerequisite { get; set; } 
        //ICollection<ICourseExam> CourseExams { get; set; } 
    }
}
