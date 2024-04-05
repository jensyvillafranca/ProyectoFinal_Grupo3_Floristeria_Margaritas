using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class ClienteModel
    {
        public int idcliente {  get; set; }
        public string? nombrecompleto { get; set; }
        public string? correo {  get; set; }
        public string? usuario { get; set; }
        public string? enlacefoto { get; set; }
        public string? telefono { get; set; }
    }
}
