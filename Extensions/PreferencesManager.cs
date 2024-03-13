using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions
{
    public class PreferencesManager
    {
        // Guarda data tipo string
        public static void SaveString(string key, string value)
        {
            SecureStorage.SetAsync(key, value);
        }

        // Obtiene data tipo string
        public static string GetString(string key)
        {
            return SecureStorage.GetAsync(key).Result;
        }

        // Guarda data tipo int
        public static void SaveInt(string key, int value)
        {
            SecureStorage.SetAsync(key, value.ToString());
        }

        // Obtiene data tipo int
        public static int GetInt(string key)
        {
            string stringValue = SecureStorage.GetAsync(key).Result;
            int.TryParse(stringValue, out int value);
            return value;
        }

        // Remueve data
        public static void Remove(string key)
        {
            SecureStorage.Remove(key);
        }
    }
}
