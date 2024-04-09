/*
 * Descripción:
 * Este código define una clase llamada FiltroModel en el espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos.
 * La clase representa un modelo de datos para un filtro, con una propiedad para el ID de la categoría y otra para el nombre de la categoría.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class FiltroModel
    {
        public int idcategoria { get; set; }
        public string? categoria { get; set; }
    }
}
