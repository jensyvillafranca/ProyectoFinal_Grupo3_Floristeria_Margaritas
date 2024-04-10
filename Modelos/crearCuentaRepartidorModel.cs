using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class crearCuentaRepartidorModel
    {
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public string? correo { get; set; }
        public string? telefono { get; set; }
        public string? dni { get; set; }
        public string? fechanacimiento { get; set; }
        public int iddepartamento { get; set; }
        public int idciudad {  get; set; }
        public int genero { get; set; }
        public int licencia_motocicleta { get; set; }
        public int licencia_pesada { get; set; }
        public int licencia_liviana { get; set; }
        public List<string>? base64Images {  get; set; }
        public string? Codigo { get; set; }
    }
}
