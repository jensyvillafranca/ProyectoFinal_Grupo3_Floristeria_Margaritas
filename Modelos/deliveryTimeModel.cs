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
