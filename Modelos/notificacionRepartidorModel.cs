/*
 * Descripción:
 * Este código define una clase llamada notificacionRepartidorModel dentro del espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos.
 * Esta clase representa un modelo para una notificación de repartidor, con propiedades que incluyen el ID de la notificación del repartidor, el título, el cuerpo, el ID del pedido,
 * la fecha de creación, el estado de lectura, el ID del repartidor y el tipo de notificación.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class notificacionRepartidorModel
    {
        public int idnotificacionrepartidor { get; set; }
        public string? titulo { get; set; }
        public string? cuerpo { get; set; }
        public int idpedido { get; set; }
        public DateTime created { get; set; }
        public int lectura { get; set; }
        public int idrepartidor { get; set; }
        public int tipo { get; set; }
    }
}
