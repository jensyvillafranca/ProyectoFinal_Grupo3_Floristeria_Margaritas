/*
 * Descripción:
 * Este código define una clase llamada recuperarPassModel dentro del espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos.
 * Esta clase representa el modelo para recuperar contraseña e incluye el código de recuperación y el correo electrónico del usuario.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class recuperarPassModel
    {
        public string? Codigo {  get; set; }
        public string? Correo {  get; set; }
    }
}
