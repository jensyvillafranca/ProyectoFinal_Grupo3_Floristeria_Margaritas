using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class ProductUpdateResponse
    {
        public int IdProducto { get; set; }
        public string PrecioVenta { get; set; }
        public string Descuento { get; set; }
    }
}
