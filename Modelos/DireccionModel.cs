using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class DireccionModel
    {
        public int iddireccion { get; set; }
        public string? direccion { get; set; }
        public string? ciudad { get; set; }
        public string? departamento { get; set; }
        public int fk_idcliente { get; set; }
        public string? descripcion { get; set; }
        public string? longitud { get; set; }
        public string? latitude { get; set; }
        public string? referencia { get; set; }
    }
}
