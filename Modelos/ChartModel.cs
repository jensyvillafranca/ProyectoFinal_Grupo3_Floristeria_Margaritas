using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class ChartModel
    {
        public string Pedidos { get; set; }
        public double Counts { get; set; }

        public ChartModel(string name, double count)
        {
            Pedidos = name;
            Counts = count;
        }
    }
}
