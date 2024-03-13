using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions
{
    public static class UserPreferences
    {
        public static void Logout()
        {
            RemovePreference("clienteID");
            RemovePreference("userID");
            RemovePreference("usuario");
            RemovePreference("tipoUsuario");
            RemovePreference("stayLogged");
        }

        private static void RemovePreference(string key)
        {
            SecureStorage.Remove(key);
        }
    }
}
