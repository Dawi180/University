using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Models
{
   
    public class StudentOrganization
    {
        public long OrgId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Advisor { get; set; } = string.Empty;
        public string President { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MeetingSchedule { get; set; } = string.Empty;
        public virtual ICollection<StudentOrganization>? Membership { get; set; } = null;
        public virtual ICollection<Student>? Students { get; set; } = null;
        public string Email { get; set; } = string.Empty;
    }
}
