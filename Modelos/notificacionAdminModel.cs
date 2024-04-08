using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class notificacionAdminModel
    {
        public int idnotificacion { get; set; }
        public string? titulo { get; set; }
        public string? cuerpo { get; set; }
        public int idsolicitud { get; set; }
        public DateTime created { get; set; }
        public int lectura { get; set; }
    }
}
