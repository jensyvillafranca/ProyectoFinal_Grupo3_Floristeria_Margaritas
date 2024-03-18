using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class pedidoModel
    {
        public int idpedido { get; set; }
        public DateTime fechapedido { get; set; }
        public string? fk_idrepartidor { get; set; }
        public string? nombresrepartidor { get; set; }
        public string? telefonorepartidor { get; set; }
        public string? idusuariorepartidor { get; set; }
        public int fk_idcliente { get; set; }
        public string? nombrescliente { get; set; }
        public int fk_iddireccion { get; set; }
        public string? direccion { get; set; }
        public int fk_idsucursal { get; set; }
        public string? nombresucursal { get; set; }
        public string? direccionsucursal { get; set; }
        public double comisionrepartidor { get; set; }
        public double subtotal { get; set; }
        public double isv { get; set; }
        public double envio { get; set; }
        public double total { get; set; }
        public double propina { get; set; }
        public int idestadopedido { get; set; }
        public DateTime fechaestimadaentrega { get; set; }
        public string? fechaentregado { get; set; }
        public string? notapedido { get; set; }
        public string? calificacion { get; set; }
        public string? motivocalificacion { get; set; }
        public string? titulonota { get; set; }
        public int fk_idtarjeta { get; set; }
        public string? numerotarjeta { get; set; }
        public string? nombretarjeta { get; set; }
    }
}
