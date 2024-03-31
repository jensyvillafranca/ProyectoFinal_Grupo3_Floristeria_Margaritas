using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class notificacionEstadoModel
    {
        public int idnotificacionpedido { get; set; }
        public string? titulo { get; set; }
        public string? cuerpo { get; set; }
        public int idpedido { get; set; }
        public DateTime created { get; set; }
        public int idestadopedido { get; set; }
        public int lectura { get; set; }
        public int idcliente { get; set; }
    }
}
