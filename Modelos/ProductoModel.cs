/*
 * Descripción:
 * Este código define una clase llamada ProductoModel dentro del espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos.
 * Esta clase representa un modelo para un prod ndo información como el ID del producto, el nombre, la categoría, el precio de venta,
 * el stock disponible, el enlace a la foto del producto, el descuento aplicado y una descripción del producto.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class ProductoModel
    {
        public int idproducto { get; set; }
        public string? nombreproducto { get; set; }
        public string? categoria { get; set; }
        public string? precioventa { get; set; }
        public int stock { get; set; }
        public string? enlacefoto { get; set; }
        public string? descuento { get; set; }
        public string? descripcion { get; set; }

    }
}
