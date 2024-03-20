using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class productoDetalleModel
    {
        public int idproducto { get; set; }
        public int cantidad { get; set; }
        public double precioproducto {  get; set; }
        public double preciototal {  get; set; }
        public int descuento {  get; set; }
        public string? nombreproducto {  get; set; }
        public string? enlacefoto {  get; set; }
    }
}
