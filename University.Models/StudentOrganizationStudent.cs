using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Models
{
    public class StudentOrganizationStudent
    {
        public long StudentOrganizationId { get; set; }
        public virtual StudentOrganization? StudentOrganization { get; set; }

        public long StudentId { get; set; }
        public virtual Student? Student { get; set; }
    }
}
