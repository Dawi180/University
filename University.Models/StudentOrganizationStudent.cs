using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Interfaces;    

namespace University.Models
{
    public class StudentOrganizationStudent : IStudentOrganizationStudent
    {
        public long StudentOrganizationId { get; set; }
        public virtual StudentOrganization? StudentOrganization { get; set; }

        public long StudentId { get; set; }
        public virtual Student? Student { get; set; }
    }
}
