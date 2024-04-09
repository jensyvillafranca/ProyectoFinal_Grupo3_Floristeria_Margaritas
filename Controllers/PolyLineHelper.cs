using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers
{
    public class PolyLineHelper
    {
        public static List<Location> Decode(string encodedPoints)
        {
            if (string.IsNullOrWhiteSpace(encodedPoints))
                return null;

            var poly = new List<Location>();
            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            try
            {
                while (index < polylinechars.Length)
                {
                    // Calculating next latitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length)
                        break;

                    currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                    // Calculating next longitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length && next5bits >= 32)
                        break;

                    currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                    Location p = new Location(Convert.ToDouble(currentLat) / 1E5, Convert.ToDouble(currentLng) / 1E5);
                    poly.Add(p);
                }
            }
            catch (Exception ex)
            {
                // Log the error
            }
            return poly;
        }
    }
}
