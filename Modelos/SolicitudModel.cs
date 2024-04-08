using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class SolicitudModel
    {
        public int idsolicitud { get; set; }
        public string? nombrecompleto { get; set; }
        public string? nombre {  get; set; }
        public string? apellido { get; set; }
        public string? correo { get; set; }
        public string? telefono { get; set; }
        public string? dni { get; set; }
        public DateTime fechanacimiento { get; set; }
        public string? departamento { get; set; }
        public int iddepartamento { get; set; }
        public int idciudad { get; set; }
        public string? genero { get; set; }
        public int licencia_motocicleta { get; set; }
        public int licencia_pesada { get; set; }
        public int licencia_liviana { get; set; }
        public DateTime fechasolicitud { get; set; }
        public int idestado { get; set; }
    }
}
