using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Interfaces
{
    public interface IStudentOrganization
    {
        long OrgId { get; set; }
        string Name { get; set; } 
        string Advisor { get; set; }
        string President { get; set; } 
        string Description { get; set; } 
        string MeetingSchedule { get; set; } 
       // ICollection<IStudentOrganization>? Membership { get; set; }
       // ICollection<IStudent>? Students { get; set; }
        string Email { get; set; } 
      //  ICollection<IStudentOrganizationStudent>? StudentOrganizationStudents { get; set; }
    }
}
