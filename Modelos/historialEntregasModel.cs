/*
 * Descripción:
 * Este código define una clase llamada historialEntregasModel dentro del espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos.
 * Esta clase representa un modelo para el historial de entregas, con propiedades que incluyen el ID del pedido, la fecha del pedido, los nombres del cliente, las ganancias, el ID del estado del pedido, la calificación y el motivo de la calificación.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class historialEntregasModel
    {
        public int idpedido {  get; set; }
        public DateTime fechapedido { get; set; }
        public string? nombrescliente { get; set; }
        public double ganancia { get; set; }
        public int idestadopedido { get; set; }
        public int calificacion { get; set; } = 0;
        public string? motivocalificacion { get; set; }
    }
}
