/*
 * Descripción:
 * Este código define una clase llamada codigoVerificacionModel en el espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos.
 * La clase representa un modelo de datos para el código de verificación, con propiedades para el estado, el mensaje y el código de verificación.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class codigoVerificacionModel
    {
        public string? status {  get; set; }
        public string? message {  get; set; }
        public string? verification_code { get; set; }
    }
}
