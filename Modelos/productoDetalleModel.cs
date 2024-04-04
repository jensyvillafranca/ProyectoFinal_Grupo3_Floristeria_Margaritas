/*
 * Descripción:
 * Este código define una clase llamada productoDetalleModel dentro del espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos.
 * Esta clase representa un modelo para los detalles de un producto en un pedido, incluyendo información como el ID del producto, la cantidad,
 * el precio unitario del producto, el precio total, el descuento aplicado, el nombre del producto y el enlace a la foto del producto.
 */

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
