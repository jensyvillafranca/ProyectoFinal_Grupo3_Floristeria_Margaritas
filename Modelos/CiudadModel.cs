/*
 * Descripción:
 * Este código define una clase llamada CiudadModel en el espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos.
 * La clase representa un modelo de datos para una ciudad, con propiedades como el ID de la ciudad, el nombre de la ciudad y el ID del departamento al que pertenece.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class CiudadModel
    {
        public int idciudad {  get; set; }
        public string? ciudad {  get; set; }
        public int fk_iddepartamento { get; set; }
    }
}
