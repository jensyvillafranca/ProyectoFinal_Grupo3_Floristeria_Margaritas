/*
 * Descripción:
 * Este código define una clase llamada deliveryTimeModel en el espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos.
 * La clase representa un modelo de datos para el tiempo de entrega, con una propiedad para la hora de entrega.
 * Utiliza la biblioteca Newtonsoft.Json para realizar la serialización y deserialización de objetos JSON.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class deliveryTimeModel
    {
        [JsonProperty("deliverytime")]
        public DateTime deliverytime { get; set; }
    }
}
