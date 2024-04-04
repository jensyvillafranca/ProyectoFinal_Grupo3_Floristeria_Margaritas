/*
 * Descripción:
 * Este código define una clase llamada crearCuentaModel en el espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos.
 * La clase representa un modelo de datos para la creación de una cuenta, con propiedades para el nombre, apellido, correo electrónico, teléfono, usuario, contraseña y código de verificación.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class crearCuentaModel
    {
        public string? Nombre {  get; set; }
        public string? Apellido { get; set; }
        public string? Correo { get; set; }
        public string? Telefono {  get; set; }
        public string? Usuario { get; set; }
        public string? Contrasenia { get; set; }
        public string? Codigo { get; set; }
    }
}
