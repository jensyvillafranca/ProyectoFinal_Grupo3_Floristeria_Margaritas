/*
 * Descripción:
 * Este código define una clase llamada pedidoModel dentro del espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos.
 * Esta clase representa un modelo para los detalles de un pedido, incluyendo información como el ID del pedido, la fecha de pedido,
 * los detalles del repartidor, los detalles del cliente, los detalles de la dirección de entrega, los detalles de la sucursal,
 * los montos financieros asociados al pedido, el estado del pedido, la fecha estimada de entrega, entre otros.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class pedidoModel
    {
        public int idpedido { get; set; }
        public DateTime fechapedido { get; set; }
        public string? fk_idrepartidor { get; set; }
        public string? nombresrepartidor { get; set; }
        public string? telefonorepartidor { get; set; }
        public string? idusuariorepartidor { get; set; }
        public int fk_idcliente { get; set; }
        public string? nombrescliente { get; set; }
        public int fk_iddireccion { get; set; }
        public string? direccion { get; set; }
        public int fk_idsucursal { get; set; }
        public string? nombresucursal { get; set; }
        public string? direccionsucursal { get; set; }
        public double comisionrepartidor { get; set; }
        public double subtotal { get; set; }
        public double isv { get; set; }
        public double envio { get; set; }
        public double total { get; set; }
        public double propina { get; set; }
        public int idestadopedido { get; set; }
        public DateTime fechaestimadaentrega { get; set; }
        public string? fechaentregado { get; set; }
        public string? notapedido { get; set; }
        public string? calificacion { get; set; }
        public string? motivocalificacion { get; set; }
        public string? titulonota { get; set; }
        public int fk_idtarjeta { get; set; }
        public string? numerotarjeta { get; set; }
        public string? nombretarjeta { get; set; }
        public string? factura { get; set; }
    }
}
