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
