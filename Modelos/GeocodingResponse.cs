/*
 * Descripción:
 * Este código define una serie de clases dentro del espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos para manejar respuestas de geocodificación.
 * La clase GeocodingResponse representa la respuesta de geocodificación, que contiene una lista de resultados de geocodificación.
 * La clase GeocodingResult representa un resultado de geocodificación, que contiene información de geometría.
 * La clase Geometry representa la geometría de un resultado de geocodificación, que contiene información de ubicación.
 * La clase Location representa la ubicación de un resultado de geocodificación, con coordenadas de latitud y longitud.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class GeocodingResponse
    {
        public List<GeocodingResult> Results { get; set; }
    }
    public class GeocodingResult
    {
        public Geometry Geometry { get; set; }
    }

    public class Geometry
    {
        public Location Location { get; set; }
    }

    public class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
