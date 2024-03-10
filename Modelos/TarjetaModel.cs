using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class TarjetaModel
    {
        public int idtarjeta {  get; set; }
        public string? numerotarjeta { get; set; }
        public string? fechavencimiento { get; set; }
        public int fk_idcliente { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
    }
}
