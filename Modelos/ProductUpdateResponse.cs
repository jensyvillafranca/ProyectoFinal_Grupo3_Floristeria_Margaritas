/*
 * Descripción:
 * Este código define una clase llamada ProductUpdateResponse dentro del espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos.
 * Esta clase representa la respuesta de una actualización de producto e incluye el ID del producto actualizado, el nuevo precio de venta y el nuevo descuento.
 */

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
