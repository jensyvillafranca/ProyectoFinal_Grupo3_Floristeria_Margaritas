using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class PagoDireccionTotalesModel
    {
        public double TotalPrecio { get; set; }
        public double ISV { get; set; }
        public double Envio { get; set; }
        public double Total { get; set; }
    }
}
