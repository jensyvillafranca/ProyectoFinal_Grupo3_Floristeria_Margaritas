using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class historialEntregasModel
    {
        public int idpedido {  get; set; }
        public DateTime fechapedido { get; set; }
        public string? nombrescliente { get; set; }
        public double ganancia { get; set; }
        public int idestadopedido { get; set; }
        public int calificacion { get; set; } = 0;
        public string? motivocalificacion { get; set; }
    }
}
