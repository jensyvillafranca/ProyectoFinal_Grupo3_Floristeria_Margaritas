using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions
{
    public class AgeCalculator
    {
        public int CalculateAge(DateTime birthdate)
        {
            // Get current date
            DateTime currentDate = DateTime.Today;

            // Calculate age
            int age = currentDate.Year - birthdate.Year;

            // Check if the birthdate hasn't occurred yet this year
            if (birthdate.Date > currentDate.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}
