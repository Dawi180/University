using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Interfaces
{
    public interface IStudentOrganizationStudent
    {
        long StudentOrganizationId { get; set; }
        long StudentId { get; set; }
       // IStudentOrganization StudentOrganization { get; set; }
       // IStudent Student { get; set; }
    }
}
