using University.Interfaces;
using System;

namespace University.Services
{
    public class ValidationService : IValidationService
    {
        public bool IsValidPESEL(string pesel)
        {
            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            bool result = false;
            if (pesel.Length == 11)
            {
                int controlSum = CalculateControlSum(pesel, weights);
                int controlNum = controlSum % 10;
                controlNum = 10 - controlNum;
                if (controlNum == 10)
                {
                    controlNum = 0;
                }
                int lastDigit = int.Parse(pesel[pesel.Length - 1].ToString());
                result = controlNum == lastDigit;
            }
            return result;
        }

        public bool IsValidDateOfBirth(DateTime? dateOfBirth)
        {
            // Define a valid date range (e.g., not more than 150 years in the past and not in the future)
            DateTime minValidDate = DateTime.Now.AddYears(-150);
            DateTime maxValidDate = DateTime.Now;

            // Check if dateOfBirth is null
            if (!dateOfBirth.HasValue)
            {
                return false;
            }

            // Check if dateOfBirth is within the valid range
            return (dateOfBirth.Value >= minValidDate && dateOfBirth.Value <= maxValidDate);
        }

        private int CalculateControlSum(string input, int[] weights, int offset = 0)
        {
            int controlSum = 0;
            for (int i = 0; i < input.Length - 1; i++)
            {
                controlSum += weights[i + offset] * int.Parse(input[i].ToString());
            }
            return controlSum;
        }
    }
}
