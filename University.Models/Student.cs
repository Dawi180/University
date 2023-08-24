using System;
using System.Collections.Generic;

namespace University.Models
{
    public class Student
    {
        public long StudentId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PESEL { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; } = null;
        // 1.Add four choces new properties to the Student entity:
        public string Gender { get; set; } = string.Empty;  // New property: Gender
        public string PlaceOfBirth { get; set; } = string.Empty;  // New property: Place of Birth
        public string PlaceOfResidence { get; set; } = string.Empty;  // New property: Place of Residence
        public string AddressLine1 { get; set; } = string.Empty;  // New property: Address Line 1
        public string AddressLine2 { get; set; } = string.Empty;  // New property: Address Line 2
        public string PostalCode { get; set; } = string.Empty;  // New property: Postal Code
        public virtual ICollection<Subject>? Subjects { get; set; } = null;
    }
}
