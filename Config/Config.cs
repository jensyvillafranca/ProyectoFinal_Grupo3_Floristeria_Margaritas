using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Config
{
    public static class Config
    {
        public static string GoogleApiKey = "AIzaSyB9XVzR_GxYa7YPvfLTz3hOZhLjx-WP89E";
        public static string BingMapsApiKey = "Au7sMtQzyQZRzuQ2krOIbZg8j2MGoHzzOJAmVym6vQjHq_BJ8a1YQGX3iCosFh8u";

        //ID de Cliente
        public static int activeUserId
        {
            get
            {
                int userId = PreferencesManager.GetInt("clienteID");
                return userId != 0 ? userId : -1;
            }
        }

        public static int activeRepartidorId
        {
            get
            {
                int userId = PreferencesManager.GetInt("repartidorID");
                return userId != 0 ? userId : -1;
            }
        }

        public static int tipoUsuario
        {
            get
            {
                int tipo = PreferencesManager.GetInt("tipoUsuario");
                return tipo != 0 ? tipo : -1;
            }
        }

        public static int stayLogged
        {
            get
            {
                int stayLogged = PreferencesManager.GetInt("stayLogged");
                return stayLogged != 0 ? stayLogged : -1;
            }
        }
    }
}
