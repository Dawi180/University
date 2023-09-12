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
        string Gender { get; set; }  // New property: Gender
        string PlaceOfBirth { get; set; }  // New property: Place of Birth
        string PlaceOfResidence { get; set; }  // New property: Place of Residence
        string AddressLine1 { get; set; }  // New property: Address Line 1
        string AddressLine2 { get; set; }  // New property: Address Line 2
        string PostalCode { get; set; }  // New property: Postal Code
    }
}
