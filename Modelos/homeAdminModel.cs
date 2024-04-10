using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class homeAdminModel
    {
        public int pedidosentregados {  get; set; }
        public int pedidospendientes { get; set; }
        public int pedidoscancelados { get; set; }
        public string? totalpedidos { get; set; }
    }
}
