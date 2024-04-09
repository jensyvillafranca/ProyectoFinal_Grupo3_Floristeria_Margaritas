using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Services {
    public static class WhatsAppService {
        public static async Task open_whatsapp(string phone_number) {
            var whatsapp_url = $"whatsapp://send?phone=+504{phone_number}";

            try {
                await Launcher.OpenAsync(whatsapp_url);
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
