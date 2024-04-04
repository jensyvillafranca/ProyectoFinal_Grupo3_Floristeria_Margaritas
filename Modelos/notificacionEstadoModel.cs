/*
 * Descripción:
 * Este código define una clase llamada notificacionEstadoModel dentro del espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos.
 * Esta clase representa un modelo para una notificación de estado de pedido, con propiedades que incluyen el ID de la notificación del pedido, el título, el cuerpo, el ID del pedido,
 * la fecha de creación, el ID del estado del pedido, la lectura y el ID del cliente.
 */

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
