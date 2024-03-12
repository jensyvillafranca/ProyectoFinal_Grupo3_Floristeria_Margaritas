using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class loginModel
    {
        public int idusuario { get; set; }
        public string? usuario {  get; set; }
        public string? contrasenia { get; set; }
        public int fk_idtipousuario { get; set; }
    }
}
