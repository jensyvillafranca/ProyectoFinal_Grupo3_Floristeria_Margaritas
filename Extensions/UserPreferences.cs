/*
 * Descripción:
 * Este código define una clase estática llamada UserPreferences en el espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions.
 * Esta clase proporciona un método estático Logout() para cerrar sesión del usuario actual.
 * Dependiendo del tipo de usuario, elimina las preferencias almacenadas correspondientes.
 * También contiene un método privado RemovePreference() para eliminar una preferencia específica.
 */

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
            if(Config.Config.tipoUsuario == 1)
            {
                RemovePreference("clienteID");
                RemovePreference("userID");
                RemovePreference("usuario");
                RemovePreference("tipoUsuario");
                RemovePreference("stayLogged");
            }
            else if(Config.Config.tipoUsuario == 3)
            {
                RemovePreference("repartidorID");
                RemovePreference("userID");
                RemovePreference("usuario");
                RemovePreference("tipoUsuario");
                RemovePreference("stayLogged");
            }    
        }

        private static void RemovePreference(string key)
        {
            SecureStorage.Remove(key);
        }
    }
}
