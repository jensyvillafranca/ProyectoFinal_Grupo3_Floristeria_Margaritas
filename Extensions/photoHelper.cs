using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Utilities
{
    public static class photoHelper
    {
        public static string GetImg64(FileResult photo)
        {
            if (photo != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Stream stream = photo.OpenReadAsync().Result;
                    stream.CopyTo(ms);
                    byte[] data = ms.ToArray();

                    String Base64 = Convert.ToBase64String(data);

                    return Base64;
                } 
            }
            return null;
        }

        public static byte[] GetImageArrayFromBase64(string base64String)
        {
            if (!string.IsNullOrEmpty(base64String))
            {
                try
                {
                    byte[] data = Convert.FromBase64String(base64String);
                    return data;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting base64 string to byte array: {ex.Message}");
                }
            }
            return null;
        }
    }
}
