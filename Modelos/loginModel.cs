/*
 * Descripción:
 * Este código define una clase llamada loginModel dentro del espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos.
 * Esta clase representa un modelo para el inicio de sesión, con propiedades que incluyen el ID del usuario, el nombre de usuario, la contraseña y el ID del tipo de usuario.
 */

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
