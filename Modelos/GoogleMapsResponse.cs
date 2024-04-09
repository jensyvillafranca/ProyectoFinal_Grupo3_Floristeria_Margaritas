using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class GoogleMapsResponse
    {
        [JsonProperty("routes")]
        public List<Ruta> Rutas { get; set; }
    }
    public class Ruta
    {
        [JsonProperty("overview_polyline")]
        public OverviewPolyline OverviewPolyline { get; set; }

        [JsonProperty("legs")]
        public List<Legs> Legs { get; set; }
    }
    public class OverviewPolyline
    {
        [JsonProperty("points")]
        public string Points { get; set; } //Viene del response la API de google
    }

    public class Legs
    {
        [JsonProperty("start_location")]
        public Location StartLocation { get; set; }

        [JsonProperty("end_location")]
        public Location EndLocation { get; set; }
    }

    
}
