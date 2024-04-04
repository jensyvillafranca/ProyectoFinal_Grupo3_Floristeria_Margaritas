/*
 * Descripción:
 * Este código define una clase llamada PagoDireccionTotalesModel dentro del espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos.
 * Esta clase representa un modelo para los totales de pago y dirección, con propiedades que incluyen el precio total, el impuesto sobre el valor agregado (ISV),
 * el costo de envío y el total a pagar.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class PagoDireccionTotalesModel
    {
        public double TotalPrecio { get; set; }
        public double ISV { get; set; }
        public double Envio { get; set; }
        public double Total { get; set; }
    }
}
