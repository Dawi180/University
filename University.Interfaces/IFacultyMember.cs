using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Interfaces
{
    public interface IFacultyMember
    {
         long FacultyId { get; set; }
         string Name { get; set; } 
         int Age { get; set; }
         string Gender { get; set; } 
         string Department { get; set; } 
         string Position { get; set; } 
         string Email { get; set; } 
         string OfficeRoomNumber { get; set; }
        //ICollection<ICourse>? Courses { get; set; } = null;

    }
}
