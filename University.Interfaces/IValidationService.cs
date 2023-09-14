using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Interfaces
{
    public interface IValidationService
    {
        bool IsValidPESEL(string pesel);
        bool IsValidDateOfBirth(DateTime? dateOfBirth);
    }
}
