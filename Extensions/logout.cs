using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions
{
    public class logout
    {
        public static async Task<bool> PerformLogoutAsync(ApiService _apiService)
        {
            
            var data = new
            {
                idcliente = Config.Config.activeUserId
            };

            

            bool isSuccess = await _apiService.PostSuccessAsync("logoutCliente.php", data);

            if (isSuccess)
            {
                UserPreferences.Logout();
            }

            return isSuccess;
        }
    }
}
