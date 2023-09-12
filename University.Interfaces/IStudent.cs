using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Interfaces
{
    public interface IStudent
    {
        long StudentId { get; set; }
        string Name { get; set; }
        string LastName { get; set; }
        string PESEL { get; set; }
        DateTime? BirthDate { get; set; }
        string Gender { get; set; }
        string PlaceOfBirth { get; set; }
        string PlaceOfResidence { get; set; }
        string AddressLine1 { get; set; }
        string AddressLine2 { get; set; }
        string PostalCode { get; set; }
        //ICollection<ICourse>? Courses { get; set; }
        //ICollection<IStudentOrganization>? StudentOrganizations { get; set; } 
        //ICollection<IStudentOrganizationStudent>? StudentOrganizationStudents { get; set; }
    }
}
