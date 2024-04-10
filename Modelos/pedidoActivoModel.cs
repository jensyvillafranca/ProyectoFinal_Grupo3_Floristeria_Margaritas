using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class pedidoActivoModel
    {
        public string? dir_direccion {  get; set; }
        public string? suc_nombresucursal { get; set;}
        public int ped_idestadopedido { get; set; }
        public string? ped_enlacefoto { get; set; }
        public string? cli_nombrecompleto { get; set; }
        public int ped_idpedido { get; set; }
    }
}
